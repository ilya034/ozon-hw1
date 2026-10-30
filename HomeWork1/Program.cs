using HomeWork1.Application;
using HomeWork1.Infrastructure;
using HomeWork1.Infrastructure.FileSystem;
using HomeWork1.Presentation;
using Microsoft.Extensions.DependencyInjection;

const string orderLogFileName = "orders_log.csv";

var orderLogPath = PathLocator.FindFileUpwards(orderLogFileName);
if (orderLogPath is null)
{
    Console.Error.WriteLine(
        $"Файл {orderLogFileName} не найден в '{Environment.CurrentDirectory}' и родительских папках.");
    return 1;
}

var reportOutputDirectory = Path.GetDirectoryName(orderLogPath)!;

var services = new ServiceCollection();

services.AddApplication();
services.AddInfrastructure(orderLogPath);
services.AddPresentation(reportOutputDirectory);

await using var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<AppRunner>();
return await app.RunAsync(args);
