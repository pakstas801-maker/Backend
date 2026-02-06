using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Dto
{
    public class LoginDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
