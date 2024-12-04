using FluentResults;

namespace API.Models;

public class JsonResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = [];

    private JsonResponse()
    {
    }

    public static JsonResponse<T> Ok(T data, string message = "Success")
    {
        return new JsonResponse<T> { Success = true, Data = data, Message = message };
    }

    public static JsonResponse<T> Error(List<string> errors, string message = "Error")
    {
        return new JsonResponse<T>() { Success = false, Errors = errors, Message = message };
    }

    public static JsonResponse<T> Error(List<IError> errors, string message = "Error")
    {
        return new JsonResponse<T>()
            { Success = false, Errors = errors.Select(error => error.Message).ToList(), Message = message };
    }
}