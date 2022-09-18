using TodoList.Api.Common.Hosting.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddConfigs(builder.Configuration)
    .ConfigureSwaggerGeneration()
    .ConfigureHsts()
    .AddCors();

const string corsPolicy = "corsOriginPolicy";
builder.Services.AddControllers();
builder.Services.ConfigureCors(corsPolicy);
builder.Services.AddHealthChecks();
builder.LoadTodoApiModuleStartups();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
    app.AddSwaggerUi();
else
    app.UseHsts();

app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.UseCors(corsPolicy);
app.MapControllers();

await app.RunAsync();