using System.ComponentModel.DataAnnotations;

namespace juliWebApi.model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Firstmame is required")]
        public string? FirstName { get; set; }
        
        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; set; }


        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}