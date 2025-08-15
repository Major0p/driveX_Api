using driveX_Api.CommonClasses;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;

namespace driveX_Api.Repository.File
{
    public interface IFileSave
    {
        public Task<ApiResponse<DetailsDto>> CreateFolder(DetailsDto detailsDto);
        public Task<ApiResponse<DetailsDto>> SaveFile(DetailsDto detailsDto);
        public Task<bool> IsValidParentId(string parentId);
        public Task<bool> IsSameNameFile(string name, string parentId);
        public Task<bool> IsTrashedFile(string parentId, string name);
        public Task<bool> IsIdExist(string id);
        public Task<string> CreateUniqueFileId();
        public Task<string> GetParentPath(string id);
    }
}
