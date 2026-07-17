# ADR-0007: Arquitectura de Despliegue Nativo en Azure con Container Apps y Bicep

## Estado
Aceptado

## Contexto
El proyecto requiere desplegar la Prueba de Concepto (PoC) de la plataforma de consolidación de datos e ingesta de pagos en un entorno de nube pública. Se busca cumplir con requerimientos de alta disponibilidad (99.9% de uptime) y facilidad de mantenimiento, minimizando la sobrecarga operativa y los costos fijos iniciales (OpEx), pero asegurando que la solución sea portable.

## Decisión
Se opta por una arquitectura nativa en **Microsoft Azure** utilizando los siguientes componentes administrados:

1.  **Azure Container Apps (ACA)**: Para alojar los contenedores del API Gateway (`MunicipioPoC.Api`) y del servicio de ingesta (`MunicipioPoC.Ingestion`), configurados bajo la especificación de .NET Aspire.
2.  **Azure Service Bus (Standard)**: Como bus de mensajes empresarial para la cola de auditoría de pagos (`audit-payments-queue`), implementando el desacoplamiento del Outbox.
3.  **Azure SQL Database (Basic/Standard)**: Para la persistencia estructurada y transaccional del Ledger de pagos.
4.  **Azure Bicep**: Como lenguaje de Infraestructura como Código (IaC) para declarar y aprovisionar todos estos recursos de forma automatizada y repetible.

## Justificación
El uso de **Azure Container Apps** en lugar de un clúster de Kubernetes auto-administrado (como AKS o EKS) elimina la complejidad de administración de red, parches de seguridad y nodos del clúster, reduciendo la carga de mantenimiento a cero. 

De manera similar, **Azure Service Bus** ofrece colas empresariales administradas con un costo operativo mucho menor en comparación con levantar un clúster de Apache Kafka dedicado en esta fase de la PoC. La portabilidad se mantiene a nivel de aplicación mediante el uso de contenedores Docker estándar y la orquestación unificada de **.NET Aspire**.

## Consecuencias
*   **Positivas**:
    *   **Menor Time-to-Market**: Aprovisionamiento inmediato de la infraestructura en minutos.
    *   **Menor OpEx**: Pago por consumo real en Container Apps y costo base bajo para SQL y Service Bus.
    *   **Operación simplificada**: No requiere un rol dedicado a la administración de infraestructura de Kubernetes o Kafka.
*   **Negativas**:
    *   **Dependencia de APIs de Azure**: La comunicación con Service Bus y Azure SQL utiliza SDKs de Azure, aunque se abstraen mediante interfaces en el código para facilitar una potencial migración a RabbitMQ o PostgreSQL.
