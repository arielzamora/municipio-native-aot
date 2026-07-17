# Municipio Native AOT - Plataforma de Datos y Pagos (PoC)

Esta Prueba de Concepto (PoC) presenta una arquitectura de referencia moderna para la consolidación de datos a escala municipal y la auditoría de transacciones financieras, optimizada bajo el ecosistema de **.NET 9 (.NET Aspire)** y **Angular 19 (Signals)**.

---

## 🎯 Resumen Ejecutivo (Para Gerentes y Líderes de Negocio)

El proyecto resuelve el desafío de integrar y normalizar información proveniente de **4 fuentes legadas heterogéneas** (bases de datos relacionales, documentales y almacenamiento de archivos) bajo estrictas restricciones operativas:

1.  **Límite de Recursos Físicos**: Toda la lógica de ingesta, cálculo y lectura analítica local debe operar con un límite estricto de **4 GB de memoria RAM** en el contenedor.
2.  **Modo Fast (SLA de Lectura < 200ms)**: El panel de control debe responder de forma instantánea a las consultas analíticas consumiendo datos de una proyección inmutable en disco local, evitando consultas remotas costosas.
3.  **Modo Fresh (Consulta en Tiempo Real)**: Permite consultar en vivo a los orígenes externos con un límite de tiempo de hasta 5 segundos, protegiendo al usuario con mecanismos de **resiliencia (Circuit Breaker)** si alguna de las bases de datos externas experimenta caídas.
4.  **Auditoría de Pagos Inmutable**: Mantiene la consistencia de eventos financieros (500 transacciones por segundo estimadas para el mes 12) a través del patrón transaccional **Outbox**, delegando el cumplimiento de PCI-DSS a pasarelas externas.

---

## 🏛️ Decisiones Estratégicas de Nube: De Multicloud a Azure Nativo

Durante la fase de diseño inicial (detallada en los documentos ADR), se analizaron dos enfoques de nube:

*   **Evaluación Multicloud (Agnóstica)**: Se consideró un despliegue agnóstico basado en clústeres autoadministrados de Kubernetes (EKS/AKS) y Apache Kafka para evitar el bloqueo del proveedor (*Vendor Lock-in*). Si bien reduce a cero el *Exit Cost* a largo plazo, requiere un alto costo de implementación inicial y OpEx de administración.
*   **Enfoque Elegido (Azure Nativo + .NET 9)**: Para acelerar el tiempo de salida al mercado (estimado en 3 meses) y minimizar la carga operativa de administración, se optó por un enfoque nativo en la nube utilizando **Azure Container Apps (ACA)**, **Azure Service Bus** (para colas de mensajería del Outbox) y **Azure SQL**.
*   **Mitigación de Lock-in**: La portabilidad se garantiza a nivel de aplicación mediante el uso de contenedores Docker estándar y el framework **.NET Aspire**, que unifica la orquestación y el rastreo de dependencias de forma agnóstica para desarrollo local y despliegue final.

---

## 🗺️ Estructura del Monorepo

```text
/
├── .agents/                 # Configuración de Agentes Especialistas locales
│   ├── agents/              # Perfiles (Analista, Arquitecto, DevOps, QA, Fullstack)
│   └── skills/              # Skill compartida "Guía de Arquitectura PoC Municipio"
├── docs/                    # Documentación de diseño, diagramas C4 y ADRs
├── infra/                   # Plantillas de Infraestructura como Código (Azure Bicep)
└── src/
    ├── backend/             # Solución .NET 9 con .NET Aspire
    │   ├── MunicipioPoC.AppHost/          # Orquestador local del sistema
    │   ├── MunicipioPoC.Api/              # API Web Gateway y Endpoints de SLA
    │   ├── MunicipioPoC.Ingestion/        # Worker Service e ingesta con Backpressure
    │   └── MunicipioPoC.Core/             # Modelos y calculador matemático de SLA
    └── frontend/            # Aplicación SPA Angular 19 (Signals y OnPush)
```

---

## ⚡ Guías de Desarrollo e Ingesta
Toda la lógica de diseño (SLA, límites de RAM, DuckDB y Service Bus) se encuentra documentada en la skill:
*   📘 **Guía de Arquitectura PoC Municipio**: [municipio_poc_guide/SKILL.md](file:///.agents/skills/municipio_poc_guide/SKILL.md)

---

## 🚀 Inicio Rápido (Local)

Para correr la PoC local y simular el flujo en tiempo real, abrí dos terminales en la raíz del proyecto:

### 1. Iniciar el Backend (.NET Aspire)
```powershell
cd src/backend/MunicipioPoC
dotnet run --project MunicipioPoC.AppHost
```
*(Esto levantará la API en el puerto `5222` y el orquestador Aspire. En los logs verás la URL del dashboard de observabilidad).*

### 2. Iniciar el Frontend (Angular 19)
```powershell
cd src/frontend
npm start
```
*(Abre tu navegador en **http://localhost:4200** para ver el panel con los semáforos de SLA interactivos en tiempo real).*
