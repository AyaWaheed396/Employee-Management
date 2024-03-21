using System.ComponentModel.DataAnnotations;

namespace Company.PL.Models
{
	public class SignUpViewModel
	{
		
		[Required(ErrorMessage = "Email Is Required!!")]
		[EmailAddress(ErrorMessage = "Invalid Format For Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required!!")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password Is Required!!")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password Does Not Match Password!! ")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }
	}
}
