namespace TodoList.Api.Common.Hosting.ConfigSections;

public class CorsConfig
{
    public string? AllowOrigins { get; set; }
    public string? ExposedHeaders { get; set; }
}