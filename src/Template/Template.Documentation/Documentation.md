# Creación del proyecto

Creo los 4 proyectos principales:

- Folder Presentation:
-- Proyecto Template.MvcWebApp

- Folder Application:
-- Proyecto Template.Application
-- Proyecto Template.Domain

-Folder Infrastructure
-- Proyecto Template.DataAccess
-- Proyecto Template.ConnectedServices

- Folder Cross Cutting
-- Proyecto Template.Common (o proyectos separados por utilidades)

Agrego referencias entre proyectos.

# Configuración de proyectos

Creo el proyecto web `Template.MvcWebApp`, utilizo el template:

![Template de proyecto web](/assets/01.TemplateProyectoWeb.png "Template de proyecto web")

Luego selecciono la siguiente configuración

![Configuración del proyecto web](/assets/02.TemplateProyectoWebConf.png "Configuración del proyecto web")

Creo la estructura inicial del proyecto `Template.DataAccess`:

![Estructura de proyecto DataAccess](/assets/03.EstructuraProyectoData.png "Estructura de proyecto DataAccess")

La clase `Context` es una clase que hereda de DbContext (Entity Framework).

La clase `DataAccessAssembly` es solo una clase dummy para poder configurar el contexto de EF.

```cs
builder.ApplyConfigurationsFromAssembly(typeof(DataAccessAssembly).Assembly);
```

En el proyecto web `Template.MvcWebApp` modifico el Connection String para crear una base de datos con el nombre que quiera indicarle.

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TemplateAppDb;Trusted_Connection=True;MultipleActiveResultSets=true",
    }
}
```

Luego en el archivo `Program.cs` modifico el contexto para que apunte a la clase `Context` del proyecto `Template.DataAccess`. Si había otro contexto creado lo elimino.

```cs
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
        .AddDbContext<Context>(options =>
            options.UseSqlServer(connectionString));
```

Agrego dependencias de `Microsoft.AspNetCore.Identity` y `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (NO es Microsoft Identity Platform).

En el archivo `Program.cs` configuro los servicios de Identity.

```cs
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
```

o puedo agregar la configuración en el `appsettings.json` y luego simplificar el archivo `Program.cs`

```json

```

```cs

```

Desde `Package Manager Console` creo la migración inicial y aplico la migración en base de datos.