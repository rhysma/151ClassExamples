
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

namespace MegaTravelAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

#if (DEBUG)
            var databaseConnectionString = builder.Configuration["MegaTravel:DatabaseConnectionString"];
#elif (RELEASE)
         SecretClientOptions options = new SecretClientOptions()
        {
            Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
             }
        };
            var client = new SecretClient(new Uri("https://megatravelsecretssb.vault.azure.net/"), new DefaultAzureCredential(),options);

            KeyVaultSecret secret = client.GetSecret("DatabaseConnectionString");

            var databaseConnectionString = secret.Value;
#endif
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MegaTravel Services",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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
