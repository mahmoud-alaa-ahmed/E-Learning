using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
    public class Instructor:BaseModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string About { get; set; }
        public string Contact { get; set; }
        public string ImageUrl  { get; set; }
        [MaxLength(300)]
        public string Studies { get; set; }
       
       
    }
}
