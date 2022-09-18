using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Api.Common;

public interface IStartupConfiguration
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}