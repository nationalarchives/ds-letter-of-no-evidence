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

            builder.Services.AddDbContext<LONEDBContext>(opt =>
                //opt.UseSqlServer(Environment.GetEnvironmentVariable("LONEConnection"))
                opt.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Database=letter-of-no-evidence;Trusted_Connection=True;")
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