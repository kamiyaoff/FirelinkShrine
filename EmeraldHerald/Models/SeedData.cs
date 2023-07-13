using Microsoft.EntityFrameworkCore;

namespace FirelinkShrine.Models {
	public class SeedData {
		public static void Initialize(IServiceProvider serviceProvider) {
			using var context = new ApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>());
			if (context.Objectives.Any())
				return;

			var user = new User { Name = "realshit", Password = "realshit" };
			context.Users.Add(user);

			context.Objectives.AddRange(
				new Objective {
					ShortGoal = "Find something to eat",
					Goal = "Im really hungry and i guess if i dont eat ill die",
					Priority = "High",
					Status = "Active",
					User = user
				},
				new Objective {
					ShortGoal = "Get souls back",
					Goal = "Gael killed me again so i have to take my souls back or i lose them",
					Priority = "Low",
					Status = "Abandoned",
					User = user
				},
				new Objective {
					ShortGoal = "Upgrade Uchigatana to +10",
					Goal = "I finally found titanite slab, now i can upgrade my uchigatana",
					Priority = "Medium",
					Status = "Completed",
					User = user
				}
			);

			context.Notes.AddRange(
				new Note {
					Name = "First encounter dialogue",
					Body = "Welcome. To the painted world of Ariandel. I am Friede. I have long stood beside our blessed Father," +
					" and the rest of the Forlorn. But Forlorn thou seemeth not.",
				},
				new Note {
					Name = "Upon opening Shortcut",
					Body = "Be forewarned, eager Ash, hould this world wither and rot, Even then would Ariandel remain our home. " +
					"Leave us be, Ashen One. Sweep all thought of us from thy mind. As thy kind always have.",
				},
				new Note {
					Name = "Upon Giving Chillbite Ring",
					Body = "Ahh yes, there is a thing thou shouldst by rights possess. A remembrance of this cold world, " +
					"for the great Lord of Londor. May it help thee bear thy duty. (hands you the chillbite ring) " +
					"Now, return from whence thou cam'st. Thou'st a place in that world, and that alone is cause to rejoice.",
				}
			);

			context.SaveChanges();

		}
	}
}
