using driveX_Api.CommonClasses;
using Newtonsoft.Json;

namespace driveX_Api.Middleware
{
    public class ExecptionHandle
    {
        public RequestDelegate _next;

        public ExecptionHandle(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await ExecptionProcess(httpContext,ex);
            }
        }

        private static Task ExecptionProcess(HttpContext httpContext, Exception exception)
        {
            ApiResponse<object> response = new();

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;

            response.SetFailure(exception.Message);
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

    }
}
