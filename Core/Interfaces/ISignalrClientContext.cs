namespace Core.Interfaces;

public interface ISignalrClientContext : IDisposable
{
    Task EstablishConnection();
    Task TerminateConnection();
    bool IsConnected();
    Task InvokeAsync(string methodName, object? arg1, object? arg2 = null);
}