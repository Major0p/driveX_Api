using System.ComponentModel.DataAnnotations;

namespace driveX_Api.Models.LogIn
{
    public class Request
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
