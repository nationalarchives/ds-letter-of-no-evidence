using Amazon.Extensions.NETCore.Setup;
using letter_of_no_evidence.api.Service;
using letter_of_no_evidence.data;
using Microsoft.EntityFrameworkCore;

namespace letter_of_no_evidence.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            app.Run();
        }
    }
}