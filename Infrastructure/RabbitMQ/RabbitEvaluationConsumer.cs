using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Application.Models;
using Core.Interfaces;
using Infrastructure.Signalr;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.RabbitMQ;

public class RabbitEvaluationConsumer(
    IRabbitConfig config,
    IEvaluationService examService,
    IClientNotificationInitiator clientNotificationInitiator
) : IRabbitConsumer, IDisposable
{
    private readonly ConnectionFactory _connectionFactory = new()
    {
        Uri = new Uri(config.Uri),
        ClientProvidedName = "Examination App"
    };

    private IChannel? _channel;

    private IConnection? _connection;

    public void Dispose()
    {
        Console.WriteLine("Stopping RabbitEvaluationConsumer");

        _channel?.Dispose();
        _connection?.Dispose();
    }

    public async Task<AsyncEventingBasicConsumer> Consume()
    {
        _channel ??= await _CreateChannel();
        var consumer = await CreateConsumer(_channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var evaluationRequest = JsonSerializer.Deserialize<RabbitExamRequest>(message);
            Console.WriteLine($" [x] Received {evaluationRequest}");
            if (evaluationRequest is null)
                return;

            await examService.EvaluateExam(evaluationRequest.ExamId, evaluationRequest.Solutions);

            try
            {
                await clientNotificationInitiator.SendNotificationToUser(evaluationRequest.StudentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        };

        var consumeTag = await _channel.BasicConsumeAsync(config.Queue, false, consumer);
        Console.WriteLine(consumeTag);
        return consumer;
    }

    private async Task<IChannel> _CreateChannel()
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync();

        return await _connection!.CreateChannelAsync();
    }

    private async Task<AsyncEventingBasicConsumer> CreateConsumer(IChannel channel)
    {
        await channel.ExchangeDeclareAsync(config.Exchange, ExchangeType.Topic);
        await channel.QueueDeclareAsync(config.Queue, true, false, false);
        await channel.QueueBindAsync(config.Queue, config.Exchange, config.RoutingKey);
        await channel.BasicQosAsync(0, 1, false);

        return new AsyncEventingBasicConsumer(channel);
    }
}