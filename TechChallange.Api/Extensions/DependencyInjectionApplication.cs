using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using TechChallangeApi.Data;

namespace TechChallenge.Api.Extensions
{
	public static class DependencyInjectionApplication
	{
		public static IServiceCollection AddDIAuthentication(this IServiceCollection services, WebApplicationBuilder? builder)
		{
			//authentication
			var key = Encoding.ASCII.GetBytes("1f1ce09a-0b07-4fd8-889e-e3e18442b081");

			builder.Services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});


			return services;
		}

		public static IServiceCollection AddDIServices(this IServiceCollection services, WebApplicationBuilder? builder)
		{
			//Add Services
			builder.Services.AddScoped<IFotosRepository, FotosRepository>();
			builder.Services.AddScoped<IFotosRepository, FotosRepository>();
			builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();

			return services;
		}

		public static IServiceCollection AddDICors(this IServiceCollection services, WebApplicationBuilder? builder)
		{
			//cors
			var politica = "CorsPolicy-public";

			builder.Services.AddCors(option => option.AddPolicy(politica, builder => builder.WithOrigins("http://localhost:4200", "https://localhost")
				 .AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials()
							.Build()));

			return services;
		}

		public static IServiceCollection AddDILogs(this IServiceCollection services, WebApplicationBuilder? builder)
		{
			//Log configuration
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json")
						.Build();

			var connectionStringLog = builder.Configuration.GetConnectionString("Log_SQL_CONNECTIONSTRING");

			var option = new ColumnOptions
			{
				AdditionalColumns = new Collection<SqlColumn>
	{
		new SqlColumn {ColumnName = "Action"}
	}
			};

			var sinkOpts = new MSSqlServerSinkOptions();

			Serilog.Log.Logger = new LoggerConfiguration()
						.ReadFrom.Configuration(configuration)
						.CreateLogger();

			Serilog.Debugging.SelfLog.Enable(msg =>
			{
				Debug.Print(msg);
				//Debugger.Break();
			});

			builder.Host.UseSerilog();

			return services;
		}

	}
}
