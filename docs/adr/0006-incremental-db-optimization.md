# ADR-0006: OptimizaciÃ³n Incremental sin RefactorizaciÃ³n Total
Contexto: Cuello de botella en BD central con Alta Disponibilidad (HA). 8 meses para refactorizar es inaceptable.

DecisiÃ³n: Plan de 4 fases: 1. DiagnÃ³stico (Explain Analyze), 2. Offloading de lectura a rÃ©plicas HA, 3. Caching lateral, 4. Tuning de Ã­ndices.

JustificaciÃ³n: Aprovecha la infraestructura de HA existente para derivar el trÃ¡fico de reportes y consultas pesadas, aliviando el nodo primario de inmediato.
