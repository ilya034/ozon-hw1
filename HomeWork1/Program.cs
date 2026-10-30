using HomeWork1.Application;
using HomeWork1.Infrastructure;
using HomeWork1.Infrastructure.FileSystem;
using HomeWork1.Presentation;
using Microsoft.Extensions.DependencyInjection;

var orderLogPath = PathLocator.FindFileUpwards("orders_log.csv")
                   ?? throw new FileNotFoundException("orders_log.csv not found");
var reportOutputDirectory = Path.GetDirectoryName(orderLogPath)!;

var services = new ServiceCollection();

services.AddApplication();
services.AddInfrastructure(orderLogPath);
services.AddPresentation(reportOutputDirectory);

await using var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<AppRunner>();
await app.RunAsync();
