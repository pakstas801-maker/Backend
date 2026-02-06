using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace WebApplication8.Dto
{
    public class UserDto
    {
        [Required]
        [MinLength (6)]
      public int Id { get; set; }

        [Required]
        [MinLength (3)]

        public string Name { get; set; }

        
    }
}
