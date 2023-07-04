using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using TechChallangeApi.Controllers;
using TechChallangeApi.Data;
using TechChallenge.Api.Extensions;
using TechChallenge.Api.Factory;
using TechChallenge.Api.Models;
using TechChallenge.Api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDILogs(builder);

//Configura��es DB
var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
builder.Services.AddDbContext<ApplicationContext>(opts => opts.UseSqlServer(connectionString));

builder.Services.AddDIServices(builder);

//Add Facotry
builder.Services.AddScoped<ILogFactory, LogFactory>();


// ?????
// � a melhor forma de configurar o servi�o de HTTPRTEQUEST???
builder.Services.AddHttpClient<FotoController>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDICors(builder);

builder.Services.AddDIAuthentication(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//cors
app.UseCors("CorsPolicy-public");
app.UseAuthorization();

app.MapControllers();

app.Run();
