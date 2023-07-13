using FirelinkShrine.Models;
using FirelinkShrine.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FirelinkShrine {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			var jwtSettings = builder.Configuration.GetSection("JwtSettings");
			string? connString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connString));
			builder.Services.AddControllersWithViews();
			builder.Services.AddAuthorization();
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
				options.LoginPath = "/Home/Login";
			});
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IObjectiveRepository, ObjectiveRepository>();
			builder.Services.AddScoped<INoteRepository, NoteRepository>();

			var app = builder.Build();

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			SeedData.Initialize(services);

			if (!app.Environment.IsDevelopment()) {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
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