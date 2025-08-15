using driveX_Api.CommonClasses;

namespace driveX_Api.Repository.Auth
{
    public interface IJwtToken
    {
        public string GenerateAccessToken(string userId);
        public string GenerateSessionToken(string userId);
        public ApiResponse<object> GetNewToken(string userId);
    }
}
