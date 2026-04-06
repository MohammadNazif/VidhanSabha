VidhanSabha.Api\Responses\ApiResponse.cs
namespace VidhanSabha.Api.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public object? Meta { get; set; }

        public ApiResponse(T? data, string? message = null, object? meta = null)
        {
            Success = true;
            Data = data;
            Message = message;
            Meta = meta;
        }

        public ApiResponse(string? message)
        {
            Success = false;
            Message = message;
        }
    }
}