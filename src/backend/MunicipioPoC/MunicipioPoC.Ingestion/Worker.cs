using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MunicipioPoC.Core;

namespace MunicipioPoC.Ingestion;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Channel<LegacyRecord> _channel;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        // Bounded capacity of 100 items to force backpressure under load
        var options = new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait, // Tells the producer to wait (backpressure)
            SingleWriter = false,
            SingleReader = true
        };
        _channel = Channel.CreateBounded<LegacyRecord>(options);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Ingestion Service with Bounded Channel (Capacity: 100)");

        // Start Producer (Ingesting from A, B, C, D)
        var producerTask = StartProducerAsync(stoppingToken);

        // Start Consumer (Writing to local DuckDB/Disk)
        var consumerTask = StartConsumerAsync(stoppingToken);

        await Task.WhenAll(producerTask, consumerTask);
    }

    private async Task StartProducerAsync(CancellationToken stoppingToken)
    {
        var sources = new[] { "Source_A", "Source_B", "Source_C", "Source_D" };
        var random = new Random();

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Generate a random record from one of the sources
                var source = sources[random.Next(sources.Length)];
                var record = new LegacyRecord
                {
                    Id = Guid.NewGuid(),
                    Source = source,
                    RawData = $"{source} transaction data payload...",
                    ProcessedAt = DateTime.UtcNow,
                    Amount = (decimal)(random.NextDouble() * 1000)
                };

                _logger.LogInformation("Producer: Ingesting from {Source}. Bounded Channel Count: {Count}", 
                    source, _channel.Reader.Count);

                // Write to channel. If the channel is full, this will await (Backpressure!)
                await _channel.Writer.WriteAsync(record, stoppingToken);

                // Simulate high-rate ingestion (e.g. every 50ms)
                await Task.Delay(50, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Producer task cancelled.");
        }
        finally
        {
            _channel.Writer.Complete();
        }
    }

    private async Task StartConsumerAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (var record in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                // Simulate slow I/O write (e.g. 150ms write time to local DuckDB storage)
                // This slow write relative to the fast producer (50ms) will quickly fill the channel
                // and trigger the backpressure mechanism.
                await Task.Delay(150, stoppingToken);

                _logger.LogInformation("Consumer: Successfully wrote {Id} from {Source} to DuckDB. Amount: {Amount:C}", 
                    record.Id, record.Source, record.Amount);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Consumer task cancelled.");
        }
    }
}
