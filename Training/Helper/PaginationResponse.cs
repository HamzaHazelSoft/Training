namespace Training.Helper
{
    public class PaginationResponse<T>
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
