﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
