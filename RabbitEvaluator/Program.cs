using Application;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Signalr;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitEvaluator;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath("/home/ava/dev/atos/ExaminationSystem/API/")
            .AddJsonFile("appsettings.Development.json");
        var configuration = configurationBuilder.Build();

        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.Bind(configuration);

        builder.Services.AddLogging();
        builder.Services.AddApplication();
        builder.Services.AddSingleton<IConfiguration>(configuration);
        builder.Services.AddInfrastructure(configuration, builder.Environment);

        var serviceProvider = builder.Services.BuildServiceProvider();

        var rabbitConsumer = serviceProvider.GetRequiredService<IRabbitConsumer>();
        var signalrManager = serviceProvider.GetRequiredService<ISignalrClientContext>();
        try
        {
            await signalrManager.EstablishConnection();
            var notificationInitiator = serviceProvider.GetRequiredService<IClientNotificationInitiator>();
            await notificationInitiator.SendBroadCastNotification("The rabbit says Hi");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _ = await rabbitConsumer.Consume();
        }

        Console.ReadLine();
    }
}