namespace ASP_NET_12._TaskFlow_Authentication_and_Authorizaton.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }


    public static ApiResponse<T> SuccessResponse(
        T? data,
        string message = "Operation executed successfully",
        long? executionTimeMs = null)
        => new()
        {
            Success = true,
            Message = message,
            Data = data,
        };

    public static ApiResponse<T> ErrorResponse(
        string message,
        object? errors = null,
        long? executionTimeMs = null)
        => new()
        {
            Success = false,
            Message = message,
            Data = default,
        };
}
