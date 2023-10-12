using AutoMapper;
using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.CourseDtos.CommentDTOs;
using ModelLayer.DTO.CourseDtos.CourseDto;
using ModelLayer.Models.Course;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.Security.Claims;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Section = ModelLayer.Models.Course.Section;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CourseController : ControllerBase
    {
        #region Fields
        private readonly IUnit unitOfwork;
        private readonly IMapper _mapper;
        private readonly IAuth _auth;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public CourseController(IUnit unitOfwork, IMapper mapper, IAuth auth, IWebHostEnvironment env)
        {
            this.unitOfwork = unitOfwork;
            _mapper = mapper;
            _auth = auth;
            _env = env;
        }
        #endregion

        #region upload course
        
        [HttpPost]
        public async Task<IActionResult> UploadCourse(CourseDto model)
        {

            if (await unitOfwork.Category.GetByIdAsync(a => a.Id == model.CategoryId) is null || await unitOfwork.Instructor.GetByIdAsync(a => a.Id == model.InstructorId) is null)
                return BadRequest(new { message = "The Instructor Or Category Not Exists" });
            var course = new Course()
            {
                Name = model.CourseName,
                Description = model.Description,
                Image = model.Image,
                Language = model.Language,
                Level = model.Level,
                Requirement = model.Requirement,
                InstructorId = model.InstructorId,
                CategoryId = model.CategoryId,
               
            };
            await unitOfwork.Course.AddAsync(course);
            await unitOfwork.SaveDataAsync();
            List<Section> sections = new();

            foreach (var sectionDto in model.Sections)
            {
                var section = new Section()
                {
                    CourseId = course.Id,
                    Name = sectionDto.SectionTitle,
                    OrderInCourse = sectionDto.OrderInCourse
                };
                await unitOfwork.Section.AddAsync(section);
                await unitOfwork.SaveDataAsync();
                foreach (LessonDto lessonDto in sectionDto.Lessons)
                {
                    var lesson = new Lesson()
                    {
                        Name = lessonDto.LessonTitle,
                        OrderInSection = lessonDto.OrderInSection,
                        SectionId = section.Id,
                        VideoUrl = lessonDto.Video,
                    };
                    await unitOfwork.Lesson.AddAsync(lesson);
                    await unitOfwork.SaveDataAsync();
                }
            }

            return Ok(course);
        }
        #endregion
        //if the user subscribe the site 
        //if the user is enroll the course -> return object contain the user progress and the percentage of finshed 
        //if the user not subscribe
        //if the user unauhtorized
        #region Get Course By ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            //check the userid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //check if id is correct
            var course = await unitOfwork.Course.GetByIdAsync(a => a.Id == id,new[] { "CourseComments" });
            if (course is null)
                return NotFound(new { message = "The Course You Try To Get Not Found" });
            //map course comment
            var comments = course.CourseComments.Select(a=> new CommentResponseDTO() { 
            Comment = a.commentTxt ,
            CreatedTime = a.CreatedTime ,
            UserName = a.User.UserName,
            }).ToList();
            var sections = course.Sections.Select(a => new SectionResponseDTO()
            {
                OrderInCourse = a.OrderInCourse,
                SectionTitle = a.Name,
               Lessons = a.Lessons.Select(a=>new LessonResponseDTO() {
                LessonId=a.Id,
               Title =a.Name,
               OrderInSection = a.OrderInSection ,
               VideoUrl =a.VideoUrl
               }).ToList()
            }).ToList();
            var returnedCourse = new CourseResponseDTO()
            {
                CategoryName = course.Category.Name ,
                Id =course.Id ,
                Comments =  comments,
                Sections = sections ,
                Image =course.Image ,
                Description = course.Description ,
                Instructor = course.Instructor,
                Language = course.Language ,
                LastUpdate = course.LastUpdate ,
                Level = course.Level ,
                Name = course.Name,
                ReleaseDate = course.ReleaseDate ,
                Requirement = course.Requirement ,
            };
            //if (string.IsNullOrEmpty(userId))
            //    returnedCourse.Comments = null;
            return Ok(returnedCourse);
        }
        #endregion

        #region Search Course
        
        [HttpGet]
        public async Task<IActionResult> SearchForCourses(string courseName, string sort, int pageSize, int pageNumber)
        {
            if (courseName is null || sort is null || pageSize == 0 || pageNumber == 0)
                return NotFound(new { message = "Cant find what you need!" });
            var results = await unitOfwork.Course.SearchAsync((c => c.Name.Contains(courseName)), (c => c.LastUpdate), sort, pageSize, pageNumber);
            if (results == null)
                return NotFound(new { message = "Course your search for is not found!" });
            var courses = new List<CourseResponseDTO>();
            foreach (var c in results)
            {
                var course = new CourseResponseDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Level = c.Level,
                    ReleaseDate = c.ReleaseDate,
                    Image = c.Image,
                    Description = c.Description,
                    Language = c.Language,
                    Requirement = c.Requirement,
                    LastUpdate = c.LastUpdate,
                    CategoryName = c.Category.Name,
                    InstructorName=c.Instructor.Name
                };
                courses.Add(course);
            }
            return Ok(courses);
        }
        #endregion

        #region Get Course By Category
        
        [HttpGet("all")]
        public async Task<IActionResult> GetCoursesByCategory(int? categoryId, int pageSize, int pageNumber)
        {
            
            //validate subscribtion 
            if (categoryId == 0)
                return BadRequest(error: "Invalid Id");
            if (categoryId != null && categoryId != 0)
            {
                var coursez = await unitOfwork.Course.GetAllAsync((c => c.CategoryId == categoryId), pageSize, pageNumber);
                if (coursez == null)
                    return NotFound(new { message = "no courses found" });
                var newCoursez = new List<CourseCategoryDto>();
                foreach (var course in coursez)
                {
                    var newView = new CourseCategoryDto()
                    {
                        Id = course.Id,
                        Name = course.Name,
                        Description = course.Description,
                        ReleaseDate = course.ReleaseDate,
                        LastUpdate = course.LastUpdate,
                        Image = course.Image,
                        Language = course.Language,
                        Level = course.Level,
                        Requirement = course.Requirement,
                        CategoryName = course.Category.Name,
                        InstructorName = course.Instructor.Name
                    };
                    newCoursez.Add(newView);
                }
                return Ok( newCoursez );
            }
                var courses = await unitOfwork.Course.GetAllAsync(null, pageSize, pageNumber);
            if (courses is null || courses.Count() == 0)
                return NotFound(new { message = "No Result Found" });
            var newCourses = new List<CourseCategoryDto>();
            foreach (var course in courses)
            {
                var newView = new CourseCategoryDto()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    ReleaseDate = course.ReleaseDate,
                    LastUpdate = course.LastUpdate,
                    Image = course.Image,
                    Language = course.Language,
                    Level = course.Level,
                    Requirement = course.Requirement,
                    CategoryName = course.Category.Name,
                    InstructorName=course.Instructor.Name
                };
                newCourses.Add(newView);
            }
            return Ok(newCourses);
        }
        #endregion
     
     
        #region Certification

        [HttpGet("certification")]
        //token
        public async Task<IActionResult> GetStudentCertification(int? courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (studentId is null || courseId is null || courseId == 0)
                return BadRequest(error: "Invalid ids");
            var user = await _auth.FindUserAsync(studentId);
            var course = await unitOfwork.Course.FindAnyAsync(c => c.Id == courseId);
            if (user is null && course is null)
                return BadRequest(error: "Request Failed cant find user or course");
            var webRootPath = _env.WebRootPath;
            var signature = "/images/signature.png";
            var signaturePath = Path.Combine(webRootPath, signature);
            var document = new PdfDocument();
            var htmlContent = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <title>Certificate of Completion</title>\r\n    <style>\r\n      body {        font-family: Arial, sans-serif;\r\n        font-size: 16px;\r\n      }\r\n      .certificate {\r\n        border: 2px solid #000;\r\n        padding: 20px;\r\n        text-align: center;\r\n      }\r\n      .title {\r\n        font-size: 24px;\r\n        font-weight: bold;\r\n        margin-bottom: 30px;\r\n      }\r\n      .name {\r\n        font-size: 20px;\r\n        font-weight: bold;\r\n        margin-bottom: 10px;\r\n      }\r\n      .date {\r\n        font-size: 16px;\r\n        font-weight: bold;\r\n        margin-bottom: 20px;\r\n      }\r\n      .course {\r\n        font-size: 18px;\r\n        font-weight: bold;\r\n        margin-bottom: 20px;\r\n      }\r\n      .signature {\r\n        margin-top: 50px;\r\n      }\r\n      .signature img {\r\n        width: 150px;\r\n      }\r\n    </style>\r\n  </head>\r\n  <body>\r\n    <div class=\"certificate\">\r\n      <div class=\"title\" style='padding:4rem;'>Certificate of Completion</div>\r\n ";
            htmlContent += $"<div class='name' style='color:brown;'>{user.UserName}</div>     <div style='padding:4rem;' class='date'>{DateTime.Now}</div>      <div style='padding:4rem;' class='course'>{course.Name}</div>      <div class='signature'>    <div style='padding:4rem;'>Instructor:{course.Instructor.Name}</div>   </div>  <img style='padding:4rem;'     </div>  </body></html>";
            PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A5);
            byte[]? result = null;
            using MemoryStream stream = new MemoryStream();
            document.Save(stream);
            result = stream.ToArray();
            string fileName = "studentName " + "Certificate" + ".pdf";
            return File(result, "application/pdf0", fileName);
        }
        #endregion
       

    }
}
