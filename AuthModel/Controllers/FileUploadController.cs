using Microsoft.AspNetCore.Mvc;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [HttpPost("video")]
        //[RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)]
        public IActionResult UploadVideo(IFormFile file)
        {
            #region checkExtension
            var extension = new string[] { ".mp4" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Contains(fileExtension))
                return BadRequest(new { message = "only .mp4 allowed" });
            #endregion
            #region storing
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
            var filePath = Path.Combine(videoPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
            #endregion
            #region
            var fileUrl = $"{Request.Scheme}://{Request.Host}/video/{fileName}";
            #endregion
            return Ok(new { message = "successfully uploaded", url = fileUrl });
        }
        [HttpPost("image")]
        //[RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)]
        public IActionResult UploadImage(IFormFile file)
        {
            #region checkExtension
            var extension = new string[] { ".png",".jpg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Contains(fileExtension))
                return BadRequest(new { message = "only .mp4 allowed" });
            #endregion
            #region storing
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            var filePath = Path.Combine(videoPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
            #endregion
            #region
            var fileUrl = $"{Request.Scheme}://{Request.Host}/image/{fileName}";
            #endregion
            return Ok(new {message = "successfully uploaded",url= fileUrl });
        }
    }
}
