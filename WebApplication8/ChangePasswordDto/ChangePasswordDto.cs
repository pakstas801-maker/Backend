using Microsoft.AspNetCore.Identity;
namespace WebApplication8.ChangePasswordDto
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
