using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IFeatureManager _featureManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFeatureManager featureManager)
        {
            _ = logger;
            _featureManager = featureManager;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [FeatureGate("WeatherForecast")] // True --> Enter in code, False --> 404
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            bool testEnabled = await _featureManager.IsEnabledAsync("WeatherTest");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = testEnabled ? Summaries[Random.Shared.Next(Summaries.Length)] : null
                })
                .ToArray();
        }
    }
}