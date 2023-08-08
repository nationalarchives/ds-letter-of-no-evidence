using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleEmail;
using letter_of_no_evidence.web.Helper;
using letter_of_no_evidence.web.Logging;
using letter_of_no_evidence.web.Service;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using System.Net.Http.Headers;

namespace letter_of_no_evidence.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();

            // Get the AWS profile information from configuration providers
            AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
            // Configure AWS service clients to use these credentials
            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddDataProtection().PersistKeysToAWSSystemsManager("/LONE-WEB/DataProtection");
            builder.Services.AddAWSService<IAmazonSimpleEmailService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Add NLoging to the container.
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
            builder.Host.UseNLog();

            var logger = NLogHelper.ConfigureLogger();

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

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "LONE.Session";
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.ConfigureExceptionHandler(logger);
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
    }
}