# ADR-0004: Monolito Modular para Plataforma de Pagos
Contexto: Necesidad de salida en 3 meses, con escalabilidad masiva en 12 meses.

DecisiÃ³n:

Stack Java: Spring Modulith.

Stack.NET: Modular Monolith Architecture (separaciÃ³n por proyectos/librerÃ­as).

JustificaciÃ³n: Un monolito modular permite desarrollar rÃ¡pido sin la complejidad de red de los microservicios (cumpliendo los 3 meses), pero mantiene fronteras de dominio estrictas que permiten migrar a microservicios al mes 12 sin refactorizar.
