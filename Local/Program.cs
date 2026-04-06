using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Host...");

        var apiPath = Path.GetFullPath(@".\Api\Api.exe");
        var frontendPath = Path.GetFullPath(@".\Frontend");
        var apiProcess = StartProcess(apiPath, "", "API");
        var frontendProcess = StartProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "npm", "serve.cmd"), "-s -l 8080", "FRONTEND", frontendPath);

        Console.WriteLine("Both services started.");
        Console.WriteLine("Press Ctrl+C to stop.");

        var exitEvent = new TaskCompletionSource();

        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            exitEvent.SetResult();
        };

        await exitEvent.Task;

        Console.WriteLine("Shutting down...");

        KillProcess(apiProcess);
        KillProcess(frontendProcess);
    }

    static Process StartProcess(string fileName, string args, string name, string? workingDir = null)
    {
        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = args,
            WorkingDirectory = workingDir ?? Path.GetDirectoryName(fileName)!,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data != null)
                Console.WriteLine($"[{name}] {e.Data}");
        };

        process.ErrorDataReceived += (s, e) =>
        {
            if (e.Data != null)
                Console.WriteLine($"[{name} ERROR] {e.Data}");
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }

    static void KillProcess(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(true);
                process.WaitForExit();
            }
        }
        catch { }
    }
}