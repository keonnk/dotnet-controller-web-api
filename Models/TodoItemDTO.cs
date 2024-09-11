namespace dotnet_controller_web_api.Models
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }

    //TodoItemDTO with all optional fields to support PUT request partial updates
    public class TodoItemUpdateDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsComplete { get; set; }
    }
}