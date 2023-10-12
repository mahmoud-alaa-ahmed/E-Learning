using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.InstructorDto
{
    public class InstructorRequestDTO
    {
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string About { get; set; }
        [MaxLength(50)]
        public string Contact { get; set; }
        public string Image { get; set; }
        [MaxLength(300)]
        public string Studies { get; set; }
        

    }
}
