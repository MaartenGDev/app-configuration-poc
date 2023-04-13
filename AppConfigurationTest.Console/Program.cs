using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AppConfigurationTest.Console;

internal static class Program
{
    private static IConfiguration _configuration = null;
    private static IConfigurationRefresher _refresher = null;

    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.AddAzureAppConfiguration(options =>
        {
            options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                .ConfigureRefresh(refresh =>
                {
                    refresh.Register("TestApp:Settings:Message")
                        .SetCacheExpiration(TimeSpan.FromSeconds(10));
                })
                .UseFeatureFlags();

            _refresher = options.GetRefresher();
        });
        

        _configuration = builder.Build();
        PrintMessage().Wait();
    }

    private static async Task PrintMessage()
    {
        System.Console.WriteLine(_configuration["TestApp:Settings:Message"] ?? "Hello world!");

        // Wait for the user to press Enter
        System.Console.ReadLine();

        await _refresher.TryRefreshAsync();
        System.Console.WriteLine(_configuration["TestApp:Settings:Message"] ?? "Hello world!");
    }
}