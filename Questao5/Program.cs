using FluentValidation.AspNetCore;
using FluentValidation;
using MediatR;
using Questao5.Application.Services;
using Questao5.Application.Services.Interfaces;
using Questao5.Application.Validators;
using Questao5.Infrastructure.Repositories;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateMovimentoCommandValidator>();

// Services
builder.Services.AddScoped<IContaCorrenteService, ContaCorrenteService>();

// Repos
builder.Services.AddScoped<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddScoped<IContaCorrenteQueryRepository, ContaCorrenteQueryRepository>();
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


