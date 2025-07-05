using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using driveX_Api.Models.User;


namespace driveX_Api.Models.File
{
    public class Details
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        [Required]
        public Guid ParentId { get; set; }
        [Required]
        public string Path { get; set; }
        public bool Trashed { get; set; }
        public bool IsFile { get; set; }
        public bool Starred { get; set; }
        public string Label { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public UserInfo User { get; set; }
        public Storage Storage { get; set; }
        public ICollection<SharedDetails> SharedDetails { get; set; }
    }
}
