using driveX_Api.CommonClasses;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;
using driveX_Api.Repository.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace driveX_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public IFileSave _fileServices;

        public FileController(IFileSave fileSave)
        {
            _fileServices = fileSave;
        }

        [HttpPost]
        [Route("createFolder")]
        public async Task<IActionResult> CreateFolder(DetailsDto details)
        {
            try
            {
                if (!string.IsNullOrEmpty(details.UserId) && !string.IsNullOrEmpty(details.Name) && !string.IsNullOrEmpty(details.ParentId))
                    return BadRequest("insuffiecint data");

                var response = await _fileServices.CreateFolder(details);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }
        }
    }
}
