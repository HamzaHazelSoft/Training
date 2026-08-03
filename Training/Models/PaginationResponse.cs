namespace Training.Models
{
    public class PaginationResponse<T>
    {
        public IEnumerable<T> Data { get; set; } //Here student details will be stored in Data property
        public int Count { get; set; } // Here total number of students exist in DB
        public string? nextPage { get; set; }
        public string? previousPage { get; set; }   

    }
}
