namespace FirelinkShrine.Models.ViewModels {
	public class ObjectiveIndexViewModel {
        public IEnumerable<Objective> Objectives { get; }
        public PageViewModel PageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public SortViewModel SortViewModel { get; }
        public ObjectiveIndexViewModel(IEnumerable<Objective> objectives, PageViewModel pageViewModel,
            FilterViewModel filterViewModel, SortViewModel sortViewModel) 
        {
            Objectives = objectives;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
