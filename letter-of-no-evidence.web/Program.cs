using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleEmail;
using Amazon.SimpleSystemsManagement;
using letter_of_no_evidence.web.Helper;
using letter_of_no_evidence.web.Logging;
using letter_of_no_evidence.web.Service;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using System.Net.Http.Headers;

namespace letter_of_no_evidence.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddHttpContextAccessor();

                // Add NLoging to the container.
                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
                builder.Host.UseNLog();

                var cacheSqlConnection = Environment.GetEnvironmentVariable("CACHE_SQL_CONNECTION");
                if (!string.IsNullOrWhiteSpace(cacheSqlConnection))
                {
                    builder.Services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = cacheSqlConnection;
                        options.SchemaName = "dbo";
                        options.TableName = "CacheData";
                    });
                }

                builder.Services.AddSession(options =>
                {
                    options.Cookie.Name = "LONE.Session";
                    options.Cookie.IsEssential = true;
                });

                // Get the AWS profile information from configuration providers
                AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
                // Configure AWS service clients to use these credentials
                builder.Services.AddDefaultAWSOptions(awsOptions);
                builder.Services.AddDataProtection().PersistKeysToAWSSystemsManager("/LONE-WEB/DataProtection");
                builder.Services.AddAWSService<IAmazonSimpleEmailService>();
                builder.Services.AddScoped<IEmailService, EmailService>();

                builder.Services.AddHttpClient<IPaymentService, PaymentService>(c =>
                {
                    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("GOV_PAY_URL"));
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("GOV_PAY_API_KEY"));
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });

                builder.Services.AddHttpClient<IRequestService, RequestService>(c =>
                {
                    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("LONE_WebApi_URL"));
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });

                builder.Services.AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });

                var app = builder.Build();
                app.ConfigureExceptionHandler(logger);

                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/error");
                }
                app.RegisterTNACookieConsent();
                app.UseSecurityHeaderMiddleware();
                app.UseRouting();

                var rootPath = Environment.GetEnvironmentVariable("LONE_Root_Path");
                if (!string.IsNullOrWhiteSpace(rootPath))
                {
                    app.Use((context, next) =>
                    {
                        context.Request.PathBase = new PathString(rootPath);
                        return next();
                    });
                }
                app.UseStaticFiles();
                app.UseSession();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "",
                        new { controller = "Home", action = "Index" });

                    endpoints.MapControllerRoute(
                        name: "subject-details",
                        pattern: "subject-details",
                        new { controller = "Request", action = "SubjectDetails" });

                    endpoints.MapControllerRoute(
                        name: "contact-details",
                        pattern: "contact-details",
                        new { controller = "Request", action = "ContactDetails" });

                    endpoints.MapControllerRoute(
                        name: "postal-details",
                        pattern: "postal-details",
                        new { controller = "Request", action = "PostalDetails" });

                    endpoints.MapControllerRoute(
                        name: "contact-email",
                        pattern: "contact-email",
                        new { controller = "Request", action = "ContactEmail" });

                    endpoints.MapControllerRoute(
                        name: "request-summary",
                        pattern: "request-summary",
                        new { controller = "Request", action = "RequestSummary" });

                    endpoints.MapControllerRoute(
                        name: "request-receipt",
                        pattern: "request-receipt/{requestNumber}",
                        new { controller = "Request", action = "RequestReceipt" });

                    endpoints.MapControllerRoute(
                        name: "try-again",
                        pattern: "try-again/{requestNumber}",
                        new { controller = "Request", action = "TryAgain" });

                });

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
        }
    }
}