using FirelinkShrine.Models;
using FirelinkShrine.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace FirelinkShrine.Controllers {
	public class HomeController : Controller {
		private readonly IUserRepository _userRepo;
		const string AUTH_SCHEME = CookieAuthenticationDefaults.AuthenticationScheme;

		public HomeController(IUserRepository userRepo) {
			_userRepo = userRepo;
		}

		public IActionResult Index() {
			return View();
		}

		public IActionResult Login() {
			if (User.Identity is not null) {
				if (User.Identity.IsAuthenticated)
					return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([Bind("Name", "Password")] User inputData) {
			User? user = await _userRepo.GetUserByPasswordAsync(inputData);
			if (user is null)
				return BadRequest();

			return await LogInAsync(user);
		}

		public async Task<IActionResult> Logout() {
			await HttpContext.SignOutAsync(AUTH_SCHEME);
			return LocalRedirect("/");
		}

		public IActionResult Signup() {
			if (User.Identity is not null) {
				if (User.Identity.IsAuthenticated)
					return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Signup(User user) {
			if (!ModelState.IsValid)
				return View(user);

			if (await _userRepo.UserExists(user)) {
				ModelState.AddModelError("Name", "Name is already in use.");
				return View(user);
			}
			await _userRepo.AddUser(user);
			return await LogInAsync(user);
		}

		private async Task<IActionResult> LogInAsync(User user) {
			var claims = new[] {
				new Claim("UserId", user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Name)
			};

			var claimsIdentity = new ClaimsIdentity(claims, AUTH_SCHEME);
			await HttpContext.SignInAsync(AUTH_SCHEME, new ClaimsPrincipal(claimsIdentity));
			return LocalRedirect("/");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}