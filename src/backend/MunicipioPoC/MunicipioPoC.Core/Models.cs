using System;

namespace MunicipioPoC.Core
{
    public enum SlaStatus
    {
        Green,
        Yellow,
        Red
    }

    public class FreshnessScore
    {
        public string SourceName { get; set; } = string.Empty;
        public double Score { get; set; }
        public DateTime LastSyncTime { get; set; }
        public SlaStatus Status { get; set; }
    }

    public static class FreshnessCalculator
    {
        public static FreshnessScore Calculate(string sourceName, DateTime lastSyncTime, TimeSpan slaLimit)
        {
            var now = DateTime.UtcNow;
            var elapsed = now - lastSyncTime;
            
            // Score = max(0, 100 * (1 - (T_actual - T_sincro) / T_SLA))
            double scoreRatio = 1 - (elapsed.TotalSeconds / slaLimit.TotalSeconds);
            double score = Math.Max(0, 100 * scoreRatio);
            
            SlaStatus status = SlaStatus.Red;
            if (score >= 80)
            {
                status = SlaStatus.Green;
            }
            else if (score >= 50)
            {
                status = SlaStatus.Yellow;
            }

            return new FreshnessScore
            {
                SourceName = sourceName,
                Score = Math.Round(score, 2),
                LastSyncTime = lastSyncTime,
                Status = status
            };
        }
    }

    public class LegacyRecord
    {
        public Guid Id { get; set; }
        public string Source { get; set; } = string.Empty;
        public string RawData { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
        public decimal Amount { get; set; }
    }
}
