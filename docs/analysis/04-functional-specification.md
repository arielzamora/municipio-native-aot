# Especificación Funcional: Consolidación de Fuentes y Reglas de Negocio
**Autor**: Agente Analista de Negocio y Datos
**Fecha**: Julio 2026

Este documento define la estructura de las fuentes legadas y las reglas de negocio que rigen la ingesta y la consolidación de datos para la PoC del Municipio.

---

## 1. Mapeo y Catálogo de Fuentes Legacy
El sistema consolida datos de 4 fuentes heterogéneas. Cada una tiene un comportamiento de latencia y un SLA de frescura asignado:

| Origen | Tipo de Base de Datos | Tipo de Información | Frecuencia de Cambio Estimada | Límite SLA de Frescura ($T_{SLA}$) |
| :--- | :--- | :--- | :--- | :--- |
| **Source_A** | SQL Server (Relacional) | Transacciones de Pago y Tasas | Alta (Eventos en tiempo real) | **10 minutos** |
| **Source_B** | MySQL (Relacional) | Registro de Contribuyentes | Media (Actualizaciones cada hora) | **20 minutos** |
| **Source_C** | MongoDB (NoSQL Documental) | Trámites Municipales | Media-Alta (Flujo constante) | **30 minutos** |
| **Source_D** | Archivos JSON (File Storage) | Auditoría Externa y Reportes | Baja (Una vez al día) | **40 minutos** |

---

## 2. Esquema Consolidado (Destino: DuckDB Embebido)
Para permitir consultas analíticas e instantáneas en el **Modo Fast**, los conectores del Worker de Ingesta normalizan las 4 fuentes en una única tabla columnar en DuckDB:

```sql
CREATE TABLE consolidated_events (
    id UUID PRIMARY KEY,
    source VARCHAR NOT NULL,       -- 'Source_A', 'Source_B', 'Source_C', 'Source_D'
    raw_data VARCHAR,              -- Payload en formato de texto plano o JSON serializado
    amount DECIMAL(18, 2),         -- Importe monetario normalizado
    processed_at TIMESTAMP         -- Fecha/hora de procesamiento (UTC)
);
```

---

## 3. Matriz de Umbrales del SLA de Frescura
El semáforo visual de frescura que consume la interfaz de usuario se calcula dinámicamente con el score del SLA:

*   🟢 **Verde (Estado Saludable)**: $\text{Score} \ge 80$.
    *   *Significado*: Los datos están actualizados y dentro del rango de tolerancia normal del municipio.
*   🟡 **Amarillo (Advertencia de Retraso)**: $50 \le \text{Score} < 80$.
    *   *Significado*: La fuente no se ha sincronizado en su intervalo esperado. Puede requerir atención preventiva.
*   🔴 **Rojo (Incumplimiento de SLA)**: $\text{Score} < 50$.
    *   *Significado*: Brecha crítica de datos. La consistencia temporal en Modo Fast se ha roto. Se sugiere forzar sincronización manual o revisar conectividad.
