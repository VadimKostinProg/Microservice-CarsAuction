using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.Register
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
        public string Button { get; set; }
    }
}
