# MiPruebaTekus

API en .NET 10 (Clean Architecture + DDD + CQRS) con frontend en Angular,
para gestión de Proveedores y sus Servicios.

## Requisitos previos
- Docker y Docker Compose instalados
- Git

## Levantar todo con Docker Compose

Un solo comando levanta la base de datos (Postgres), el backend (.NET API) y el
frontend (Angular servido con nginx). No necesitas tener instalado .NET ni Node
para probarlo.

1. `git clone <url-del-repositorio>`
2. `cd MiPruebaTekus`
3. `cp .env.example .env`
4. `docker compose up --build`
5. Abrir:
   - Frontend: http://localhost:4200
   - API / Swagger: http://localhost:8080/swagger

> Si ya tienes algo corriendo en el puerto 4200 (por ejemplo un `npm start`
> manual del frontend) o en el 8080, deténlo antes de levantar Compose —
> Docker no puede reutilizar un puerto que ya está en uso por otro proceso.

Para bajar todo: `docker compose down` (agrega `-v` solo si además quieres
borrar los datos de Postgres).

Credenciales del Admin por defecto (usuario `User`, sembrado automáticamente
al arrancar la API): `admin@miprueba.com` / `Admin1234!`

### Servicios y puertos

| Servicio  | Contenedor  | Puerto host | Descripción                          |
|-----------|-------------|-------------|---------------------------------------|
| postgres  | postgres:16 | 5432        | Base de datos                         |
| api       | .NET 10     | 8080        | Backend (Swagger en `/swagger`)       |
| frontend  | nginx       | 4200        | Angular compilado en modo producción  |

### Probar el flujo de Proveedor → Servicios

1. Entra a http://localhost:4200/provider/register y regístrate como
   proveedor (Nombre, NIT, Página web, Email, Contraseña).
2. Te loguea automáticamente y te lleva a "Mis Servicios".
3. Click en "Nuevo" para crear un servicio (Nombre, Costo por hora).
4. Puedes editarlo o eliminarlo desde la lista.
5. Para volver a entrar más tarde: http://localhost:4200/provider/login.

## Desarrollo local (sin Docker para el código de la app)

Si prefieres correr el backend/frontend directamente en tu máquina (con hot
reload) y usar Docker solo para la base de datos:

```bash
docker compose up -d postgres   # solo la base de datos

cd src/Milo.Api
dotnet run                      # backend en http://localhost:5122

cd frontend
npm install
npm start                       # frontend en http://localhost:4200
```

En este modo el frontend apunta a `http://localhost:5122/api`
(`src/environments/environment.ts`), mientras que el build de Docker apunta a
`http://localhost:8080/api` (`environment.prod.ts`), que es donde Compose
publica la API.

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
