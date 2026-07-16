# Fase: AnÃ¡lisis Financiero y JustificaciÃ³n de Nube

Para el Ejercicio 4, se comparan dos estrategias financieras: una soluciÃ³n Cloud Agnostic (basada en el Stack Java/AWS con K8s) y una soluciÃ³n Cloud Native (basada en el Stack.NET/Azure).

## 1. Comparativa de TCO (Proyectado a 3 aÃ±os)

| CategorÃ­a de Costo | SoluciÃ³n A: Java/AWS (AgnÃ³stica) | SoluciÃ³n B: .NET/Azure (Nativa) |
| :--- | :--- | :--- |
| **Costo de ImplementaciÃ³n (DÃ­a 1)** | Alto: ConfiguraciÃ³n de ClÃºster K8s y Kafka manual. | Bajo: Uso de servicios administrados (Azure Stream Analytics). |
| **Costo Operativo Mensual (OpEx)** | Medio: Pago por cÃ³mputo bruto (instancias EC2). | Medio-Alto: Pago por volumen procesado y licencias de servicios. |
| **Flexibilidad de Precios** | Alta: Permite Cloud Arbitrage (mover carga segÃºn precios de spot). | Baja: Sujeto a los aumentos de tarifa de un solo proveedor. |
| **Costo de Salida (Exit Cost)** | Cercano a $0: Portabilidad total de contenedores y cÃ³digo. | Muy Alto: RefactorizaciÃ³n total de cÃ³digo dependiente de APIs propietarias. |

## 2. CÃ¡lculo del Costo de Salida (Exit Cost)
El "Costo de Salida" es la mÃ©trica de riesgo mÃ¡s importante para este proyecto. Se define mediante la siguiente fÃ³rmula:

$$ExitCost = RefactoringHours \times HourlyRate + DataEgressFees$$

**RefactorizaciÃ³n:** En la SoluciÃ³n B (Nativa), migrar fuera de Azure implicarÃ­a reescribir la lÃ³gica de procesamiento de eventos, estimada en ~400 horas hombre. En la SoluciÃ³n A (AgnÃ³stica), el costo es solo de re-despliegue de scripts de Helm/Terraform.

**Tarifas de Salida (Egress Fees):** Los proveedores de nube cobran entre $0.08 y $0.12 por GB al mover datos fuera de su red. Una soluciÃ³n agnÃ³stica facilita mantener los datos en formatos abiertos (Parquet/Avro) que minimizan este impacto.

## 3. Veredicto EstratÃ©gico (Basado en CAF)
Siguiendo el Cloud Adoption Framework (CAF) de Microsoft, se recomienda la SoluciÃ³n A (AgnÃ³stica) para el despliegue en AWS para proteger la soberanÃ­a de datos de Arquitectura de Referencia, aceptando un costo inicial de configuraciÃ³n un 20% mÃ¡s alto a cambio de una reducciÃ³n del 90% en el riesgo de lock-in financiero a largo plazo.
