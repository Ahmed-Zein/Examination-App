namespace Core.Interfaces;

public interface IServerNotificationManager
{
    Task OnExamEvaluation(string userId);
    Task BroadCast(string message);
}