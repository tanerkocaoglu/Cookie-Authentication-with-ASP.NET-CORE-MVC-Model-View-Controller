using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
	public class UserModel
	{
		public Guid Id { get; set; }
		public string? FullName { get; set; } = null;
		public string Username { get; set; }
		public bool Locked { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string Role { get; set; } = "user";
	}

	public class CreateUserViewModel
	{
		[Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
		[StringLength(30, ErrorMessage = "Kullanıcı adı 30 karakterden fazla olamaz.")]
		public string Username { get; set; }

		[Required]
		[StringLength(50)]
		public string FullName { get; set; }
		public bool Locked { get; set; }

		[Required]
		[StringLength(50)]
		public string Role { get; set; } = "user";

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
