
namespace Training.Helper
{
    public class PaginationRequest
    {

        public int CurrentPage { get; set; } = 1; //Prop1
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id ASC";
        public string? SearchItem { get; set; }

    }
}
