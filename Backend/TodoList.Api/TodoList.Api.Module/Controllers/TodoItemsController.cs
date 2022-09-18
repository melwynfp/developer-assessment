using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Module.Services;

namespace TodoList.Api.Module.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "API - Todo Items")]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoItemsService _todoItemsService;

        public TodoItemsController(TodoContext context, ILogger<TodoItemsController> logger, ITodoItemsService todoItemsService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _todoItemsService = todoItemsService ?? throw new ArgumentNullException(nameof(todoItemsService));
        }

        // GET: api/TodoItems
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _todoItemsService.GetTodoItems();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id:guid}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _context.TodoItems.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id:guid}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            if (TodoItemDescriptionExists(todoItem.Description))
            {
                return BadRequest("Description already exists");
            } 

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        } 

        private bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        private bool TodoItemDescriptionExists(string? description)
        {
            return _context.TodoItems
                   .Any(x => string.Equals(x.Description, description, StringComparison.InvariantCultureIgnoreCase) && !x.IsCompleted);
        }
    }
}
