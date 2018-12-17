using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels.Account
{
    public class RegisterUserBindingModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "The {0} should be at least {1} characters long.")]
        [RegularExpression(@"^[\w-.*~]+$", ErrorMessage =
            "{0} may contain only alphanumeric characters, dashes, underscores, dots, asterisks or tildes.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The {0} should be at least {1} characters long.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[\d]{10}$", ErrorMessage = "{0} should consist of exactly 10 characters.")]
        public string UCN { get; set; }
    }
}
