namespace FirelinkShrine.Models.ViewModels {
	public class NoteIndexViewModel {
		public IEnumerable<Note>? Notes { get; }
		public Note? Note { get; set; }
		public PageViewModel PageViewModel { get; }

        public NoteIndexViewModel(IEnumerable<Note>? notes, PageViewModel pageViewModel) {
            Notes = notes;
			PageViewModel = pageViewModel;
        }
    }
}
