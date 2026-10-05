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

using Dapper;

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapUsuarioEndpoints();
app.MapEmpresaEndpoints();
app.MapJuegoEndpoints();
app.MapGeneroEndpoints();
app.MapOrdenEndpoints();
app.MapBibliotecaEndpoints();
app.MapResenaEndpoints();

app.Run();
