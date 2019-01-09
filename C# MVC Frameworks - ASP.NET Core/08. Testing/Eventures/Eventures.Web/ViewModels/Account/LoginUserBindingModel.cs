using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels.Account
{
    public class LoginUserBindingModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
