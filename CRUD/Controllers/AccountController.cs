using CRUD.Entities;
using CRUD.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CRUD.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly DatabaseContext _dataBaseContext;
		private readonly IConfiguration _configuration;

		public AccountController(DatabaseContext dataBaseContext, IConfiguration configuration)
		{
			_dataBaseContext = dataBaseContext;
			_configuration = configuration;
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				string hashedPassword = DoMD5HashedString(model.Password);

				User user = _dataBaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower()
				&& x.Password == hashedPassword);

				if (user != null)
				{
					if (user.Locked)
					{
						ModelState.AddModelError(nameof(model.Username), "Kullanıcı aktif değil.");
						return View(model);
					}

					List<Claim> claims = new()
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
						new Claim(ClaimTypes.Role, user.Role),
						new Claim("Username", user.Username)
					};

					ClaimsIdentity identity = new(claims,
						CookieAuthenticationDefaults.AuthenticationScheme);
					ClaimsPrincipal principal = new(identity);

					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
						principal);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
				}
			}
			return View(model);
		}

		private string DoMD5HashedString(string s)
		{
			string MD5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
			string salted = s + MD5Salt;
			string hashed = salted.MD5();
			return hashed;
		}

		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (_dataBaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
				{
					ModelState.AddModelError(nameof(model.Username), "Kullanıcı bulunuyor.");
					return View(model);
				}

				string hashedPassword = DoMD5HashedString(model.Password);

				User user = new()
				{
					Username = model.Username,
					Password = hashedPassword
				};
				_dataBaseContext.Users.Add(user);
				int affectedRows = _dataBaseContext.SaveChanges();

				if (affectedRows == 0)
				{
					ModelState.AddModelError("", "Kullanıcı eklenemedi.");
				}
				return RedirectToAction(nameof(Login));
			}
			return View(model);
		}

		public IActionResult Profile()
		{
			ProfileInfoLoader();

			return View();
		}

		private void ProfileInfoLoader()
		{
			Guid userid = new(User.FindFirstValue(ClaimTypes.NameIdentifier));
			User user = _dataBaseContext.Users.SingleOrDefault(x => x.Id == userid);

			ViewData["FullName"] = user.FullName;
		}

		public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullname)
		{
			if (ModelState.IsValid)
			{
				Guid userid = new(User.FindFirstValue(ClaimTypes.NameIdentifier));
				User user = _dataBaseContext.Users.SingleOrDefault(x => x.Id == userid);

				user.FullName = fullname;
				_dataBaseContext.SaveChanges();
				return RedirectToAction(nameof(Profile));
			}
			ProfileInfoLoader();
			return View("Profile");
		}
		[HttpPost]
		public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
		{
			if (ModelState.IsValid)
			{
				Guid userid = new(User.FindFirstValue(ClaimTypes.NameIdentifier));
				User user = _dataBaseContext.Users.SingleOrDefault(x => x.Id == userid);

				string hashedPassword = DoMD5HashedString(password);

				user.Password = hashedPassword;
				_dataBaseContext.SaveChanges();

				ViewData["Result"] = "PasswordChanged";
			}
			ProfileInfoLoader();
			return View("Profile");
		}
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
