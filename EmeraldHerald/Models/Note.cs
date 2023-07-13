namespace FirelinkShrine.Models {
	public class Note {
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Body { get; set; }
		public DateTime Created { get; set; } = DateTime.Now;
		public DateTime Updated { get; set; } = DateTime.Now;
		public int UserId { get; set; }
		public User User { get; set; } = null!;
	}
}
