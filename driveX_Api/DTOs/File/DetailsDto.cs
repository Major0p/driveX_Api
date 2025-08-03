
namespace driveX_Api.DTOs.File
{
    public class DetailsDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string ParentId { get; set; }
        public string Path { get; set; }
        public bool Trashed { get; set; }
        public bool IsFile { get; set; }
        public bool Starred { get; set; }
        public string Label { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
