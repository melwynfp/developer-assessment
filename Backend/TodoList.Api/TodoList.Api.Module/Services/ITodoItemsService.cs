namespace TodoList.Api.Module.Services;

public interface ITodoItemsService
{
    public Task<IEnumerable<TodoItem>> GetTodoItems();
}