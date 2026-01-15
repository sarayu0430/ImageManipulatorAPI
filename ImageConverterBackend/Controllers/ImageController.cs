using Microsoft.AspNetCore.Mvc;
using ImageConverterBackend.Models;
using ImageConverterBackend.Services;

namespace ImageConverterBackend.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : ControllerBase
    {
        private readonly IImageProcessingService _service;

        public ImageController(IImageProcessingService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Process(ImageOperationDto request)
        {

            try
            {
                var id = await _service.ProcessAsync(request);
                var imageMetaData = _service.GetMetadata(id);
                if (imageMetaData == null)
                {
                    return NotFound();
                }
                return Ok(imageMetaData);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var image = _service.GetImage(id);
            if (image == null)
            {
                return NotFound();
            }
            return File(image, "image/png");
        }


        [HttpGet("{id}/metadata")]
        public IActionResult Metadata(string id)
        {
            var metaData = _service.GetMetadata(id);
            if (metaData == null)
            {
                return NotFound();
            }
            return Ok(metaData);

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var metadata = _service.GetMetadata(id);
            if (metadata == null)
            {
                return NotFound();
            }
            _service.Delete(id);
            return Ok();
        }
    }


}
