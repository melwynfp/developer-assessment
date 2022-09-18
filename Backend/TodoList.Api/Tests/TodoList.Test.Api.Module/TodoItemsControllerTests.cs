using AutoFixture.Idioms;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TodoList.Api;
using TodoList.Api.Module.Controllers;
using TodoList.Api.Module.Persistence;
using TodoList.Api.Module.Services;
using TodoList.Test.TestCommon;
using Xunit;

namespace TodoList.Test.Api.Module
{
    public class TodoItemsControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Constructor_GuardsAgainstNulls(GuardClauseAssertion guardClauseAssertion)
        {
            guardClauseAssertion.Verify(typeof(TodoItemsController).GetConstructors());
        }
        
        [Theory(DisplayName = "GetTodoItems - Throws exception if service throws exception")]
        [AutoMoqData]
        public async void GetTodoItems_ThrowsException(Mock<ITodoItemsService> mockTodoService, Mock<ILogger<TodoItemsController>> logger, Mock<TodoContext> todoContext)
        {
            mockTodoService.Setup(x => x.GetTodoItems()).Throws<Exception>();
            var todoItemsController = new TodoItemsController(todoContext.Object, logger.Object, mockTodoService.Object);

            await todoItemsController.Invoking(x => x.GetTodoItems())
                .Should().ThrowExactlyAsync<Exception>(); 
        }
    
        [Theory(DisplayName = "GetTodoItems - Returns a list of Todos")]
        [AutoMoqData]
        public async void GetTodoItems_ReturnsSuccessWithData(Mock<ITodoItemsService> mockTodoService, Mock<ILogger<TodoItemsController>> logger, Mock<TodoContext> todoContext, IReadOnlyCollection<TodoItem> todoItems)
        {
            mockTodoService.Setup(x => x.GetTodoItems())
                .ReturnsAsync(todoItems);
            var todoItemsController = new TodoItemsController(todoContext.Object, logger.Object, mockTodoService.Object);
        
            var response = await todoItemsController.GetTodoItems();
            response.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<TodoItem>>()
                .Which.Should().HaveSameCount(todoItems);
        }
    }
}
