using HomeWork1.Application;
using HomeWork1.Infrastructure;
using HomeWork1.Presentation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddApplication();
services.AddInfrastructure();
services.AddPresentation();

await using var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<AppRunner>();
await app.RunAsync();