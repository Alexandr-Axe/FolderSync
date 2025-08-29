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
        return NotValidate("You have not provided 4 arguments!\nUsage: FolderSync.exe <source> <replica> <interval_s> <logfile>");
    }

    // Checks if the arguments are empty
    for (int i = 0; i < args.Length; i++)
    {
        if (string.IsNullOrWhiteSpace(args[i]))
            return NotValidate($"{i + 1}. argument is empty!");
    }

    // Checks if the source directory exists
    if (!Directory.Exists(args[0]))
    {
        return NotValidate($"Source directory does not exist: {args[0]}");
    }

    // Checks if the replica directory exists
    if (!Directory.Exists(args[1]))
    {
        return NotValidate($"Replica directory does not exist: {args[1]}");
    }

    // Checks if the source and replica directory are the same
    if (args[0].Equals(args[1])) 
    {
        return NotValidate($"Source and replica directory are the same");
    }

    // Checks if the given interval is a number
    if (!int.TryParse(args[2], out _))
    {
        return NotValidate("You have not provided a valid interval. It has to be a whole number!");
    }

    // Checks if the given interval is zero
    if (int.Parse(args[2]) == 0) 
    {
        return NotValidate("Synchronisation interval cannot be 0!");
    }

    // Checks if the log file exists
    if (!File.Exists(args[3])) 
    {
        return NotValidate($"Log file does not exist: {args[3]}");
    }

    // Checks if the log file is inside either folder
    string logDir = Path.GetDirectoryName(args[3]);
    if ((logDir.Equals(args[0], StringComparison.OrdinalIgnoreCase) ||
         logDir.Equals(args[1], StringComparison.OrdinalIgnoreCase)))
    {
        return NotValidate("Log file cannot be inside the source or replica folder");
    }

    return true;
}

bool NotValidate(string message) 
{
    Console.WriteLine(message);
    return false;
}