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
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

        builder.Services.AddLogging();
        builder.Services.AddApplication();
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

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

        var tcs = new TaskCompletionSource();
        await tcs.Task;
    }
}