using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.Dtos
{
	public class RegisterDto
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$",ErrorMessage ="UnVaildPassword")]
        public string Password { get; set; }
    }
}
