using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTO.AuthDtos
{
    public class RoleSetModelDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
