using AutoMapper;
using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.CourseDtos.CategoryDto;
using ModelLayer.Models.Course;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Fields 
        private readonly IMapper mapper;
        private readonly IUnit unitOfWork;
        #endregion

        #region Constructor
        public CategoryController(IMapper mapper, IUnit unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Get All Categories
        
        [HttpGet]
        public async Task<IActionResult> GetCatgories() =>
         Ok(mapper.Map<IEnumerable<CategoryResponseDTO>>(await unitOfWork.Category.GetAllAsync()));

        #endregion

        #region Get Category By ID
        [HttpGet("{Id}")]
        public  async Task<IActionResult> GetById(int Id)
        {
            
            var item = await unitOfWork.Category.GetByIdAsync(a => a.Id == Id , new[] { "category" });
            

            return item is null ?NotFound(new {message ="the Category You Search For Not Found"}) : Ok(new CategoryResponseDTO()
            {
                Id = item.Id ,
                Name = item.Name ,
                SubId = item?.category?.Id
            });
        }

        #endregion

        #region Update Category
        [Authorize(Roles ="Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit (CategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await unitOfWork.Category.GetByIdAsync(a => a.Id == model.Id) is null)
                return BadRequest(new { message = "The Category You Try To Edit Not Exist" });
            if (model.SubId != 0)
                if (await unitOfWork.Category.GetByIdAsync(a => a.Id == model.SubId) is null)
                    return BadRequest(new { message = "The Super Category Not Exist" });
            if(model.SubId ==0)
                model.SubId = null;
            if (await unitOfWork.Category.FindAnyAsync(a => a.Name == model.Name) is not null)
                return BadRequest(new { message = "The Category Already Exist" });
          var editedItem =unitOfWork.Category.Edit(mapper.Map<Category>(model));
           await unitOfWork.SaveDataAsync();
            return Ok(mapper.Map<CategoryResponseDTO>(editedItem));
        }
        #endregion

        #region Delete Catgeory
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await unitOfWork.Category.GetByIdAsync(a => a.Id == id);
            if (category is  null) 
                return NotFound(new {message ="The Category You Try To Delete Not Found"});
            unitOfWork.Category.Delete(category);
            await  unitOfWork.SaveDataAsync();
            return Ok(mapper.Map<CategoryResponseDTO>(category));
        }
        #endregion
        //null case
        #region Add Catgory
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCatgory(CategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (model.SubId != 0)
                if (await unitOfWork.Category.GetByIdAsync(a => a.Id == model.SubId) is null)
                    return BadRequest(new { message = "The Super Category Not Exist" });
            model.SubId = null;
            if (await unitOfWork.Category.FindAnyAsync(a => a.Name == model.Name) is not null)
                return BadRequest(new { message = "The Category Already Exist" });
            var category = mapper.Map<Category>(model);
            await unitOfWork.Category.AddAsync(category);
            await unitOfWork.SaveDataAsync();
            return Ok(category);
        } 
        #endregion
    }
}
