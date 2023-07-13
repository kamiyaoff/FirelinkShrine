using FirelinkShrine.Models;

namespace FirelinkShrine.Repositories {
	public interface INoteRepository {
		Task<List<Note>?> GetNotesByUser(int userId);
		Task AddNote(Note note);
		Task DeleteNote(Note note);
		Task UpdateNote(Note note);
		Task<Note?> FindById(int noteId);
	}
}
