using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTO.CourseDtos.InstructorDto
{
    public class InstructorResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(300)]
        public string About { get; set; }
        [MaxLength(50)]
        public string Contact { get; set; }
        public string ImageUrl { get; set; }
        [MaxLength(300)]
        public string Studies { get; set; }


    }
}