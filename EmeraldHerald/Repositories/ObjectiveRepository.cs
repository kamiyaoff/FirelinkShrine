using Azure.Core;
using FirelinkShrine.Models;
using Microsoft.EntityFrameworkCore;

namespace FirelinkShrine.Repositories {
	public class ObjectiveRepository : IObjectiveRepository {
		private readonly ApplicationContext _db;

		public ObjectiveRepository(ApplicationContext db) => _db = db;

		public async Task AddObjective(Objective objective) {
			_db.Objectives.Add(objective);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteObjective(Objective objective) {
			_db.Objectives.Remove(objective);
			await _db.SaveChangesAsync();
		}

		public async Task<IEnumerable<Objective>?> GetAllUserObjectives(int id) {
			var objectives = await _db.Objectives.Where(o => o.UserId == id).ToListAsync();
			return objectives;
		}

		public async Task<Objective?> TryFindObjective(int id) {
			var objective = await _db.Objectives.FirstOrDefaultAsync(o => o.Id == id);
			return objective;
		}

		public async Task UpdateObjective(Objective objective) {
			_db.Objectives.Update(objective);
			await _db.SaveChangesAsync();
		}
	}
}
