using System;
using System.IO;

public class Logger
{
	private readonly string logFilePath;

    public Logger(string logFilePath)
	{
		this.logFilePath = logFilePath;
	}

	public void Log(string message) 
	{
        string line = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} {message}";
		Console.WriteLine(line);
        File.AppendAllText(logFilePath, line + Environment.NewLine);
    }
}
