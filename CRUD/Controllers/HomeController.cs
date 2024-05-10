using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRUD.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		[AllowAnonymous]
		public IActionResult Privacy()
		{
			return View();
		}
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}