namespace TodoList.Api.Module.Repositories;

public interface ITodoItemsRepository
{
    public Task<IEnumerable<TodoItem>> GetTodoItems();
}