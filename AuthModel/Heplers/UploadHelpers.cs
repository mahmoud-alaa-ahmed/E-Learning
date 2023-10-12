using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace AuthModel.Heplers
{
    public  static class UploadHelpers
    {
        public static string CourseImagePath { get; private set; } = "courses\\images";
        public static string CourseVideoPath { get; private set; } = "courses\\videos";
        public static string InstructorImagePath { get; private set; } = "instructors\\images";

        public static string UploadFile(IFormFile file, string path)
        {
            
            //create a new name for the image
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var fileName = $"{timeStamp}{Path.GetExtension(file.FileName)}";
            //the image path
            var relativePath = Path.Combine("wwwroot", path, fileName);
            using (var stream = new FileStream(relativePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return relativePath;
        }

    }
}
