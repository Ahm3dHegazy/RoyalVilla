using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace RoyalVilla_API.Common;

public sealed class ApiResponse<T>
{
    public int StatusCode { get; init; }

    public bool IsSuccess { get; init; }

    public string? Message { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Result { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Errors { get; init; }

    private ApiResponse(int statusCode, bool isSuccess, string? message, T? result = default, object? errors = null)
    {
        StatusCode = statusCode;
        IsSuccess = isSuccess;
        Message = message;
        Result = result;
        Errors = errors;
    }
    
    public static ApiResponse<T> Success(
        T? result,
        string message = "Operation completed successfully.",
        int statusCode = StatusCodes.Status200OK)
        => new(statusCode, true, message, result);

    public static ApiResponse<T> Created(
        T? result,
        string message = "Resource created successfully.")
        => new(StatusCodes.Status201Created, true, message, result);
    
    public static ApiResponse<T> BadRequest(string message, object? errors = null)
        => Failure(StatusCodes.Status400BadRequest, message, errors);

    public static ApiResponse<T> NotFound(string message = "Resource not found.")
        => Failure(StatusCodes.Status404NotFound, message);

    public static ApiResponse<T> Conflict(string message)
        => Failure(StatusCodes.Status409Conflict, message);
    
    private static ApiResponse<T> Failure(int statusCode, string message, object? errors = null)
        => new(statusCode, false, message, errors: errors);
}
