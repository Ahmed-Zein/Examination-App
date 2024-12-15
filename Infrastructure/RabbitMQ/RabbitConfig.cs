using Microsoft.Extensions.Configuration;

namespace Infrastructure.RabbitMQ;

public interface IRabbitConfig
{
    string Uri { get; }
    string Exchange { get; }
    string Queue { get; }
    string RoutingKey { get; }
}

public class RabbitConfig(IConfiguration configuration) : IRabbitConfig
{
    public string Uri { get; } = configuration["RabbitMQ:URI"] ??
                                 throw new Exception("RabbitMQ URI not configured");

    public string Exchange { get; } = configuration["RabbitMQ:ExchangeName"] ??
                                      throw new Exception("RabbitMQ exchange name not configured");

    public string Queue { get; } = configuration["RabbitMQ:QueueName"] ??
                                   throw new Exception("RabbitMQ queue name not configured");

    public string RoutingKey { get; } = configuration["RabbitMQ:RoutingKey"] ??
                                        throw new Exception("RabbitMQ routing key not configured");
}