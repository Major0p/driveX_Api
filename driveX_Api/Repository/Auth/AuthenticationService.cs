using AutoMapper;
using Azure;
using driveX_Api.DataBase.DBContexts;
using driveX_Api.DTOs.LogIn;
using driveX_Api.DTOs.SignUp;
using driveX_Api.Models.User;
using driveX_Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace driveX_Api.Repository.Auth
{
    public class AuthenticationService : IAuthentication
    {
        public DriveXDBC _db;
        public Mapper _mapper;
        public IJwtToken _jwtTokenService;

        public AuthenticationService(DriveXDBC driveXDBC,Mapper mapper, IJwtToken jwtTokenService)
        {
            _db = driveXDBC;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
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
            if (isExist)
            {
                LogInResponseDto logInResponse = new();

                UserInfo userInfo = _mapper.Map<SignUpRequestDto, UserInfo>(signUpRequest);

                await _db.Users.AddAsync(userInfo);
                await _db.SaveChangesAsync();

                logInResponse.UserId = signUpRequest.UserId;
                logInResponse.Name = signUpRequest.FirstName;

                apiResponse.SetSuccess(logInResponse,"succesful signup");
            }
            else
                apiResponse.SetFailure("User does not exist.");

            //add jwt token 
            string token = _jwtTokenService.GenerateToken(signUpRequest.UserId);
            apiResponse.SetToken(token);

            return apiResponse;
        }

        public async Task<ApiResponse<LogInResponseDto>> SignIn(LogInRequestDto logInRequest)
        {
            ApiResponse<LogInResponseDto> apiResponse = new();

            LogInResponseDto logInInResponse = new();

            var user = _db.Users
                .AsNoTracking()
                .Where(u => u.Id.ToString() == logInRequest.UserId)
                .Select(u => new { userId = u.Id.ToString(), password = u.Password,firstName = u.FirstName })
                .FirstOrDefaultAsync();
            
            if (!string.IsNullOrEmpty(user.Result.userId))
            {
                if (logInRequest.UserId == user.Result.userId && logInRequest.Password == user.Result.password)
                {
                    logInInResponse.UserId = logInRequest.UserId;
                    logInInResponse.Name = user.Result.firstName;

                    apiResponse.SetSuccess(logInInResponse,"loged in");
                }
                apiResponse.SetFailure("incorrect userid and password");
            }
            else
                apiResponse.SetFailure("User does not exist.");

            //add jwt token
            string token = _jwtTokenService.GenerateToken(logInRequest.UserId);
            apiResponse.SetToken(token);

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

                apiResponse.SetSuccess(null,"removed user");
            }
            else
                apiResponse.SetFailure("user not found");

            return apiResponse;
        }

    }
}
