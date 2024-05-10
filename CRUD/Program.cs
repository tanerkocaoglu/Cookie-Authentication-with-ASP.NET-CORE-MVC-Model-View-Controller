using CRUD.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CRUD
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
			builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
			builder.Services.AddDbContext<DatabaseContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
				//options.UseLazyLoadingProxies();
			});

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.Name = "CRUD.auth";
					options.ExpireTimeSpan = TimeSpan.FromDays(7);
					options.SlidingExpiration = false;
					options.LoginPath = "/Account/Login";
					options.LogoutPath = "/Account/Logout";
					options.AccessDeniedPath = "/Home/AccessDenied";
				});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}