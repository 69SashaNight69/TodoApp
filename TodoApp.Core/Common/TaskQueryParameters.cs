namespace TodoApp.Core.Common
{
    public class TaskQueryParameters
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}