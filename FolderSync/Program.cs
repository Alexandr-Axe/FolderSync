using System.Threading;

if (!Validate()) 
{
    Console.WriteLine("Validation has failed!");
    return;
}

CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    Console.WriteLine("Cancellation requested. Exiting sync loop...");
};

SyncConfig config = new SyncConfig {
    SourcePath = args[0],
    ReplicaPath = args[1],
    SyncIntervalSeconds = int.Parse(args[2]),
    LogFilePath = args[3]
};
Logger logger = new Logger(config.LogFilePath);
Synchroniser sync = new Synchroniser(config, logger, cts.Token);

logger.Log(LogLevel.INF, "FolderSync has booted up!");
logger.Log(LogLevel.DEB, "Arguments received:");
logger.Log(LogLevel.DEB, $"Source: {config.SourcePath}");
logger.Log(LogLevel.DEB, $"Replica: {config.ReplicaPath}");
logger.Log(LogLevel.DEB, $"Interval: {config.SyncIntervalSeconds}");
logger.Log(LogLevel.DEB, $"Logfile: {config.LogFilePath}");
logger.Log(LogLevel.INF, "Starting the program...");

sync.Start();

/// <summary>
/// Validates command line arguments and configuration before running the synchronisation.
/// </summary>
/// <returns>True if validation succeeds, false otherwise.</returns>
bool Validate()
{
    // args[0] = Source folder
    // args[1] = Replica folder
    // args[2] = Interval in seconds
    // args[3] = Logfile path

    // Checks if there's a correct number of arguments
    if (args.Length != 4)
    {
        Console.WriteLine("You have not provided 4 arguments!");
        Console.WriteLine("Usage: FolderSync.exe <source> <replica> <interval_s> <logfile>");

        return false;
    }

    // Checks if the arguments are empty
    foreach (string arg in args)
    {
        if (string.IsNullOrWhiteSpace(arg)) 
        {
            Console.WriteLine("Arguments must not be empty!");
            return false;
        }
    }

    // Checks if the source directory exists
    if (!Directory.Exists(args[0]))
    {
        Console.WriteLine($"Source directory does not exist: {args[0]}");
        return false;
    }

    // Checks if the replica directory exists
    if (!Directory.Exists(args[1]))
    {
        Console.WriteLine($"Replica directory does not exist: {args[1]}");
        return false;
    }

    // Checks if the source and replica directory are the same
    if (args[0].Equals(args[1])) 
    {
        Console.WriteLine($"Source and replica directory are the same");
        return false;
    }

    // Checks if the given interval is a number
    if (!int.TryParse(args[2], out _))
    {
        Console.WriteLine("You have not provided a valid argument: <interval_s>");
        Console.WriteLine("It has to be a number!");
        return false;
    }

    // Checks if the log file exists
    if (!File.Exists(args[3])) 
    {
        Console.WriteLine($"Log file does not exist: {args[3]}");
        return false;
    }

    // Checks if the log file is inside either folder
    string logDir = Path.GetDirectoryName(args[3]);
    if ((logDir.Equals(args[0], StringComparison.OrdinalIgnoreCase) ||
         logDir.Equals(args[1], StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("Log file cannot be inside the source or replica folder");
        return false;
    }

    return true;
}