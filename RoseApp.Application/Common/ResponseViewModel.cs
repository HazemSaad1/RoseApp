namespace RoseApp.Application.Common
{
    public class ResponseViewModel<T>
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IDictionary<string, string[]>? ValidationErrors { get; set; }
        public string? TraceId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ResponseViewModel<T> Success(
            T data,
            string message = "Operation completed successfully",
            int code = 200)
            => new()
            {
                IsSuccess = true,
                Code = code,
                Message = message,
                Data = data
            };

        public static ResponseViewModel<T> Fail(
            string message,
            int code = 400)
            => new()
            {
                IsSuccess = false,
                Code = code,
                Message = message
            };

        public static ResponseViewModel<T> ValidationFail(
            string message,
            IDictionary<string, string[]> validationErrors,
            string? traceId = null)
            => new()
            {
                IsSuccess = false,
                Code = 422,
                Message = message,
                ValidationErrors = validationErrors,
                TraceId = traceId
            };

        public static ResponseViewModel<T> ServerError(
            string message,
            string? traceId = null)
            => new()
            {
                IsSuccess = false,
                Code = 500,
                Message = message,
                TraceId = traceId
            };
    }
}
