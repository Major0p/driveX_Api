using driveX_Api.CommonClasses;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;

namespace driveX_Api.Repository.File
{
    public interface IFileSave
    {
        public Task<ApiResponse<Details>> CreateFolder(DetailsDto detailsDto);
        public Task<bool> IsValidParentId(string parentId);
    }
}
