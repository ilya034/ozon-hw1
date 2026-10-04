using System;
using System.Threading.Tasks;

namespace HomeWork1.Presentation;

public class AppRunner
{
    public AppRunner()
    {
    }

    public async Task RunAsync()
    {
        // Minimal runner implementation — extend with application flow as needed
        Console.WriteLine("Application started. Implement your application logic in AppRunner.RunAsync().");
        await Task.CompletedTask;
    }
}

