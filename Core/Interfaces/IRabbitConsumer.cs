using RabbitMQ.Client.Events;

namespace Core.Interfaces;

public interface IRabbitConsumer
{
    Task<AsyncEventingBasicConsumer> Consume();
}