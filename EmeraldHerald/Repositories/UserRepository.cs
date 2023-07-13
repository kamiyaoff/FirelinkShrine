using FirelinkShrine.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FirelinkShrine.Repositories {
	public class UserRepository : IUserRepository {
		private readonly ApplicationContext _db;

		public UserRepository(ApplicationContext dbContext) => _db = dbContext;

		public async Task AddUser(User user) {
			_db.Users.Add(user);
			await _db.SaveChangesAsync();
		}

		public async Task<User?> GetUserByPasswordAsync(User inputData) {
			User? user = await _db.Users.FirstOrDefaultAsync(u =>
				u.Name == inputData.Name && u.Password == inputData.Password);
			return user;
		}

		public async Task<bool> UserExists(User user) {
			return await _db.Users.AnyAsync(u => u.Name == user.Name);
		}
	}
}
