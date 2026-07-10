# CLAUDE.md — Backend (.NET)

Este proyecto es una API en .NET 10 con **Clean Architecture + DDD + CQRS (MediatR)**.
Este archivo existe para que cualquier caso de uso nuevo siga exactamente el mismo patrón ya establecido — no inventar estructuras nuevas.

## Arquitectura de capas

```
src/
├── Milo.Domain/          → Entidades, enums, interfaces de repos. CERO dependencias externas.
├── Milo.Application/     → Commands, Queries, Handlers, Validators (CQRS con MediatR)
├── Milo.Infraestructure/ → EF Core, implementación de repos, servicios externos
└── Milo.Api/              → Controllers delgados, Program.cs
```

Flujo de dependencia (nunca al revés):
`Controller → MediatR (ISender) → Handler → Repository (interface en Domain, impl en Infraestructure) → DbContext`

## Convenciones de la entidad (Domain)

Ejemplo real: `src/Milo.Domain/Entities/Content.cs`

- `sealed class`, hereda de `BaseEntity` (da `Id`), implementa `IAuditable` e `ISoftDeletable`
- Constructor **privado y vacío**: `private Content() { }`
- Propiedades con `private set` — nunca setters públicos
- Factory method estático `Create(...)` que arma la entidad y genera el `Id`
- Métodos de negocio explícitos en vez de setters (`Activate()`, `Deactivate()`, `Update(...)`)
- Enums de la entidad viven en `Domain/Entities/Enums/`, y se usa `HasConversion<string>()` en la configuración EF para guardarlos como texto legible en la DB

```csharp
public sealed class {Entidad} : BaseEntity, IAuditable, ISoftDeletable
{
    private {Entidad}() { }
    public string Campo { get; private set; } = default!;
    // ... IAuditable: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
    // ... ISoftDeletable: IsDeleted, DeletedAt (el filtro se aplica solo, vía reflection en el DbContext)

    public static {Entidad} Create(...) => new() { Id = Guid.NewGuid(), ... };
}
```

## Convenciones del repositorio (Domain interface + Infraestructure impl)

Interface en `Domain/Repositories/I{Entidad}Repository.cs`:
```csharp
public interface I{Entidad}Repository
{
    Task<{Entidad}?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<{Entidad}>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add({Entidad} entity);
    void Update({Entidad} entity);
    void Delete({Entidad} entity);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

Implementación en `Infraestructure/Persistence/Repositories/{Entidad}Repository.cs`, con **primary constructor**:
```csharp
public sealed class {Entidad}Repository(MiloDbContext dbContext) : I{Entidad}Repository
{
    public async Task<{Entidad}?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.{Entidades}.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    // Add/Update/Delete son síncronos (solo trackean), SaveChangesAsync persiste de verdad
}
```

Registro obligatorio en `Infraestructure/DependencyInjection.cs`:
```csharp
services.AddScoped<I{Entidad}Repository, {Entidad}Repository>();
```

Y el `DbSet<{Entidad}>` en `MiloDbContext.cs`, más su `{Entidad}Configuration.cs` en `Persistence/Configurations/` (`IEntityTypeConfiguration<{Entidad}>`).

## Convenciones de Application (CQRS)

Por cada entidad, estructura de carpetas:
```
Application/{Entidad}s/
├── Commands/
│   ├── Create{Entidad}/   → Command + Validator + Handler
│   ├── Update{Entidad}/   → Command + Validator + Handler
│   └── Delete{Entidad}/   → Command + Handler (sin Validator si solo recibe Id)
└── Queries/
    ├── Get{Entidad}s/     → {Entidad}Dto.cs (el DTO vive AQUÍ) + Query + Handler
    └── Get{Entidad}ById/  → Query + Handler
```

**Regla clave del DTO**: vive en `Queries/Get{Entidad}s/{Entidad}Dto.cs` — tanto Commands como Queries lo importan desde ahí. No crear una carpeta `Common/` aparte para DTOs (así lo hacía el proyecto original).

**Regla clave de errores**: los Handlers **nunca lanzan excepción** para errores de negocio esperados (ej. "no encontrado"). Usan `Result<T>.Failure("mensaje")`. Las excepciones son solo para fallos inesperados del sistema.

```csharp
// Command
public record Create{Entidad}Command(...) : IRequest<Result<{Entidad}Dto>>;

// Validator (FluentValidation)
public sealed class Create{Entidad}CommandValidator : AbstractValidator<Create{Entidad}Command> { ... }

// Handler (primary constructor con las dependencias)
public sealed class Create{Entidad}Handler(
    I{Entidad}Repository repository) : IRequestHandler<Create{Entidad}Command, Result<{Entidad}Dto>>
{
    public async Task<Result<{Entidad}Dto>> Handle(Create{Entidad}Command request, CancellationToken ct)
    {
        var entity = {Entidad}.Create(...);
        repository.Add(entity);
        await repository.SaveChangesAsync(ct);
        return Result<{Entidad}Dto>.Success(new {Entidad}Dto(...));
    }
}
```

**No hay que tocar `Application/DependencyInjection.cs`** para casos de uso nuevos — MediatR y FluentValidation escanean el ensamblado automáticamente vía reflection (`RegisterServicesFromAssembly` / `AddValidatorsFromAssembly`). Solo se toca si se agrega un `IPipelineBehavior` nuevo.

## Convenciones del Controller (Api)

`Api/Controllers/{Entidad}sController.cs`, con `ISender` inyectado (no `IMediator` completo):

```csharp
[ApiController]
[Route("api/{entidad}s")]
public sealed class {Entidad}sController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await sender.Send(new Get{Entidad}sQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : Problem(title: result.Error, statusCode: 400);
    }
    // GetById → 404 en Failure
    // Create  → 400 en Failure
    // Update  → validar que route id == body id, 404 en Failure
    // Delete  → NoContent() en Success, 404 en Failure
}
```

Todos los endpoints están protegidos por defecto (`app.MapControllers().RequireAuthorization()` en `Program.cs`). Usar `[AllowAnonymous]` solo en endpoints públicos explícitos (como `Auth/register`, `Auth/login`).

## Migraciones (EF Core)

Siempre desde `src/Milo.Infraestructure`, apuntando a `Milo.Api` como startup project:

```bash
cd src/Milo.Infraestructure
dotnet ef migrations add {Nombre} --startup-project ../Milo.Api
dotnet ef database update --startup-project ../Milo.Api
```

⚠️ **`MiloDbContextFactory.cs` tiene una connection string HARDCODEADA** (separada de `appsettings.json`) que EF Core usa para estos comandos de diseño. Si se cambia la base de datos/credenciales, hay que actualizar AMBOS lugares:
- `src/Milo.Api/appsettings.json` → `ConnectionStrings:Default`
- `src/Milo.Infraestructure/Persistence/MiloDbContextFactory.cs` → string hardcodeada

Solo se necesita migración nueva si cambia el modelo de datos (entidad nueva, propiedad nueva, relación nueva) — NO por cada Command/Query nuevo sobre una entidad ya existente.

## Comandos frecuentes

```bash
# Build completo
dotnet build

# Levantar Postgres (Docker)
docker compose up -d postgres

# Correr el backend
cd src/Milo.Api && dotnet run

# Ver logs / entrar a Postgres
docker exec -it mipruebatekus-postgres-1 psql -U postgres -d mipruebadb -c "\dt"
```

## Notas del entorno

- Connection string real: `Host=localhost;Database=mipruebadb;Username=postgres;Password=postgres;GSS Encryption Mode=Disable`
  (el `GSS Encryption Mode=Disable` evita el warning inofensivo de Npgsql 10 intentando cargar `libgssapi_krb5` en contenedores Linux)
- Swagger disponible en `/swagger` en modo Development — usar el botón "Authorize" con el JWT del login para probar endpoints protegidos
- Seed automático al arrancar (`DbSeeder.SeedAsync`): crea un usuario Admin con las credenciales de `appsettings.json` → `Admin:Email` / `Admin:Password`
- `AppHost`/`ServiceDefaults` (Aspire) existen en el repo pero **no se usan** — el flujo real es Docker Compose (Postgres) + `dotnet run` manual

## Posible limpieza pendiente (residuos del proyecto base, no bloqueante)

- `Milo.Domain/Common/PagedResult.cs` y `Milo.Application/Common/Models/PagedResult.cs` — duplicado, revisar si se usa
- `Milo.Domain/Constants/BookingConstants.cs` — residuo del dominio original, no aplica a este proyecto