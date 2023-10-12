using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CommentDTOs
{
    public class CommentRequestDTO
    {
        [Required]
        public int CourseId { get; set; }
        [MaxLength(200)]
        public string CommentTxt { get; set; }
    }
}
