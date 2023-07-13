using EmeraldHerald.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirelinkShrine.Models {
	public class Objective {
		public int Id { get; set; }
		public string ShortGoal { get; set; } = null!;
		public string? Goal { get; set; }
		public string? Priority { get; set; }
		public string? Status { get; set; } = "Active";

		[NotMapped]
		public bool StatusBool => Status != "Active";
		public DateTime Created { get; set; } = DateTime.Now;
		public DateTime Deadline { get; set; } = DateTime.Now.AddDays(1);
		public int DeadlineInHours { get; set; }

		[NotMapped]
		public double TimeLeft => Math.Round(Deadline.Subtract(DateTime.Now).TotalSeconds);
		public DateTime? Completed { get; set; }
		public int UserId { get; set; }
		public User User { get; set; } = null!;
	}
}
