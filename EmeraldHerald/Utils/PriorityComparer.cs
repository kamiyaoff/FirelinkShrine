namespace FirelinkShrine.Utils {
    public class PriorityComparer : IComparer<string> {
        public int Compare(string? x, string? y) {
            if (x == "High")
                return y == "High" ? 0 : -1;
            else if (x == "Medium")
                return y == "High" ? 1 : -1;
            else if (x == "Low")
                return y == "Low" ? 0 : 1;
            else
                return 1;
        }
    }
}
