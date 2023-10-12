using Microsoft.AspNetCore.Http;
using ModelLayer.DTO.CourseDtos.CommentDTOs;
using ModelLayer.Models.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CourseDto
{
    public class CourseDto
    {
        [MinLength(4)]
        public string CourseName { get; set; }
        [MinLength(5)]
        public string Level { get; set; }
        [MinLength(10)]
        public string Description { get; set; }
        [MinLength(11)]
        public string Requirement { get; set; }
        [Required]
        public string Image { get; set; }
        [MinLength(2)]
        public string Language { get; set; }
        [Range(1, int.MaxValue)]
        public int InstructorId { get; set; }
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public List<SectionDto> Sections { get; set; }
        

    }
}