using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CategoryDto
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        [MaxLength(80)]
        public string Name { get; set; }
        public int? SubId { get; set; }
    }
}
