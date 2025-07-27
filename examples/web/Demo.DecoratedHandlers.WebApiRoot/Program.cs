using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.BarDomain;
using Demo.DecoratedHandlers.FooDomain;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddOpenApi();

services.AddTransient<IRequestHandler<FooQuery, FooResponse>, FooQueryHandler>();
services.AddTransient<IRequestHandler<BarQuery, BarResponse>, BarQueryHandler>();
//services.AddPipelines();
services.AddDecoratedHandlers();
//services.AddDecoratedHandlers(x => x.ScanAssemblies = [typeof(FooQuery).Assembly, typeof(BarQuery).Assembly]);

//services.AddDecoratedHandlers<FancyGlobalPrefix.PipelineRegistry>();
//services.AddHostedService<DelayedLogHostedService>();
var app = builder.Build();

var foo = app.Services.GetRequiredService<IRequestHandler<FooQuery, FooResponse>>();
var bar = app.Services.GetRequiredService<IRequestHandler<BarQuery, BarResponse>>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
