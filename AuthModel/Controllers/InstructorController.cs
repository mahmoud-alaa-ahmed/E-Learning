using AutoMapper;
using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.CourseDtos.InstructorDto;
using ModelLayer.DTO.CourseDtos.InstructorDTOS;
using ModelLayer.Models.Course;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        #region Fields
        private readonly IUnit unitOfWork;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public InstructorController(IUnit unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        #endregion

        #region Get All Instructors
       
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors() => Ok(await unitOfWork.Instructor.GetAllAsync());
        #endregion

        #region Get By Instructor Id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByInstructor(int id)
        {
            var item = await unitOfWork.Instructor.GetByIdAsync(a => a.Id == id);
           return item is not null ? Ok(item) : NotFound(new {message ="The Instructor Not Found"});
        }
        #endregion

        #region add instructor
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddInstructor(InstructorRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(error: ModelState);
            var instructor = await unitOfWork.Instructor.FindAnyAsync(i => i.Email == model.Email);
            if (instructor is not null)
                return BadRequest(new {message = "Email Address Already Exists"});
            if(! unitOfWork.Instructor.CheckEmailPattern(model.Email))
                return BadRequest(error: "Invalid Email Address");
            var newInstructor = new Instructor()
            {
                Email = model.Email,
                Contact = model.Contact,
                ImageUrl =model.Image,
                Name = model.Name,
                Studies = model.Studies,
                About = model.About,
            };
            var item = await unitOfWork.Instructor.AddAsync(newInstructor);
            await unitOfWork.SaveDataAsync();
            return Ok(newInstructor);
        }
        #endregion
        [HttpPut]
        public async Task<IActionResult> UpdateInstructor(InstructorUpdateRequestDTO model)
        {
          var instructor = await unitOfWork.Instructor.GetByIdAsyncAsNoTracking(a => a.Id == model.Id);
            if (instructor is null)
                return NotFound(new { message = "The Instructor Not Found" });
            if (model.Email != instructor.Email)
                return BadRequest(new { message = "The Email Already Taken" });
            unitOfWork.Instructor.Edit(mapper.Map<Instructor>(model));
            await unitOfWork.SaveDataAsync();
            //return Ok(new {message = "success" });
            return Ok(mapper.Map<InstructorResponseDTO>(instructor));
        }

    }
}
