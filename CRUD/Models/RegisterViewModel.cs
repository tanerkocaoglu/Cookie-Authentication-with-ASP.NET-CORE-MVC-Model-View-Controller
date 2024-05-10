using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
		[StringLength(30, ErrorMessage = "Kullanıcı adı 30 karakterden fazla olamaz.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Şifre zorunludur.")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
		[MaxLength(16, ErrorMessage = "Şifre en fazla 16 karakter olabilir.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Şifreyi tekrar giriniz.")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
		[MaxLength(16, ErrorMessage = "Şifre en fazla 16 karakter olabilir.")]
		[Compare(nameof(Password), ErrorMessage = "Girdiğiniz şifreler uyuşmuyor.")]
		public string RePassword { get; set; }
	}
}
