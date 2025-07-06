using System.ComponentModel.DataAnnotations;

namespace driveX_Api.DTOs.SignUp
{
    public class SignUpRequestDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
