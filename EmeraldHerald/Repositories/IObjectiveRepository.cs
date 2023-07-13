using FirelinkShrine.Models;

namespace FirelinkShrine.Repositories {
	public interface IObjectiveRepository {
		Task<IEnumerable<Objective>?> GetAllUserObjectives(int id);
		Task AddObjective(Objective objective);
		Task<Objective?> TryFindObjective(int id);
		Task DeleteObjective(Objective objective);
		Task UpdateObjective(Objective objective);
	}
}
