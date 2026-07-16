Fase de AnÃ¡lisis: Supuestos de Arquitectura
Debido a la naturaleza del examen y la imposibilidad de realizar preguntas aclaratorias, se establecen los siguientes supuestos que rigen el diseÃ±o de la soluciÃ³n:

1. Volumen de Datos y TrÃ¡fico
Carga de Datos: Se asume un volumen inicial de 1 millÃ³n de registros por fuente para los Ejercicios 1 y 2.

Concurrencia: Se estima un pico de 500 usuarios concurrentes para la plataforma de pagos (Ejercicio 3).

Tasa de IngestiÃ³n: Se asume que los cambios en las fuentes externas no superan los 100 eventos por segundo, permitiendo el procesamiento en un solo contenedor.

2. Acceso a Fuentes Legadas (Fuentes A, B, C, D)
Conectividad: Se asume que las bases de datos externas permiten conexiones directas vÃ­a JDBC/ODBC o drivers nativos.

Impacto en Origen: Se asume que las fuentes permiten lecturas periÃ³dicas sin degradar su rendimiento operativo.

3. Infraestructura y Despliegue
ContainerizaciÃ³n: Se asume que el entorno de ejecuciÃ³n final soporta Docker y Kubernetes (K8s).

Persistencia: Se asume disponibilidad de almacenamiento SSD local para los Read-Models, garantizando IOPS altos para DuckDB.

4. Negocio y Pagos
Pasarelas de Pago: Se asume el uso de APIs externas (Stripe, MercadoPago) que manejan el cumplimiento de PCI-DSS, dejando al sistema solo la responsabilidad de auditorÃ­a y estados.
