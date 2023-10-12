using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
    public class Category:BaseModel
    {
        [ForeignKey("category")]
        public int? SubId { get; set; }
        public virtual Category? category { get; set; }
    }
}