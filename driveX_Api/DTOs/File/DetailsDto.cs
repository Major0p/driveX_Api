
using System.ComponentModel.DataAnnotations;

namespace driveX_Api.DTOs.File
{
    public class DetailsDto
    {
        public string Id { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public string Extension { get; set; } = string.Empty;
        [Required]
        public string ParentId { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool Trashed { get; set; } = false;
        public bool IsFile { get; set; } = true;
        public bool Starred { get; set; } = false;
        public string Label { get; set; } = string.Empty;
        public IFormFile Data { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
