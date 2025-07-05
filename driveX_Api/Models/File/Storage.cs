using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace driveX_Api.Models.File
{
    public class Storage
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public byte[] Data { get; set; }
        public Details Details { get; set; }
    }
}
