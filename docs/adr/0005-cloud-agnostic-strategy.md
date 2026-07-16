# ADR-0005: Estrategia de Despliegue Cloud Agnostic vs. Native
Contexto: PolÃ­tica corporativa de independencia de nube.

DecisiÃ³n:

Stack Java/AWS (AgnÃ³stico): Despliegue en Kubernetes (EKS) utilizando Apache Kafka como bus agnÃ³stico.

Stack.NET/Azure (Platform-centric): Despliegue en Azure Container Apps utilizando Azure Service Bus (justificado por costo operativo reducido).

JustificaciÃ³n: Para AWS se elige agnÃ³stico para evitar el alto costo de salida (Exit Cost) de Kinesis. Para Azure se justifica el uso nativo por el ahorro inmediato en OpEx de administraciÃ³n.
