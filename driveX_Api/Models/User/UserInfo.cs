using driveX_Api.Models.File;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace driveX_Api.Models.User
{
    public class UserInfo
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Details> FileDetails { get; set; }
        public ICollection<SharedDetails> SharedDetails { get; set; }
    }
}
