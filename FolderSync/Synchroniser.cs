using System;

/// <summary>
/// Handles the synchronisation loop and process.
/// </summary>
public class Synchroniser
{
    private readonly SyncConfig config;
    private readonly Logger logger;
    private readonly CancellationToken cancellationToken;

    public Synchroniser(SyncConfig config, Logger logger, CancellationToken cancellationToken)
    {
        this.config = config;
        this.logger = logger;
        this.cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Starts the main synchronisation loop.
    /// </summary>
    public void Start()
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                Synchronise();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.ERR, ex.Message);
            }
            Thread.Sleep(config.SyncIntervalSeconds * 1000);
        }
        logger.Log(LogLevel.INF, "Synchronisation stopped!");
    }

    /// <summary>
    /// Synchronises source and replica directories.
    /// </summary>
    private void Synchronise()
    {
        // Check if a file from source folder exists in the replica folder
        // If the file doesn't exist, copy it
        // If the file exists, check the last write time
        // If it's the same, ignore -> otherwise copy
        // All subfolders are handled the same way

        // Check if a file from replica folder exists in the source folder
        // If the file exists, ignore it -> otherwise delete it
        // All subfolders are handled the same way

        logger.Log(LogLevel.INF, "Starting directory synchronisation...");
        SyncDir();
        RemoveExtras();
        logger.Log(LogLevel.INF, "Directory synchronization completed.");
    }

    private void SyncDir() 
    {
        string fileName = string.Empty;
        string replicaFile = string.Empty;
        string dirName = string.Empty;
        string replicaSubDir = string.Empty;
        bool shouldCopy = false;

        foreach (var file in Directory.GetFiles(config.SourcePath))
        {
            fileName = Path.GetFileName(file);
            replicaFile = Path.Combine(config.ReplicaPath, fileName);
            shouldCopy = !File.Exists(replicaFile) ||
                File.GetLastWriteTimeUtc(file) != File.GetLastWriteTimeUtc(replicaFile);

            if (shouldCopy)
            {
                File.Copy(file, replicaFile, true);
                logger.Log(LogLevel.INF, $"Updated file: {fileName}");
            }
            else logger.Log(LogLevel.WAR, $"Skipped file: {fileName}");
        }
    }

    private void RemoveExtras() 
    {

    }
}