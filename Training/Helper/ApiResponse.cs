namespace Training.Helper
{
    public class ApiResponse<T>
    {
       public Payload<T> Payload { get; set; }
    }
    public class Payload<T>
    {
        public Item<T> Items { get; set; }
        public int Status { get; set; } = 0; // 0 represnts failure
        public string Message { get; set; } = "";
        public List<string> Errors { get; set; } = new List<string>();
    }
    public class Item<T>
    {
        public T? Data { get; set; }
    }
}
