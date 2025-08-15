using Microsoft.EntityFrameworkCore.Query;

namespace driveX_Api.CommonClasses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "ok";
        public T? Data { get; set; }
        public object Token { get; set; }

        public ApiResponse<T> SetSuccess(T data, string message)
        {
            Success = true;
            Message = message;
            Data = data;
            return this;
        }

        public ApiResponse<T> SetFailure(string message)
        {
            Success = false;
            Message = message;
            Data = default;
            return this;
        }

        public ApiResponse<T> SetToken(object token)
        {
            Token = token;
            return this;
        }
    }
}
