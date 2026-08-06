using Training.Enums;

namespace Training.Helper
{
    public class PaginationRequest
    {

        public int CurrentPage { get; set; } = 1; //Prop1
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public SortDirection SortOrder { get; set; } = SortDirection.DESC;
        public string? SearchItem { get; set; }

    }
}
