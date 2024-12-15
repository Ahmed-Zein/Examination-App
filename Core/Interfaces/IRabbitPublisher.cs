namespace Core.Interfaces;

public interface IRabbitPublisher
{
    public Task Publish<T>(T message);
}