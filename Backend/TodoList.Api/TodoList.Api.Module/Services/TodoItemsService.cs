using TodoList.Api.Module.Repositories;

namespace TodoList.Api.Module.Services;

public class TodoItemsService : ITodoItemsService
{
    private readonly ITodoItemsRepository _todoItemsRepository;
    
    public TodoItemsService(ITodoItemsRepository todoItemsRepository)
    {
        _todoItemsRepository = todoItemsRepository ?? throw new ArgumentNullException(nameof(todoItemsRepository));
    }
    
    public async Task<IEnumerable<TodoItem>> GetTodoItems()
    {
        return await _todoItemsRepository.GetTodoItems();
    }
}