Fase de AnÃ¡lisis: Requerimientos No Funcionales (NFR)
Para asegurar el Ã©xito de la arquitectura propuesta, se priorizan los siguientes atributos de calidad basados en las restricciones del cliente:

1. Eficiencia de Recursos (RestricciÃ³n CrÃ­tica)
RAM: El componente de ingestiÃ³n y consulta local no debe exceder los 4GB de memoria RAM en tiempo de ejecuciÃ³n.

CPU: El procesamiento de streams debe ser optimizado para no saturar los nÃºcleos disponibles mediante el uso de Backpressure.

2. Rendimiento y Latencia
Modo Fast: El tiempo de respuesta del API para consultas "Fast" debe ser inferior a 200ms, ya que consume datos persistidos en disco local.

Modo Fresh: El tiempo de respuesta puede extenderse hasta los 5 segundos, dependiendo de la latencia de las fuentes remotas.

3. Escalabilidad y EvoluciÃ³n
Escalabilidad LÃ³gica: La arquitectura de ingestiÃ³n debe permitir agregar una "Fuente E" mediante configuraciÃ³n (YAML) sin modificar el cÃ³digo fuente.

EvoluciÃ³n del Monolito: El sistema de pagos debe estar desacoplado internamente para permitir la migraciÃ³n a Microservicios en el mes 12 con un esfuerzo de refactorizaciÃ³n menor al 10%.

4. Disponibilidad y Resiliencia
Tolerancia a Fallos: Si una de las 4 fuentes falla, el sistema debe seguir respondiendo con los datos de las otras 3, marcando el "Freshness Score" correspondiente.

Persistencia: Los datos consolidados deben ser inmutables en disco para permitir recuperaciones rÃ¡pidas ante reinicios del sistema.
