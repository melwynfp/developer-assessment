using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace TodoList.Api.Common.Hosting.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IApplicationBuilder AddSwaggerUi(this IApplicationBuilder application)
    {
        return application.UseSwagger()
            .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList.Api v1"));
    }
    
    public static void LoadTodoApiModuleStartups(this WebApplicationBuilder builder)
    {
        var assemblyPaths = GetFilePathsInExecutionDirectory("TodoList", "dll");
        var assemblies = assemblyPaths.Select(Assembly.LoadFrom);
        builder.LoadModuleStartups(assemblies);
    }
    
    private static void LoadModuleStartups(this WebApplicationBuilder builder, IEnumerable<Assembly> assemblies)
    {
        foreach (var startupType in GetTypesAssignableTo(typeof(IStartupConfiguration), assemblies))
        {
            var startupImplementation = Activator.CreateInstance(startupType) as IStartupConfiguration ?? throw new Exception($"Could not create instance of {nameof(IStartupConfiguration)}");
            startupImplementation.ConfigureServices(builder.Services, builder.Configuration);
        }
    }

    private static IEnumerable<Type> GetTypesAssignableTo(Type assigneeType, IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(a => a.GetTypes().Where(t => t.IsAssignableTo(assigneeType) && t.IsClass))
            .Where(item => item != null)
            .Select(item=> item!);;
    }
    
    private static IEnumerable<string> GetFilePathsInExecutionDirectory(string contains, string extension)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var currentAssemblyPath = Path.GetDirectoryName(currentAssembly.Location);

        var filesInAssemblyPath = Directory.GetFiles(currentAssemblyPath ?? throw new Exception("Invalid Assembly Path"));

        return filesInAssemblyPath.Where(p => Path.GetFileName(p).Contains(contains) && Path.GetExtension(p) == $".{extension}");
    }
}