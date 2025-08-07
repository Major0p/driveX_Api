using driveX_Api.CommonClasses;
using driveX_Api.DataBase.DBContexts;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;
using driveX_Api.Repository.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Writers;

namespace driveX_Api.Repository.File
{
    public class FileSaveServices : IFileSave
    {
        public DriveXDBC _driveXDBC;
        public IJwtToken _jwtToken;
        public IAuthentication _authentication;

        public FileSaveServices(IJwtToken jwtToken,DriveXDBC driveXDBC, IAuthentication authentication)
        {
            _jwtToken = jwtToken;
            _driveXDBC = driveXDBC;
            _authentication = authentication;
        }

        public async Task<bool> IsValidParentId(string parentId)
        {
            bool response = false;

            if(!string.IsNullOrEmpty(parentId))
            {
                if (parentId == Constants.FILE_ROOT_ID)
                    response = true;

                response = await _driveXDBC.FileDetails.AnyAsync(fl => fl.ParentId == parentId); 
            }

            return response;
        }

        public async Task<bool> IsSameNameFile(string name,string parentId)
        {
            bool response = false;

            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(parentId))
                response = await _driveXDBC.FileDetails.AnyAsync(fl=> fl.ParentId == parentId && fl.Name == name);

            return response;
        }

        public async Task<bool> IsTrashedFile(string parentId, string name)
        {
            bool response = false;

            if (!string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(name))
                response = await _driveXDBC.FileDetails.AnyAsync(fl=> fl.ParentId == parentId && fl.Name == name && fl.Trashed == true);

            return response;
        }

        public async Task<bool> IsIdExist(string id)
        {
            bool response = false;

            if (!string.IsNullOrEmpty(id))
                response = await _driveXDBC.FileDetails.AnyAsync(fl=>fl.Id == id);

            return response;
        }

        public async Task<string>CreateUniqueFileId()
        {
            string id = Utils.GenerateId();
            bool isExist = true;

            do
            {
                isExist = await IsIdExist(id);
            }
            while (isExist);

            return id;
        }

        public async Task<string> GetParentPath(string id)
        {
            string response = string.Empty;

            if (!string.IsNullOrEmpty(id))
                response = await _driveXDBC.FileDetails.Where(fl => fl.Id == id).Select(fl => fl.Path).FirstOrDefaultAsync();

            return response;
        }

        public async Task<string> CreateParentPath(string parentId)
        {
            string parentPath = string.Empty;

            if (parentId != Constants.FILE_ROOT_ID)
            {
                parentPath = await GetParentPath(parentId);
                parentPath += '/' + parentId;
            }
            else
            {
                parentPath = Constants.FILE_ROOT_ID;
            }

            return parentPath;
        }

        public async Task<ApiResponse<Details>> CreateFolder(DetailsDto detailsDto)
        {
            ApiResponse<Details> apiResponse = new();

            bool isUserExist = await _authentication.IsUserExist(detailsDto.UserId);
            bool isParentIdValid = await IsValidParentId(detailsDto.ParentId);
            bool isSameNameFile = await IsSameNameFile(detailsDto.Name,detailsDto.ParentId);
            bool isTrashedFile = await IsTrashedFile(detailsDto.ParentId, detailsDto.Name);

            if (isUserExist && isParentIdValid && isSameNameFile && isTrashedFile)
            {
                string fileId = await CreateUniqueFileId();
                string path = await CreateParentPath(detailsDto.ParentId);

                Details details = new()
                {
                    Id = fileId,
                    UserId = detailsDto.UserId,
                    Name = detailsDto.Name,
                    Size = detailsDto.Size,
                    Extension = detailsDto.Extension,
                    ParentId = detailsDto.ParentId,
                    Path = path,
                    Trashed = isTrashedFile,
                    IsFile = false,
                    Starred = false,
                    Label = detailsDto.Label,
                    CreationDate = Utils.GetCurrDateTime(),
                    ModifiedDate = Utils.GetCurrDateTime(),
                };

                await _driveXDBC.FileDetails.AddAsync(details);

                apiResponse.SetSuccess(details, "folder Created succesfully");

                var token = _jwtToken.GenerateToken(detailsDto.UserId);
                apiResponse.SetToken(token);
            }
            else
            {
                apiResponse.SetFailure("user does not exist");
            }

           return apiResponse;
        }
    }
}
