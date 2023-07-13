namespace FirelinkShrine.Utils {
    public class StatusComparer : IComparer<string> {
        public int Compare(string? x, string? y) {
            if (x == "Active")
                return y == "Active" ? 0 : -1;
            else if (x == "Completed")
                return y == "Active" ? 1 : -1;
            else if (x == "Abandoned")
                return y == "Abandoned" ? 0 : 1;
            else
                return 1;
        }
    }
}
