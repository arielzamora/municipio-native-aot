# ADR-0003: Persistencia de Read-Models en Disco vÃ­a DuckDB
Contexto: Se requiere un modo "Fast" que responda instantÃ¡neamente sin consultar fuentes remotas y que los datos vivan en disco.

DecisiÃ³n: DuckDB como motor analÃ­tico embebido.

JustificaciÃ³n: A diferencia de SQLite, DuckDB es columnar (OLAP). Permite hacer joins complejos entre las 4 fuentes en milisegundos. Soporta "spilling to disk", lo que permite procesar volÃºmenes que superan la RAM disponible.
