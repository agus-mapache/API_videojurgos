using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using MySql.Data.MySqlClient;
using APIVideoJuegos.Features.Usuarios.Endpoints;
using APIVideoJuegos.Features.Usuarios.Repository;
using APIVideoJuegos.Features.Usuarios.service;
using APIVideoJuegos.Features.Empresa.Repository;
using APIVideoJuegos.Features.Empresa.Service;
using APIVideoJuegos.Features.Empresa.Endpoints;
using APIVideoJuegos.Features.Juegos.Endpoints;
using APIVideoJuegos.Features.Juegos.Repository;
using APIVideoJuegos.Features.Juegos.Service;
using APIVideoJuegos.Features.Generos.Service;
using APIVideoJuegos.Features.Generos.Repository;
using APIVideoJuegos.Features.Generos.Endpoints;
using APIVideoJuegos.Features.Ordenes.Endpoints;
using APIVideoJuegos.Features.Ordenes.Repository;
using APIVideoJuegos.Features.Ordenes.Service;
using APIVideoJuegos.Features.Biblioteca.Endpoints;
using APIVideoJuegos.Features.Biblioteca.Service;
using APIVideoJuegos.Features.Biblioteca.Repository;
using APIVideoJuegos.Features.Resena.Endpoints;
using APIVideoJuegos.Features.Resena.Service;
using APIVideoJuegos.Features.Resena.Repository;

using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

using Dapper;

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddMySql5() 
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
        .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IDbConnection>(_ => new MySqlConnection(connectionString));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();

builder.Services.AddScoped<IJuegoRepository, JuegoRepository>();
builder.Services.AddScoped<IJuegoService, JuegoService>();

builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IGeneroService, GeneroService>();

builder.Services.AddScoped<IOrdenRepository, OrdenRepository>();
builder.Services.AddScoped<IOrdenService, OrdenService>();

builder.Services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();
builder.Services.AddScoped<IBibliotecaService, BibliotecaService>();

builder.Services.AddScoped<IResenaRepository, ResenaRepository>();
builder.Services.AddScoped<IResenaService, ResenaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapUsuarioEndpoints();
app.MapEmpresaEndpoints();
app.MapJuegoEndpoints();
app.MapGeneroEndpoints();
app.MapOrdenEndpoints();
app.MapBibliotecaEndpoints();
app.MapResenaEndpoints();

app.Run();
