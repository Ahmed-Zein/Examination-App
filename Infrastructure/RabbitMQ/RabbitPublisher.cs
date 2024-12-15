using System.Text;
using System.Text.Json;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.RabbitMQ;

public class RabbitPublisher(IRabbitConfig config) : IRabbitPublisher, IDisposable
{
    private readonly ConnectionFactory _connectionFactory = new()
    {
        Uri = new Uri(config.Uri),
        ClientProvidedName = "Examination App"
    };

    private IConnection? _connection;
    private IChannel? _channel;

    private async Task<IChannel> _CreateChannel()
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync();

        return await _connection!.CreateChannelAsync();
    }

    public async Task Publish<T>(T message)
    {
        _channel ??= await _CreateChannel();

        await _channel.ExchangeDeclareAsync(config.Exchange, ExchangeType.Topic);
        await _channel.QueueDeclareAsync(config.Queue, true, false, false);
        await _channel.QueueBindAsync(config.Queue, config.Exchange, config.RoutingKey);

        var serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        await _channel.BasicPublishAsync(config.Exchange, config.RoutingKey, body);
    }

    private async Task DisposeAsync()
    {
        if (_channel is not null)
            await _channel.CloseAsync();

        if (_connection is not null)
            await _connection.CloseAsync();
    }

    public void Dispose()
    {
        _ = DisposeAsync();
    }
}
// Sending & receiving objects
// Serialize the json 
// Encode it as UTF8 byte[]
// Decode it back 
// Deserialize the json