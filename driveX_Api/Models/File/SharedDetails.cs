using driveX_Api.Models.User;
using System.ComponentModel.DataAnnotations;

namespace driveX_Api.Models.File
{
    public class SharedDetails
    {
        [Required]
        public string UserId { get; set; }      
        [Required]
        public string DetailsId { get; set; }
        public DateTime SharedDate { get; set; }
        public Details Details { get; set; }
        public UserInfo User { get; set; }
    }
}
