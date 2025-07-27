using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.FooDomain;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddOpenApi();

services.AddTransient<IRequestHandler<FooQuery, FooResponse>, FooQueryHandler>();
//services.AddPipelines();
services.AddDecoratedHandlers();
//services.AddHostedService<DelayedLogHostedService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
