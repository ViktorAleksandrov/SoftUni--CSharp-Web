using System.ComponentModel.DataAnnotations;

namespace CHUSHKA.Web.ViewModels.Account
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
