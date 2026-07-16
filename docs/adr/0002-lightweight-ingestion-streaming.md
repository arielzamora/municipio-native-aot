# ADR-0002: IngestiÃ³n basada en Streaming con Backpressure
Contexto: Integrar 4 fuentes heterogÃ©neas con solo 4GB de RAM.

DecisiÃ³n:

Stack Java/AWS: Redpanda Connect (Benthos). Es un binario Ãºnico, ligero y declarativo.

Stack.NET/Azure:.NET Worker Services utilizando System.Threading.Channels.

JustificaciÃ³n: El uso de flujos (Streams) con Backpressure evita cargar datos masivos en memoria. El sistema solo procesa lo que la persistencia puede escribir, garantizando estabilidad en los 4GB de RAM.
