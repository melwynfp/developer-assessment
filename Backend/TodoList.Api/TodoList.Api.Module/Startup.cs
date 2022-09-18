using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Api.Common;
using TodoList.Api.Module.Repositories;
using TodoList.Api.Module.Services;

namespace TodoList.Api.Module;

public class Startup : IStartupConfiguration
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoItemsDB"));
        services.AddScoped<ITodoItemsService, TodoItemsService>();
        services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
    }
}