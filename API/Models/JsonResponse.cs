using FluentResults;

namespace API.Models;

public class JsonResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public List<string> Errors { get; init; } = [];

    private JsonResponse()
    {
    }

    // Success response
    public static JsonResponse<T> Ok(T data, string message = "Success") =>
        new()
        {
            Success = true,
            Data = data,
            Message = message
        };

    // Error response
    public static JsonResponse<T> Error(IEnumerable<string> errors, string message = "Error") =>
        new()
        {
            Success = false,
            Errors = errors.ToList(),
            Message = message
        };

    public static JsonResponse<T> Error(IEnumerable<IError> errors, string message = "Error") =>
        Error(errors.Select(error => error.Message), message);
}