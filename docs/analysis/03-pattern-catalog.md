# CatÃ¡logo de Patrones de Arquitectura

Este documento contiene la profundizaciÃ³n tÃ©cnica de cada patrÃ³n de arquitectura aplicado en el proyecto. Esta informaciÃ³n respaldarÃ¡ las decisiones tomadas durante la defensa presencial.

## 1. PatrÃ³n CQRS (Command Query Responsibility Segregation)
- **Problema:** La Fuente D tiene una latencia de 40 minutos. Si el usuario hace una consulta unificada (Ventas + AuditorÃ­a), el sistema tardarÃ­a 40 minutos en responder si consultamos en tiempo real.
- **SoluciÃ³n:** Separamos la Escritura (IngestiÃ³n asÃ­ncrona desde las fuentes legacy) de la Lectura (Consulta sobre DuckDB en disco).
- **Defensa:** *"UsÃ© CQRS para garantizar el modo Fast. El usuario consulta una 'ProyecciÃ³n' o vista materializada en DuckDB que es instantÃ¡nea, mientras que el 'Command side' (Redpanda Connect) se encarga de lidiar con la lentitud de las bases de datos externas en segundo plano"*.

## 2. PatrÃ³n Backpressure (Flujo Reactivo)
- **Problema:** La restricciÃ³n de 4GB de RAM. Si las fuentes externas envÃ­an datos mÃ¡s rÃ¡pido de lo que DuckDB puede escribir en disco, el contenedor explotarÃ¡ por falta de memoria (OOM).
- **SoluciÃ³n:** Implementamos un mecanismo de control de flujo donde el suscriptor (la base de datos local) le dice al publicador (fuente legacy) cuÃ¡ntos datos puede recibir.
- **Defensa:** *"No es un simple ETL. Es un pipeline con Backpressure nativo (vÃ­a Redpanda Connect o .NET Channels). Si el disco se satura, el sistema deja de leer de la fuente automÃ¡ticamente, protegiendo los 4GB de RAM de forma determinÃ­stica"*.

## 3. PatrÃ³n Outbox (Transaccionalidad en Pagos)
- **Problema:** En el Ejercicio 3, si el sistema confirma un pago pero falla la red antes de registrarlo en el sistema de auditorÃ­a, perdemos integridad financiera (el "Dual-Write Problem").
- **SoluciÃ³n:** El pago y el evento de auditorÃ­a se guardan en la misma transacciÃ³n de la base de datos local (una tabla outbox). Luego, un proceso independiente los publica de forma garantizada.
- **Defensa:** *"Para la plataforma de pagos, la consistencia es no-negociable. El patrÃ³n Outbox garantiza que ningÃºn evento de pago se pierda, incluso si el bus de mensajes (Kafka/RabbitMQ) estÃ¡ caÃ­do momentÃ¡neamente"*.

## 4. PatrÃ³n Strangler Fig (EvoluciÃ³n del Monolito)
- **Problema:** El dilema de 3 meses vs. 12 meses. No podemos hacer microservicios hoy por el tiempo, pero un monolito "puro" serÃ¡ inmanejable en un aÃ±o.
- **SoluciÃ³n:** Creamos un Monolito Modular. Cuando llegue el mes 9, empezaremos a "estrangular" las funcionalidades del monolito, extrayendo el mÃ³dulo de pagos hacia su propio microservicio de forma incremental.
- **Defensa:** *"Mi estrategia no es estÃ¡tica. Es una Arquitectura Evolutiva. El patrÃ³n Strangler Fig nos permite cumplir con el negocio hoy y escalar tÃ©cnicamente maÃ±ana sin riesgos de 'Big Bang migration'"*.

## 5. PatrÃ³n Circuit Breaker (Resiliencia)
- **Problema:** Si intentamos el modo Fresh (consulta en tiempo real a las 4 fuentes) y una de las bases de datos externas estÃ¡ caÃ­da, el API de Arquitectura de Referencia se quedarÃ­a "colgado" esperando un timeout.
- **SoluciÃ³n:** Si una fuente falla repetidamente, el circuito se abre y el sistema deja de intentar consultarla, devolviendo inmediatamente el Ãºltimo dato conocido del modo Fast.
- **Defensa:** *"Implementamos Circuit Breakers para evitar fallos en cascada. Si el SQL Server externo falla, mi arquitectura protege la experiencia del usuario devolviendo datos locales con una advertencia de frescura"*.

---

## Resumen Ejecutivo

| Escenario | PatrÃ³n Aplicado | Beneficio Principal |
| :--- | :--- | :--- |
| IntegraciÃ³n HeterogÃ©nea | Adapter | Escalabilidad lÃ³gica (Fuentes A, B, C, D). |
| RestricciÃ³n 4GB RAM | Backpressure | Estabilidad operativa y eficiencia de recursos. |
| Latencia Diferencial | CQRS | Respuesta instantÃ¡nea en modo Fast. |
| EvoluciÃ³n 3 a 12 meses | Strangler Fig | MitigaciÃ³n de riesgo en la entrega y escalabilidad futura. |
| Integridad de Pagos | Transactional Outbox | Consistencia eventual garantizada sin pÃ©rdida de datos. |
