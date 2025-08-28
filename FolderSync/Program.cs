if (!Validate()) 
{
    Console.WriteLine("Validation has failed!");
    return;
}

string source = args[0];
string replica = args[1];
int interval = int.Parse(args[2]);
string logfile = args[3];
Logger logger = new Logger(logfile);
Synchroniser sync = new Synchroniser(source, replica, interval, logfile, logger);

logger.Log("FolderSync has booted up!");
logger.Log("Arguments received:");
logger.Log($"Source: {source}");
logger.Log($"Replica: {replica}");
logger.Log($"Interval: {interval}");
logger.Log($"Logfile: {logfile}");
logger.Log("Starting the program...");

sync.Start();

bool Validate()
{
    // args[0] = Source folder
    // args[1] = Replica folder
    // args[2] = Interval in seconds
    // args[3] = Logfile path

    if (args.Length != 4)
    {
        Console.WriteLine("You have not provided 4 arguments!");
        Console.WriteLine("Usage: FolderSync.exe <source> <replica> <interval_s> <logfile>");

        return false;
    }

    foreach (string arg in args)
    {
        if (string.IsNullOrWhiteSpace(arg)) 
        {
            Console.WriteLine("Arguments must not be empty!");
            return false;
        }
    }

    if (!Directory.Exists(args[0]))
    {
        Console.WriteLine($"Source directory does not exist: {args[0]}");
        return false;
    }

    if (!Directory.Exists(args[1]))
    {
        Console.WriteLine($"Replica directory does not exist: {args[1]}");
        return false;
    }

    if (args[0].Equals(args[1])) 
    {
        Console.WriteLine($"Source and replica directory are the same");
        return false;
    }

    if (!int.TryParse(args[2], out _))
    {
        Console.WriteLine("You have not provided a valid argument: <interval_s>");
        Console.WriteLine("It has to be a number!");
        return false;
    }

    if (!File.Exists(args[3])) 
    {
        Console.WriteLine($"Log file does not exist: {args[3]}");
        return false;
    }

    string logDir = Path.GetDirectoryName(args[3]);
    if ((logDir.Equals(args[0], StringComparison.OrdinalIgnoreCase) ||
         logDir.Equals(args[1], StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("Log file cannot be inside the source or replica folder");
        return false;
    }

    return true;
}