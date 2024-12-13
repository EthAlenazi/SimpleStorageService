using Application;
using Core;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/storage")]
    public class StorageController : ControllerBase
    {
        private readonly StorageServiceHandler _storageHandler;

        public StorageController(StorageServiceHandler storageHandler)
        {
            _storageHandler = storageHandler;
        }

        [HttpPost("/v1/blobs")]
        public async Task<IActionResult> UploadFile(ObjectModel model)
        {
            try
            {
                if (!CommonServices.IsValidBase64(model.Data))
                {
                    return BadRequest("Invalid input: Provided DataBase64 string is not in a valid Base64 format.");
                }

                await _storageHandler.HandleUploadAsync(model);

                return Ok($"File uploaded successfully to the storage.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }

        [HttpGet("/v1/blobs/{id:guid}")]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            try
            {
                var id= Id.ToString();
                var file = await _storageHandler.HandleDownloadAsync(id);

                return Ok(file);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        }


}
