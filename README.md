# 

Plataforma de rentas cortas desarrollada en .NET 10 con Clean Architecture.
Conecta huéspedes con propietarios, automatiza la validación de identidad
por IA y provee herramientas de análisis financiero para propietarios.

## Requisitos previos
- Docker y Docker Compose instalados
- Git

## Levantar el proyecto
1. `git clone <url-del-repositorio>`
2. `cd MiloBnb`
3. `cp .env.example .env`
4. `docker compose up --build`
5. Abrir http://localhost:8080/swagger

Credenciales del Admin por defecto: admin@milobnb.com / Admin1234!

## Arquitectura

Se eligió Clean Architecture en 4 capas porque mantiene el dominio completamente
aislado de frameworks, bases de datos y librerías externas. Esto significa que
la lógica de negocio (reglas de reserva, validación de identidad, cálculo de
métricas) no depende de EF Core, ASP.NET ni ningún proveedor externo —
se puede cambiar PostgreSQL por otro motor, o Anthropic por otro proveedor de IA,
sin tocar una sola línea del dominio.

Las 4 capas y su responsabilidad:
- Domain: entidades, interfaces de repositorio, enums, constantes de negocio.
  No referencia ningún proyecto externo.
- Application: casos de uso como Commands y Queries via CQRS con MediatR.
  Un handler por caso de uso, una sola responsabilidad por clase.
- Infrastructure: implementaciones concretas — EF Core, repositorios,
  servicios externos (KYC, Excel, notificaciones).
- Api: controllers delgados que solo despachan via MediatR, sin lógica de negocio.

CQRS con MediatR se eligió porque separa explícitamente las operaciones de
escritura (Commands) de las de lectura (Queries), hace cada flujo trazable
de forma independiente y permite agregar cross-cutting concerns (validación,
logging) como Pipeline Behaviors sin modificar los handlers.

## Cómo se resolvió cada requerimiento de la prueba

### Requerimientos Generales
