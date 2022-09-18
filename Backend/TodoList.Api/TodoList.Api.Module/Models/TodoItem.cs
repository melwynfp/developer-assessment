using System.ComponentModel.DataAnnotations;

namespace TodoList.Api
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
