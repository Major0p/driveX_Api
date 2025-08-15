using AutoMapper;
using driveX_Api.CommonClasses;
using driveX_Api.DataBase.DBContexts;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;
using driveX_Api.Repository.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Writers;

namespace driveX_Api.Repository.File
{
    public class FileSaveServices : IFileSave
    {
        public DriveXDBC _driveXDBC;
        public IAuthentication _authentication;
        public IMapper _mapper;

        public FileSaveServices(DriveXDBC driveXDBC, IAuthentication authentication,IMapper mapper)
        {
            _driveXDBC = driveXDBC;
            _authentication = authentication;
            _mapper = mapper;
        }

        public async Task<bool> IsValidParentId(string parentId)
        {
            if(!string.IsNullOrEmpty(parentId))
            {
                if (parentId == Constants.FILE_ROOT_ID)
                    return true;

                 return await _driveXDBC.FileDetails.AnyAsync(fl => fl.ParentId == parentId); 
            }

            return false;
        }

        public async Task<bool> IsSameNameFile(string name,string parentId)
        {
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(parentId))
                return await _driveXDBC.FileDetails.AnyAsync(fl => fl.ParentId == parentId && fl.Name == name);

            return false;
        }

        public async Task<bool> IsTrashedFile(string parentId, string name)
        {
            if (!string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(name))
                return await _driveXDBC.FileDetails.AnyAsync(fl => fl.ParentId == parentId && fl.Name == name && fl.Trashed == true);

            return false;
        }

        public async Task<bool> IsIdExist(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return await _driveXDBC.FileDetails.AnyAsync(fl=>fl.Id == id);

            return false;
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
            if (!string.IsNullOrEmpty(id))
                return await _driveXDBC.FileDetails.Where(fl => fl.Id == id).Select(fl => fl.Path).FirstOrDefaultAsync();

            return string.Empty;
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

        public async Task<ApiResponse<DetailsDto>> CreateFolder(DetailsDto detailsDto)
        {
            ApiResponse<DetailsDto> apiResponse = new();

            bool isUserExist = await _authentication.IsUserExist(detailsDto.UserId);
            bool isParentIdValid = await IsValidParentId(detailsDto.ParentId);
            bool isSameNameFile = await IsSameNameFile(detailsDto.Name,detailsDto.ParentId);
            bool isTrashedFile = await IsTrashedFile(detailsDto.ParentId, detailsDto.Name);

            if (isUserExist && isParentIdValid && !isSameNameFile && !isTrashedFile)
            {
                string fileId = await CreateUniqueFileId();
                string path = await CreateParentPath(detailsDto.ParentId);

                Details details = new()
                {
                    Id = fileId,
                    UserId = detailsDto.UserId,
                    Name = detailsDto.Name,
                    Size = detailsDto.Size,
                    Extension = string.Empty,
                    ParentId = detailsDto.ParentId,
                    Path = path,
                    Trashed = false,
                    IsFile = false,
                    Starred = false,
                    Label = detailsDto.Label,
                    CreationDate = Utils.GetCurrDateTime(),
                    ModifiedDate = Utils.GetCurrDateTime(),
                };

                await _driveXDBC.FileDetails.AddAsync(details);
                await _driveXDBC.SaveChangesAsync();

                DetailsDto responseData = new();
                responseData = _mapper.Map(details,responseData);

                apiResponse.SetSuccess(responseData, "folder Created succesfully");
            }
            else
            {
                string msg = string.Empty;

                if (!isUserExist)
                    msg = "user not exist";

                if (!isParentIdValid)
                    msg = "parent id is invalid";

                if (isSameNameFile)
                    msg = "same folder exist";

                if (isTrashedFile)
                    msg = "file is in trash";

                apiResponse.SetFailure(msg);
            }

           return apiResponse;
        }

        public async Task<ApiResponse<DetailsDto>> SaveFile(DetailsDto detailsDto)
        {
            ApiResponse<DetailsDto> apiResponse = new();

            bool isUserExist = await _authentication.IsUserExist(detailsDto.UserId);
            bool isParentIdValid = await IsValidParentId(detailsDto.ParentId);
            bool isSameNameFile = await IsSameNameFile(detailsDto.Name, detailsDto.ParentId);
            bool isTrashedFile = await IsTrashedFile(detailsDto.ParentId, detailsDto.Name);

            if (isUserExist && isParentIdValid && !isSameNameFile && !isTrashedFile)
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
                    Trashed = false,
                    IsFile = true,
                    Starred = false,
                    Label = detailsDto.Label,
                    CreationDate = Utils.GetCurrDateTime(),
                    ModifiedDate = Utils.GetCurrDateTime(),
                };

                byte[] fileData = []; 
                if(detailsDto.Data != null)
                {
                    using var ms = new MemoryStream();
                    await detailsDto.Data.CopyToAsync(ms);
                    fileData = ms.ToArray();
                }
                Storage storage = new()
                { 
                    Id = fileId,
                    Data = fileData
                }
            ;
                await _driveXDBC.FileDetails.AddAsync(details);
                await _driveXDBC.SaveChangesAsync();

                await _driveXDBC.FileStorage.AddAsync(storage);
                await _driveXDBC.SaveChangesAsync();

                DetailsDto responseData = new();
                responseData = _mapper.Map(details, responseData);
                responseData.Data = null;

                apiResponse.SetSuccess(responseData, "file saved succesfully");
            }
            else
            {
                string msg = string.Empty;

                if (!isUserExist)
                    msg = "user not exist";

                if (!isParentIdValid)
                    msg = "parent id is invalid";

                if (isSameNameFile)
                    msg = "same folder exist";

                if (isTrashedFile)
                    msg = "file is in trash";

                apiResponse.SetFailure(msg);
            }

           return apiResponse;
        }

    }
}
