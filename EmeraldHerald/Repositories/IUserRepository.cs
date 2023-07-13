using FirelinkShrine.Models;

namespace FirelinkShrine.Repositories {
	public interface IUserRepository {
		Task<User?> GetUserByPasswordAsync(User user);
		Task<bool> UserExists(User user);
		Task AddUser(User user);
	}
}
