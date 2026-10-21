using System.Text;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Middlewares;
using Web.OpenApi;

// Carga las variables del archivo .env (secretos que NO se suben a git) como variables de entorno.
// Tiene que ir antes de CreateBuilder para que la configuración las lea.
// En el .env, "Jwt__SecretKey" equivale a la clave "Jwt:SecretKey" de appsettings.json.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // Botón "Authorize" en Swagger para probar endpoints con token.
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

#region CORS
// Permite que el frontend (React, en otro puerto/dominio) llame a la API.
// Sin esto, el navegador bloquea las requests. La URL se configura en "App:FrontendUrl".
const string FrontendCorsPolicy = "AllowFrontend";
var frontendUrl = builder.Configuration["App:FrontendUrl"] ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        policy.WithOrigins(frontendUrl)
            .AllowAnyHeader()   // incluye Authorization (el token)
            .AllowAnyMethod();
    });
});
#endregion

#region Database
// La cadena de conexión se lee de appsettings.json (ConnectionStrings:DefaultConnection).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new Exception("La cadena de conexión 'DefaultConnection' no está configurada en appsettings.json");

var connection = new SqliteConnection(connectionString);
connection.Open();

// Set journal mode to DELETE using PRAGMA statement
using (var command = connection.CreateCommand())
{
    command.CommandText = "PRAGMA journal_mode = DELETE;";
    command.ExecuteNonQuery();
}

builder.Services.AddDbContext<ApplicationContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));
#endregion

#region Authentication (JWT)
// Configuración del token, sección "Jwt" de appsettings.json.
var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
if (jwtOptions is null || jwtOptions.SecretKey.Length < 32)
    throw new Exception("Falta configurar el JWT: revisá la sección 'Jwt' de appsettings.json y Jwt__SecretKey en el archivo .env (mínimo 32 caracteres)");

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

// Cómo se valida el token que llega en el header Authorization: Bearer <token>.
// La fecha de vencimiento se valida por defecto.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Deja los claims con el nombre con que se escribieron ("sub", "role"...),
        // sin traducirlos a los nombres largos de Microsoft.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            // Así [Authorize(Roles = "...")] sabe en qué claim buscar el rol.
            RoleClaimType = AuthClaimTypes.Role,
            NameClaimType = AuthClaimTypes.Email
        };
    });

builder.Services.AddAuthorization();
#endregion

#region Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepositoryEF>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
#endregion

#region Services
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
#endregion

#region Seed
// Crea el primer SuperAdmin si no existe (sección "SuperAdminSeed": datos en appsettings.Development.json,
// mail y contraseña en el archivo .env).
builder.Services.Configure<SuperAdminSeedOptions>(builder.Configuration.GetSection(SuperAdminSeedOptions.SectionName));
builder.Services.AddScoped<SuperAdminSeeder>();
#endregion

// Middleware global de manejo de excepciones (NotFound -> 404, Validation -> 400,
// credenciales inválidas -> 401, sin permiso -> 403, resto -> 500).
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<SuperAdminSeeder>().Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
    });
}

// Va primero para capturar las excepciones de todo lo que viene después.
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// CORS va antes de la autenticación, para que el navegador pueda hacer la consulta previa (preflight).
app.UseCors(FrontendCorsPolicy);

// Primero se averigua QUIÉN es (lee y valida el token)...
app.UseAuthentication();
// ...y después si PUEDE entrar al endpoint ([Authorize]).
app.UseAuthorization();

app.MapControllers();

app.Run();
