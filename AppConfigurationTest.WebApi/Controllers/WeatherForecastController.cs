using AppConfigurationTest.WebApi.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace AppConfigurationTest.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IFeatureManager _featureManager;
    private readonly Settings _settings;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsSnapshot<Settings> options, IFeatureManager featureManager)
    {
        _logger = logger;
        _featureManager = featureManager;
        _settings = options.Value;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.OptimizedGetWeather))
        {
            return "Optimized Get Weather response!";
        }
        
        return "Background color: " + _settings.BackgroundColor;
    }
}