using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Helpers;
using TrilhaApiDesafio.Repositories;
using TrilhaApiDesafio.Services;
using TrilhaApiDesafio.Validation;
using TrilhaApiDesafio.ViewModels;
using FluentValidation;
using TrilhaApiDesafio.Profiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Microsoft.OpenApi.Models;
using System.IO;
using System;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Formatting.Compact;
using System.Text;
using Microsoft.AspNetCore.Hosting;

// The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
// logger configured in `AddSerilog()` below, once configuration and dependency-injection have both been
// set up successfully.
Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
                .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var routines = new ApplicationRoutines();

    builder.Services.AddSingleton<ApplicationRoutines>(routines);
    builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
    builder.Services.AddScoped<ITarefaService, TarefaService>();
    builder.Services.AddScoped<IValidator<CreateTarefaViewModel>, CreateTarefaValidation>();
    builder.Services.AddScoped<IValidator<TarefaViewModel>, TarefaValidation>();

    builder.Services.AddAutoMapper(settings =>
    {
        settings.AddProfiles([new TarefaProfile()]);
    });

    var connectionBuilder = new SqlConnectionStringBuilder();
    var dataSource = builder.Configuration["DatabaseConnection:DataSource"];
    var isLocalDb = builder.Configuration.GetValue<Boolean>("DatabaseConnection:localDb");
    if (isLocalDb) dataSource = @$"(localdb)\{builder.Configuration["DatabaseConnection:DataSource"]}";
    var initialCatalog = builder.Configuration["DatabaseConnection:InitialCatalog"];
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Databases");
    if (!Directory.Exists(dbPath))
    {
        Directory.CreateDirectory(dbPath);
    }
    var dbFileName = Path.Combine(dbPath, $"{builder.Configuration["DatabaseConnection:DatabaseName"]}.mdf");
    connectionBuilder.DataSource = dataSource;
    connectionBuilder.InitialCatalog = initialCatalog;
    connectionBuilder.IntegratedSecurity = true;
    connectionBuilder.AttachDBFilename = dbFileName;
    var connection = connectionBuilder.ConnectionString;

    // Add services to the container.
    builder.Services.AddDbContext<OrganizadorContext>(options =>
        options.UseSqlServer(connection));

    builder.Services.AddSerilog((services, lc) => lc
       .ReadFrom.Services(services)
       .Enrich.FromLogContext()
       .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
       .WriteTo.File(formatter: new CompactJsonFormatter(),
                     path: "./logs/log-.txt",
                     rollingInterval: RollingInterval.Day,
                     rollOnFileSizeLimit: true,
                     retainedFileCountLimit: 10,
                     encoding: Encoding.UTF8
       )
    );

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.AddServerHeader = false;
        serverOptions.Limits.MaxRequestBodySize = 80_000_000;
    });

    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.Culture = CultureInfo.CurrentCulture;
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        options.UseCamelCasing(true);
    });

    builder.Services.AddCors();
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromHours(1);
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.EnableAnnotations();
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "TrilhaDesafioApi",
            Description = "Solução do Desafio de projeto referente a um sistema de agendamento de tarefas com Entity Framework para o bootcamp XP Inc. - Full Stack Developer",
            //TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Leonardo Buta",
                Url = new Uri("https://www.linkedin.com/in/leonardo-buta")
            },
            License = new OpenApiLicense
            {
                Name = "Termos de uso",
                Url = new Uri("https://app.dio.me/terms/")
            }
        });

        // using System.Reflection; - lentidão no carregamento da página - substituído por annotations
        //var xmlFilename = "desafio.xml";
        //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "documentation", xmlFilename));
    });

    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseCors();
    app.UseHsts();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (ApplicationException ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.Information("Finishing app!");
    Log.CloseAndFlush();
}