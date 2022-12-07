using Microsoft.FeatureManagement;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFeatureManagement();

string connectionString = builder.Configuration.GetConnectionString("AppConfig");
builder.Configuration.AddAzureAppConfiguration(options =>
    options.Connect(connectionString)
        .UseFeatureFlags(
            featureFlagOptions =>
            {
                featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(5);
                featureFlagOptions.Select("FeatureManagement:*");
            })
        .ConfigureRefresh(
            refreshOptions =>
            {
                refreshOptions.Register("FeatureManagement:WeatherTest", refreshAll:true)
                    .SetCacheExpiration(new System.TimeSpan(0, 0, 0, 5, 0));
            }));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
