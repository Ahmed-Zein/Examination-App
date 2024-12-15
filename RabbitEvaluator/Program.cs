using Application;
using Core.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitEvaluator;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.SetBasePath("/home/ava/dev/atos/ExaminationSystem/API/");
        var configuration = configurationBuilder.AddJsonFile("appsettings.Development.json").Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApplication();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddInfrastructure(configuration, null);

        var serviceProvider = services.BuildServiceProvider();

        var rabbitConsumer = serviceProvider.GetRequiredService<IRabbitConsumer>();
        _ = await rabbitConsumer.Consume();

        Console.ReadLine();
    }
}