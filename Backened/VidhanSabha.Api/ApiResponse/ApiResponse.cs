namespace VidhanSabha.Api.Responses
{
    public class ApiResponse<T>
    {
        // Success fields
        public T? Data { get; set; }
        public object? Meta { get; set; }

        // ProblemDetails fields (RFC 7807)
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? Instance { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }

        // Private constructor — use static factories
        private ApiResponse() { }

        // ── Success ────────────────────────────────────────────
        public static ApiResponse<T> Ok(T data, string? detail = null) =>
            new ApiResponse<T>
            {
                Data = data,
                Detail = detail,
                Status = 200,
                Title = "Success"
            };

        // ── Failure (ProblemDetails style) ─────────────────────
        public static ApiResponse<T> Fail(
            string title,
            string detail,
            int status,
            string? type = null,
            string? instance = null,
            IDictionary<string, string[]>? errors = null) =>
            new ApiResponse<T>
            {
                Title = title,
                Detail = detail,
                Status = status,
                Type = type ?? StatusCodeToType(status),
                Instance = instance,
                Errors = errors
            };

        // ── Shortcuts ──────────────────────────────────────────
        public static ApiResponse<T> NotFound(string detail) =>
            Fail("Not Found", detail, 404);

        public static ApiResponse<T> BadRequest(string detail,
            IDictionary<string, string[]>? errors = null) =>
            Fail("Bad Request", detail, 400, errors: errors);

        public static ApiResponse<T> Unauthorized(string detail = "Unauthorized") =>
            Fail("Unauthorized", detail, 401);

        public static ApiResponse<T> Forbidden(string detail = "Forbidden") =>
            Fail("Forbidden", detail, 403);

        public static ApiResponse<T> ServerError(string detail = "An unexpected error occurred.") =>
            Fail("Internal Server Error", detail, 500);

        public bool IsSuccess => Status is >= 200 and < 300;

        private static string StatusCodeToType(int status) => status switch
        {
            400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
            403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            500 => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            _ => "https://tools.ietf.org/html/rfc7231"
        };
    }
}