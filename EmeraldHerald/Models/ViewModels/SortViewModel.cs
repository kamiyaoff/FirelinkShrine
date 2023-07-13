using FirelinkShrine.Utils;

namespace FirelinkShrine.Models.ViewModels {
    public class SortViewModel {
        public SortState NameSort { get; }
        public SortState StatusSort { get; }
        public SortState PrioritySort { get; }
        public SortState CreatedSort { get; }
        public SortState DeadlineSort { get; }
        public SortState Current { get; }

        public SortViewModel(SortState sortOrder) {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            StatusSort = sortOrder == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;
            PrioritySort = sortOrder == SortState.PriorityAsc ? SortState.PriorityDesc : SortState.PriorityAsc;
            CreatedSort = sortOrder == SortState.CreatedAsc ? SortState.CreatedDesc : SortState.CreatedAsc;
            DeadlineSort = sortOrder == SortState.DeadlineAsc? SortState.DeadlineDesc : SortState.DeadlineAsc;
            Current = sortOrder;
        }
    }
}
