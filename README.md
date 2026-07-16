# Municipio Native AOT - Plataforma de Datos y Pagos (PoC Municipio)

Este repositorio contiene la Prueba de Concepto (PoC) para la propuesta de arquitectura del **Municipio**, diseñada para consolidar 4 fuentes legacy con optimización de recursos extrema (.NET 9 Native AOT, DuckDB local, Angular 19 y despliegue nativo en Azure).

---

## 🏛️ Estructura del Monorepo (Proyectada)

```text
/
├── .agents/                 # Configuración de Agentes Especialistas de Gemini/Antigravity
│   ├── agents/              # Perfiles de los Agentes (Analista, Arquitecto, DevOps, QA, Fullstack)
│   └── skills/              # Guías y reglas de arquitectura compartidas (SLA, DuckDB, Backpressure)
├── docs/                    # Documentación técnica y propuesta de arquitectura (PDF, ADRs)
├── src/                     # Código fuente de la aplicación (Backend y Frontend)
└── infra/                   # Infraestructura como Código (Azure Bicep)
```

---

## 🤖 Configuración del Equipo de Agentes

Este monorepo incluye un **equipo de agentes autónomos y especializados** configurados bajo el estándar de Antigravity. Están ubicados en `.agents/` y listos para colaborar contigo:

1.  **Analista de Negocio**: [analista/agent.md](file:///.agents/agents/analista/agent.md) — Definición de requisitos, análisis de datos legacy e ingesta.
2.  **Arquitecto .NET**: [arquitecto_net/agent.md](file:///.agents/agents/arquitecto_net/agent.md) — Estructura técnica, patrones CQRS, DuckDB y orquestación con Aspire.
3.  **Desarrollador Fullstack**: [fullstack_net_angular/agent.md](file:///.agents/agents/fullstack_net_angular/agent.md) — API .NET 9, Channels para Backpressure y Frontend Angular 19.
4.  **Especialista en QA**: [qa/agent.md](file:///.agents/agents/qa/agent.md) — Pruebas unitarias, integración y pruebas de carga/RAM de 4GB.
5.  **DevOps & Infraestructura**: [devops/agent.md](file:///.agents/agents/devops/agent.md) — Azure Bicep, monorepo y pipelines de CI/CD en GitHub Actions.

---

## ⚡ Guías de Desarrollo e Ingesta
Toda la lógica de diseño (SLA, límites de RAM, DuckDB y Service Bus) se encuentra documentada en la skill:
*   📘 **Guía de Arquitectura PoC Municipio**: [municipio_poc_guide/SKILL.md](file:///.agents/skills/municipio_poc_guide/SKILL.md)

---

## 🚀 Cómo empezar
Para activar e interactuar con los agentes especializados en tu IDE, simplemente llámalos por su nombre en el chat (ej. `@Analista`, `@Arquitecto`, `@DevOps`, etc.).
