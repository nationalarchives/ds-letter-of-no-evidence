using Amazon.Extensions.NETCore.Setup;
using letter_of_no_evidence.api.Logging;
using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    // Add NLoging to the container.
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Host.UseNLog();

    // Get the AWS profile information from configuration providers
    AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
    // Configure AWS service clients to use these credentials
    builder.Services.AddDefaultAWSOptions(awsOptions);
    builder.Services.AddDataProtection().PersistKeysToAWSSystemsManager("/LONE-API/DataProtection");

    builder.Services.AddDbContext<LONEDBContext>(opt =>
                opt.UseSqlServer(Environment.GetEnvironmentVariable("LONEConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

    // Add services to the container.
    builder.Services.AddScoped<IRequestService, RequestService>();
    builder.Services.AddScoped<IPaymentService, PaymentService>();
    builder.Services.AddScoped<IDeliveryService, DeliveryService>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Letter of no evidence-api", Version = "v1" });
    });
    builder.Services.AddHealthChecks();

    var app = builder.Build();
    app.ConfigureExceptionHandler(logger);

    app.UsePathBase(new PathString("/letter-of-no-evidence-api"));
    app.MapHealthChecks("/letter-of-no-evidence-api/healthz");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.MapControllers();
    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}