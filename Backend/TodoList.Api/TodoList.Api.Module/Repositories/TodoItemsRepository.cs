namespace TodoList.Api.Module.Repositories;

public class TodoItemsRepository : ITodoItemsRepository
{
    private readonly TodoContext _context;
    
    public TodoItemsRepository(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<TodoItem>> GetTodoItems()
    {
        return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
    }
}