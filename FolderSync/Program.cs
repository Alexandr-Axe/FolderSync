if (args.Length != 4)
{
    Console.WriteLine("You have not provided 4 arguments!");
    Console.WriteLine("Usage: FolderSync.exe <source> <replica> <interval_s> <logfile>");

    return;
}

if (!int.TryParse(args[2], out _)) 
{
    Console.WriteLine("You have not provided a valid argument: <interval_s>");
    Console.WriteLine("It has to be a number!");
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