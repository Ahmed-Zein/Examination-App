using System.Text;
using Application.DTOs;
using Application.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Signalr;

public class SignalrClientContext(IConfiguration configuration) : ISignalrClientContext
{
    private readonly string _authEndPoint = configuration["RabbitMQ:SignalR:Auth:EndPoint"]!;
    private readonly string _email = configuration["RabbitMQ:SignalR:Auth:Email"]!;
    private readonly string _password = configuration["RabbitMQ:SignalR:Auth:Password"]!;
    private readonly string _serverUrl = configuration["RabbitMQ:SignalR:ServerUrl"]!;
    private HubConnection? _hubConnection;

    public async Task EstablishConnection()
    {
        _hubConnection ??= new HubConnectionBuilder()
            .WithUrl(_serverUrl, options =>
                options.AccessTokenProvider = () => SignIn(_email, _password)!)
            .WithAutomaticReconnect()
            .Build();

        await _hubConnection.StartAsync();
        Console.WriteLine($"Signalr Connection Established With the Server: {_serverUrl}");
    }


    public async Task InvokeAsync(string methodName, object? arg1, object? arg2 = null)
    {
        await _hubConnection?.InvokeAsync(methodName, arg1)!;
    }

    public bool IsConnected()
    {
        return _hubConnection?.State == HubConnectionState.Connected;
    }

    public async Task TerminateConnection()
    {
        await _hubConnection?.StopAsync()!;
        Console.WriteLine("Signalr Connection Terminated");
    }

    public void Dispose()
    {
        _ = TerminateConnection();
    }

    private async Task<string> SignIn(string email, string password)
    {
        var body = new { Email = email, Password = password };

        using var client = new HttpClient();
        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync(_authEndPoint, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<JsonResponse<AuthenticationResponseDto>>(responseContent);
            Console.WriteLine($"Success: {responseContent}");
            return res?.Data?.Token ?? "FAILED";
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }

        return "";
    }
}