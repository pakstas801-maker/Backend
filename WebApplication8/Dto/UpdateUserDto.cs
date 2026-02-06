using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Dto
{
    public class UpdateUserDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
