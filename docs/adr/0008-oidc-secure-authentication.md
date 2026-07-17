# ADR-0008: Integración de Seguridad y Gestión de Secretos mediante OIDC y Managed Identities

## Estado
Aceptado

## Contexto
El desarrollo de la PoC se aloja en un repositorio público de GitHub (`https://github.com/arielzamora/municipio-native-aot`). Guardar secretos estáticos de larga duración (como contraseñas de bases de datos, claves de Service Bus o contraseñas de Service Principals de Azure) en el código fuente o en los secretos de GitHub introduce un riesgo crítico de filtración y mantenimiento (rotación de claves).

## Decisión
Se decide implementar un modelo de seguridad "Zero Secrets" (cero secretos compartidos) utilizando las siguientes tecnologías:

1.  **OpenID Connect (OIDC)**: GitHub Actions se autenticará con Azure mediante credenciales federadas de confianza. Azure confiará en la identidad emitida por GitHub para la rama `main` de este repositorio específico, emitiendo un token de acceso temporal de corto ciclo de vida.
2.  **Identidades Administradas de Azure (Managed Identities)**: Se configurarán identidades administradas asignadas por el sistema (*System-Assigned Managed Identity*) para los recursos de Azure Container Apps. 
3.  **Acceso basado en Roles (RBAC)**: En lugar de usar cadenas de conexión con contraseña para acceder a Azure SQL o Azure Service Bus, los contenedores se autenticarán usando su identidad administrada, y se les concederán los roles específicos necesarios (ej. `Azure Service Bus Data Receiver` y `Azure Service Bus Data Sender`).

## Justificación
La autenticación federada OIDC elimina la necesidad de crear y mantener contraseñas (*Client Secrets*) en GitHub para el despliegue automático. Si el repositorio es público, no existe riesgo de robo de credenciales estáticas de Azure. 

El uso de **Managed Identities** entre Container Apps, SQL y Service Bus garantiza que no existan cadenas de conexión con usuario y contraseña almacenadas en los archivos de configuración (`appsettings.json`) de la API o del Ingestor, delegando toda la autenticación al plano de control seguro de Azure.

## Consecuencias
*   **Positivas**:
    *   **Seguridad Extrema**: Cero contraseñas guardadas en código o secretos de GitHub.
    *   **Mantenimiento Cero**: No requiere rotación manual de contraseñas de Service Principals ni de bases de datos.
    *   **Cumplimiento Normativo**: Se alinea con las mejores prácticas de gobernanza y seguridad en la nube de Microsoft.
*   **Negativas**:
    *   **Complejidad Inicial de Configuración**: Requiere configurar la relación de confianza federada de forma manual en Azure Portal o mediante Azure CLI en la primera instalación.
