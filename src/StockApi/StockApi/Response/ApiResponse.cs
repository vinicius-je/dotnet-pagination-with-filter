using System.Text.Json.Serialization;

namespace StockApi.Response
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public ApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ApiResponse(int statusCode, string message, T? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
