# Creación del proyecto

Creo los proyectos principales:

* Carpeta "Presentation":
    * Proyecto Template.MvcWebApp

- Carpeta "Application":
    - Proyecto Template.Application
    - Proyecto Template.Domain

- Carpeta "Infrastructure":
    - Proyecto Template.Infrastructure
    - Proyecto Template.Persistence
    - Proyecto Template.Persistence.Identity
    - Proyecto Template.ConnectedServices
    - Proyecto Template.Security

- Carpeta "Cross Cutting":
    - Proyecto Template.Common
    - Proyecto Template.Configuration
    - Proyecto Template.MailSender

Agrego referencias entre proyectos.

# Configuración de proyectos

Creo el proyecto web `Template.MvcWebApp`, utilizo el template:

![Template de proyecto web](/assets/01.TemplateProyectoWeb.png "Template de proyecto web")

Luego selecciono la siguiente configuración

![Configuración del proyecto web](/assets/02.TemplateProyectoWebConf.png "Configuración del proyecto web")

Creo la estructura inicial del proyecto `Template.DataAccess`:

![Estructura de proyecto DataAccess](/assets/03.EstructuraProyectoData.png "Estructura de proyecto Persistence")

La clase `Context` es una clase que hereda de DbContext (Entity Framework).

La clase `PersistenceAssembly` es solo una clase dummy para poder configurar el contexto de EF.

```cs
builder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
```

En el proyecto web `Template.MvcWebApp` modifico el Connection String para crear una base de datos con el nombre que quiera indicarle.

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TemplateAppDb;Trusted_Connection=True;MultipleActiveResultSets=true",
    }
}
```

Luego en el archivo `Program.cs` modifico el contexto para que apunte a la clase `Context` del proyecto `Template.Persistence`. Si había otro contexto creado lo elimino.

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

Si como password utilizo un length distinto al default, debo modificar el modelo para el Register.
Para eso tengo que hacer scaffold de los componentes de Identity (seguir guía: [Scaffold Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-6.0&tabs=visual-studio#scaffold-identity-into-an-mvc-project-with-authorization).

Luego editar `RegisterModel`:

```cs
/// <summary>
///
///
/// </summary>
[Required]
[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
[DataType(DataType.Password)]
[Display(Name = "Password")]
public string Password { get; set; }
```

Desde `Package Manager Console` creo la migración inicial y aplico la migración en base de datos.

## Login con proveedores externos

Para hacer Login con Google o Microsoft hay que seguir las guías:

- [Login con Google](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-6.0)
- [Login con Microsoft](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-6.0)

### Habilitar la generación de códigos QR para 2FA

Para habilitar la generación de códigos QR, seguir esta guía:

- [Generación de códigos QR] (https://learn.microsoft.com/es-mx/aspnet/core/security/authentication/identity-enable-qrcodes?view=aspnetcore-6.0)

## Configurar el envío de mails

Establecer las variables en `appsettings.json`:

```json
"MailSettings": {
    "Host": "127.0.0.1",
    "Port": 25,
    "EnableSSL": false,
    "UserName": "account@sample.com",
    "Password": "Y0urP4ssw0rd!!!",
    "FromEmail": "fromemail@sample.com",
    "DisplayName": "Sample Name"
  },
```