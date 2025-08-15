using driveX_Api.CommonClasses;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;
using driveX_Api.Repository.Auth;
using driveX_Api.Repository.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace driveX_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        public IFileSave _fileServices;

        public FileController(IFileSave fileSave)
        {
            _fileServices = fileSave;
        }

        [HttpPost]
        [Route("createfolder")]
        public async Task<IActionResult> CreateFolder([FromBody] DetailsDto details)
        {
            try
            {
                if (string.IsNullOrEmpty(details.UserId) || string.IsNullOrEmpty(details.Name) || string.IsNullOrEmpty(details.ParentId))
                    return BadRequest("insuffiecint data");

                var response = await _fileServices.CreateFolder(details);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }
        }

        [HttpPost]
        [Route("savefile")]
        public async Task<IActionResult> SaveFile([FromForm] DetailsDto detailsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("insuffiecint data");

                var response = await _fileServices.SaveFile(detailsDto);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }
        }

        [HttpGet]
        [Route("setstar")]
        public async Task<IActionResult> SetStarFile([FromQuery] string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId))
                    return BadRequest("fileId missing");

                var response = new { };//await _fileServices.SetStarFile(fileId);
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex);
            }
        }
    }
}
