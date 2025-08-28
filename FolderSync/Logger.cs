using System;
using System.IO;

public class Logger
{
	private readonly string logFilePath;

    public Logger(string logFilePath)
	{
		this.logFilePath = logFilePath;
	}

	public void Log(LogLevel level, string message) 
	{
        string line = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} [{level}] {message}";
		Console.WriteLine(line);
        try 
        {
            File.AppendAllText(logFilePath, line + Environment.NewLine); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logger ERROR: {ex.Message}");
        }
    }
}