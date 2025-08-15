using AutoMapper;
using Azure;
using driveX_Api.CommonClasses;
using driveX_Api.DataBase.DBContexts;
using driveX_Api.DTOs.LogIn;
using driveX_Api.DTOs.SignUp;
using driveX_Api.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace driveX_Api.Repository.Auth
{
    public class AuthenticationService : IAuthentication
    {
        public DriveXDBC _db;
        public IMapper _mapper;
        public IJwtToken _jwtToken;

        public AuthenticationService(DriveXDBC driveXDBC,IMapper mapper, IJwtToken jwtToken)
        {
            _db = driveXDBC;
            _mapper = mapper;
            _jwtToken = jwtToken;
        }

        public async Task<bool> IsUserExist(string userId)
        {
            var res = await _db.Users.FindAsync(userId);
            if (res == null)
                return false;

            return true;
        }

        public async Task<ApiResponse<LogInResponseDto>> SignUp(SignUpRequestDto signUpRequest)
        {
            ApiResponse<LogInResponseDto> apiResponse = new();

            bool isExist = await IsUserExist(signUpRequest.UserId);
            if (!isExist)
            {
                LogInResponseDto logInResponse = new();

                UserInfo userInfo = _mapper.Map<SignUpRequestDto, UserInfo>(signUpRequest);

                await _db.Users.AddAsync(userInfo);
                await _db.SaveChangesAsync();

                logInResponse.UserId = signUpRequest.UserId;
                logInResponse.Name = signUpRequest.FirstName;

                var token = new
                {
                    AccessToken = _jwtToken.GenerateAccessToken(logInResponse.UserId),
                    SessionToken = _jwtToken.GenerateSessionToken(logInResponse.UserId)
                };
                apiResponse.SetToken(token);
                apiResponse.SetSuccess(logInResponse,"successfull signup");
            }
            else
                apiResponse.SetFailure("User exist. Please log in");

            return apiResponse;
        }

        public async Task<ApiResponse<LogInResponseDto>> SignIn(LogInRequestDto logInRequest)
        {
            ApiResponse<LogInResponseDto> apiResponse = new();

            LogInResponseDto logInInResponse = new();

            var user = await _db.Users
                .AsNoTracking()
                .Where(u => u.UserId.ToString() == logInRequest.UserId)
                .Select(u => new { userId = u.UserId, password = u.Password,firstName = u.FirstName })
                .FirstOrDefaultAsync();
            
            if (user != null && !string.IsNullOrEmpty(user.userId))
            {
                if (logInRequest.UserId == user.userId && logInRequest.Password == user.password)
                {
                    logInInResponse.UserId = logInRequest.UserId;
                    logInInResponse.Name = user.firstName;

                    var token = new
                    {
                        AccessToken = _jwtToken.GenerateAccessToken(logInInResponse.UserId),
                        SessionToken = _jwtToken.GenerateSessionToken(logInInResponse.UserId)
                    };
                    apiResponse.SetToken(token);
                    apiResponse.SetSuccess(logInInResponse,"loged in");
                }
                else
                    apiResponse.SetFailure("incorrect userid and password");
            }
            else
                apiResponse.SetFailure("User does not exist.");

            return apiResponse;
        }

        public async Task<ApiResponse<LogInResponseDto>> RemoveUser(LogInRequestDto logInRequest)
        {
            ApiResponse<LogInResponseDto> apiResponse = new();

            UserInfo userInfo = await _db.Users.FindAsync(logInRequest.UserId);
            if (userInfo != null)
            {
                _db.Users.Remove(userInfo);
                await _db.SaveChangesAsync();

                apiResponse.SetSuccess(null, "removed user");
            }
            else
                apiResponse.SetFailure("user not found");

            return apiResponse;
        }
    }
}
