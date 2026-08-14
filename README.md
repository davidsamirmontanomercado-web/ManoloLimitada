# ManoloLimitada

## Descripción

Aplicación web desarrollada para la gestión de contactos mediante un sistema CRUD con un modelo de visual studio
community con el patron MVC. El sistema permite a un administrador iniciar sesión de forma segura y gestionar la información de los contactos
registrados, incluyendo creación, consulta, edición y eliminación de registros. La aplicación utiliza 
Entity Framework Core para la comunicación con SQL Server y aplica validaciones tanto a nivel de aplicación 
como de base de datos.

## Tecnologías

- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQL Server
- Razor
- HTML5
- CSS3
- Bootstrap
- JavaScript
- Git
- GitHub

## Funcionalidades

- Inicio de sesión de administrador mediante correo y contraseña.
- Cierre de sesión.
- Contraseñas almacenadas utilizando hash.
- Protección de formularios mediante Anti-Forgery Token.
- Visualización de contactos registrados.
- Registro de nuevos contactos.
- Edición de contactos.
- Eliminación de contactos.
- Validación de información.
- Validación de cédulas únicas.
- Cálculo y visualización de edad.
- Mensajes de confirmación y error.
- Modales para crear, editar y eliminar contactos.
- Persistencia de información mediante SQL Server.
- Migraciones mediante Entity Framework Core.
- Favicon personalizado.
- Interfaz adaptable mediante Bootstrap.

## Validaciones

El sistema realiza diferentes validaciones para garantizar la integridad de la información:

- Verifica que los campos obligatorios estén diligenciados.
- Valida la información ingresada en los formularios.
- Evita registrar una cédula duplicada.
- La cédula también está protegida mediante un índice único en SQL Server.
- Valida la fecha de nacimiento.
- Verifica que las operaciones CRUD sean realizadas correctamente.
- Valida las credenciales del administrador.
- Evita almacenar contraseñas en texto plano.

Por otro lado, Las contraseñas de los administradores no se almacenan directamente en la base de datos.
Se utiliza `PasswordHasher<Administrador>` de ASP.NET Core Identity para generar un hash seguro:
Durante el inicio de sesión se utiliza: 
_passwordHasher.VerifyHashedPassword(
    administrador,
    administrador.Password,
    password
);

Los formularios utilizan:[ValidateAntiForgeryToken] para proteger las solicitudes POST contra ataques CSRF y 
Al cerrar sesión se limpia la sesión: HttpContext.Session.Clear();

## Base de datos

La aplicación utiliza SQL Server como sistema de gestión de base de datos. La conexión se realiza mediante 
Entity Framework Core. 
La entidad Contacto contiene información como:

Id
Cédula
Nombre
Apellidos
Fecha de nacimiento
Teléfono
Dirección
Edad
Entidad Administrador

La entidad Administrador contiene:

Id
Correo
Password
Cédula única

Para evitar que dos contactos tengan la misma cédula, se configuró una restricción de unicidad en SQL Server 
mediante Entity Framework Core. La migración genera un índice único:

CREATE UNIQUE INDEX [IX_Contactos_Cedula]
ON [Contactos] ([Cedula]);

De esta manera, la restricción no depende únicamente de la validación del formulario, sino que también está
protegida directamente en la base de datos.

## Entity Framework Core

Entity Framework Core se utiliza como ORM para facilitar la comunicación entre la aplicación y SQL Server.
El contexto principal es:

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options
    ) : base(options)
    {
    }

    public DbSet<Contacto> Contactos { get; set; }

    public DbSet<Administrador> Administradores { get; set; }
}

data/AppDbContext.cs

Las modificaciones de la estructura de la base de datos se administran mediante migraciones de 
Entity Framework Core.Ademas la aplicación incluye un DbSeeder encargado de crear un administrador 
inicial cuando no existen administradores registrados. El proceso verifica primero si existe un administrador:

if (await context.Administradores.AnyAsync())
{
    return;
}

Si no existe, se crea el administrador y su contraseña se almacena utilizando hash.

## Instalación

1: Clonar el repositorio: https://github.com/davidsamirmontanomercado-web/ManoloLimitada.git
2: Restaurar dependencias: dotnet restore
3: Configurar la base de datos: Modificar la cadena de conexión en appsettings.json
Ejemplo:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ManoloDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
4: Aplicar las migraciones desde la Consola del Administrador de paquetes: usando Update-Database O utilizando la CLI:
dotnet run
También puede ejecutarse directamente desde Visual Studio.

## Decisiones técnicas
Se utilizó ASP.NET Core MVC debido a su separación de responsabilidades mediante el patrón Model-View-Controller,
Entity Framework Core para facilitar el acceso a SQL Server y administrar la estructura de la base de datos mediante 
migraciones, Bootstrap para construir una interfaz adaptable y facilitar la creación de formularios, tablas, botones y 
modales.
Para la autenticación se utilizó PasswordHasher<Administrador>, evitando almacenar contraseñas directamente.
La cédula se configuró como única tanto a nivel de aplicación como de base de datos para garantizar la integridad de los 
registros.

## Requisitoss
Para ejecutar el proyecto se requiere:

.NET 8 SDK
Visual Studio 2022 o superior
SQL Server
SQL Server Management Studio (opcional)
Git



