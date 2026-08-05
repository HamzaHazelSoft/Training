namespace Training.Helper
{
    public class PaginationRequest
    {

        public int CurrentPage { get; set; } = 1; //Prop1

        private const int MaxPageSize = 100;

        private int _pageSize = 10;
        public int PageSize //Prop2
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; } = "DESC";
        public string? Search { get; set; }
    }
}
