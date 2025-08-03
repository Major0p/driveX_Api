using driveX_Api.CommonClasses;
using driveX_Api.DTOs.File;
using driveX_Api.Models.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace driveX_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [HttpPost]
        [Route("createFolder")]
        public async Task<IActionResult> CreateFolder(DetailsDto details)
        {
            try
            {
                if (!string.IsNullOrEmpty(details.UserId) && !string.IsNullOrEmpty(details.Name) && !string.IsNullOrEmpty(details.ParentId))
                return BadRequest("insuffiecint data");


            }
            catch(Exception ex)
            {
                StatusCode(500,ex.ToString());
            }
        }
    }
}
