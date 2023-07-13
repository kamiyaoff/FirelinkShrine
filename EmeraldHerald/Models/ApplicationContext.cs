using Microsoft.EntityFrameworkCore;

namespace FirelinkShrine.Models {
	public class ApplicationContext : DbContext {
		public DbSet<Objective> Objectives { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Note> Notes { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
	}
}
