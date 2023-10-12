using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CommentDTOs
{
    public class CommentResponseDTO
    {
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
