using letter_of_no_evidence.web.Helper;
using Microsoft.AspNetCore.Mvc;

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

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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

            app.RegisterTNACookieConsent();
            //app.UseSecurityHeaderMiddleware();
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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "",
                    new { controller = "Home", action = "Index" });
            });

            app.Run();
        }
    }
}