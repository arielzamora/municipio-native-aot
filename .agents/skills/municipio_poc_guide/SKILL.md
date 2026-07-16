---
name: Guía de Arquitectura PoC Municipio
description: Contexto técnico y reglas de diseño para la PoC de Municipio (.NET 9, DuckDB, Azure, Angular 19, Backpressure, SLA Freshness).
---
# Skill: Guía de Arquitectura PoC Municipio

Este skill proporciona el contexto y reglas de diseño técnico para la PoC de Municipio, asegurando que todos los agentes compartan las mismas decisiones arquitectónicas y restricciones.

## 1. Reglas Técnicas Críticas
- **Límite de Recursos**: El sistema no debe pasar de **4GB de RAM** en tiempo de ejecución.
- **Modo Fast**: Consultas a datos persistidos localmente en disco vía **DuckDB** en menos de **200ms**.
- **Modo Fresh**: Consultas bajo demanda a las fuentes externas (latencia hasta 5 segundos). Si una fuente está caída, el sistema debe activar el Circuit Breaker y servir datos históricos desde DuckDB.
- **SLA Compliance (Fórmula de Frescura)**:
  $$\text{Score}_i = \max\left(0, 100 \times \left(1 - \frac{T_{\text{actual}} - T_{\text{sincro}, i}}{T_{\text{SLA}, i}}\right)\right)$$
  - *Fuente D* tiene un SLA de retraso máximo de 40 minutos (`T_SLA = 40 min`).
- **Backpressure**: La ingesta de datos desde fuentes remotas debe usar un buffer reactivo (`System.Threading.Channels` en .NET) que ralentice o pause la lectura si el storage local (DuckDB) no puede procesar los datos a la velocidad de entrada.

## 2. Pila Tecnológica Azure & .NET
- **Orquestador de Desarrollo**: .NET Aspire (.NET 9).
- **Backend API**: ASP.NET Core con soporte para Native AOT.
- **Ingesta**: .NET Worker Services usando `System.Threading.Channels`.
- **Persistencia**:
  - Lecturas/Analítico: DuckDB embebido en disco.
  - Pagos/Ledger: Azure SQL Database con aislamiento transaccional y tabla de Outbox.
- **Mensajería**: Azure Service Bus (para eventos de auditoría y distribución del Outbox).
- **Frontend**: Angular 19 (Signals + OnPush + SignalR para actualizaciones push en tiempo real).
- **Despliegue Cloud**: Azure Container Apps (ACA).
