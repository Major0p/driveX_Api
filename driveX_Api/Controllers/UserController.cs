using Azure;
using driveX_Api.CommonClasses;
using driveX_Api.DataBase.DBContexts;
using driveX_Api.DTOs.LogIn;
using driveX_Api.DTOs.SignUp;
using driveX_Api.Repository.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace driveX_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IAuthentication _authServices;
        IJwtToken _jwtTokenService;

        public UserController(IAuthentication authentication, IJwtToken jwtTokenService)
        {
            _authServices = authentication;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        [Route("IsLogedIn")]
        [Authorize(AuthenticationSchemes = "SessionScheme")]
        public IActionResult IsLogedIn()
        {
            try
            {
                
                return Ok(JsonConvert.SerializeObject(new {data = 1 }));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }
        }

        [HttpGet]
        [Route("refreshToken")]
        [Authorize(AuthenticationSchemes = "SessionScheme")]
        public IActionResult RefreshToken([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("userId is required");

                var response = _jwtTokenService.GetNewToken(userId);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("signUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto signUpRequest)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest("insufficient data");

                var response = await _authServices.SignUp(signUpRequest);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("logIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] LogInRequestDto logInRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("insufficient data");

                var response = await _authServices.SignIn(logInRequest);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }

        [HttpPost]
        [Route("RemoveUser")]
        [Authorize(AuthenticationSchemes = "AccessScheme")]
        public async Task<IActionResult> RemoveUser([FromBody] LogInRequestDto logInRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("insufficient data");

                var response = await _authServices.RemoveUser(logInRequest);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}

