using driveX_Api.CommonClasses;
using driveX_Api.DTOs.LogIn;
using driveX_Api.DTOs.SignUp;

namespace driveX_Api.Repository.Auth
{
    public interface IAuthentication
    {
        public Task<bool> IsUserExist(string userId);
        public Task<ApiResponse<LogInResponseDto>> SignUp(SignUpRequestDto signUpRequest);
        public Task<ApiResponse<LogInResponseDto>> SignIn(LogInRequestDto logInRequest);
        public Task<ApiResponse<LogInResponseDto>> RemoveUser(LogInRequestDto logInRequest);

    }
}
