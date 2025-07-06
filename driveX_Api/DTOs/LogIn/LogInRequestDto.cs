using System.ComponentModel.DataAnnotations;

namespace driveX_Api.DTOs.LogIn
{
    public class LogInRequestDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
