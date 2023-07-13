using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirelinkShrine.Models.ViewModels {
    public class FilterViewModel {
        public SelectList Statuses { get; }
        public string SelectedStatus { get; }
        public string? SelectedName { get; }

        public FilterViewModel(string status, string? name) {
            var statuses = new List<string> {
                "All",
                "Active",
                "Completed",
                "Abandoned"
            };
            Statuses = new SelectList(statuses, status);
            SelectedStatus = status;
            SelectedName = name;
        }
    }
}
