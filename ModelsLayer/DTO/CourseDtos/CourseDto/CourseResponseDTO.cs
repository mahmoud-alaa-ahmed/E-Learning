using ModelLayer.Models.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO.CourseDtos.CommentDTOs;

namespace ModelLayer.DTO.CourseDtos.CourseDto
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string Requirement { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Image { get; set; }     
        public string Language { get; set; }
        public string CategoryName { get; set; }
        public string? InstructorName { get; set; }
        public List<CommentResponseDTO> Comments { get; set; }
        public List<SectionResponseDTO> Sections { get; set; }
        public Instructor Instructor { get; set; }


    }
}
