using AutoMapper;
using ModelLayer.DTO.CourseDtos.CategoryDto;
using ModelLayer.DTO.CourseDtos.CourseDto;
using ModelLayer.DTO.CourseDtos.InstructorDto;
using ModelLayer.DTO.CourseDtos.InstructorDTOS;
using ModelLayer.Models.Course;

namespace AuthModel.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            //CreateMap(Instructor, InstructorRequestDTO)();
            CreateMap<InstructorRequestDTO, Instructor>();/*.ForMember(i => i.Image, opt => opt.Ignore());*/
            CreateMap<Instructor,InstructorResponseDTO>();
            CreateMap<InstructorUpdateRequestDTO, Instructor>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<Course, CourseResponseDTO>();
           
        }
    }
}
