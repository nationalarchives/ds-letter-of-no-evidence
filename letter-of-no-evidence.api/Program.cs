using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleSystemsManagement;
using letter_of_no_evidence.api.Logging;
using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;

namespace letter_of_no_evidence.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Get the AWS profile information from configuration providers
            AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
            // Configure AWS service clients to use these credentials
            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddDataProtection().PersistKeysToAWSSystemsManager("/LONE-API/DataProtection");

            // Add NLoging to the container.
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Warning);
            builder.Host.UseNLog();

            var logger = NLogHelper.ConfigureLogger();

            builder.Services.AddDbContext<LONEDBContext>(opt =>
                     opt.UseSqlServer(Environment.GetEnvironmentVariable("LONEConnection"))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            // Add services to the container.
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Letter of no evidence-api", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Letter of no evidence-api v1"));
            }
            app.ConfigureExceptionHandler(logger);

            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}