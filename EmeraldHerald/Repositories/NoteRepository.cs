using FirelinkShrine.Models;
using Microsoft.EntityFrameworkCore;

namespace FirelinkShrine.Repositories {
	public class NoteRepository : INoteRepository {
		private readonly ApplicationContext _db;

        public NoteRepository(ApplicationContext context) {
			_db = context;
        }

        public async Task AddNote(Note note) {
			_db.Notes.Add(note);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteNote(Note note) {
			_db.Notes.Remove(note);
			await _db.SaveChangesAsync();
		}

		public async Task<Note?> FindById(int noteId) {
			var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
			return note;
		}

		public async Task<List<Note>?> GetNotesByUser(int userId) {
			var notes = await _db.Notes.Where(n => n.UserId == userId).OrderByDescending(n => n.Created).ToListAsync();
			return notes;
		}

		public async Task UpdateNote(Note note) {
			_db.Notes.Update(note);
			await _db.SaveChangesAsync();
		}
	}
}
