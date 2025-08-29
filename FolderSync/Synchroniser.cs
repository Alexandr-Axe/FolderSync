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
        SyncDir(config.SourcePath, config.ReplicaPath);
        logger.Log(LogLevel.INF, "Deleting all extra files from replica...");
        RemoveExtras(config.SourcePath, config.ReplicaPath);
        logger.Log(LogLevel.INF, "Directory synchronization completed.");
    }

    private void SyncDir(string sourceDir, string replicaDir) 
    {
        string fileName = string.Empty;
        string replicaFile = string.Empty;
        string dirName = string.Empty;
        string replicaSubDir = string.Empty;

        // Create directory if doesn't exist
        if (!Directory.Exists(replicaDir))
        {
            Directory.CreateDirectory(replicaDir);
            logger.Log(LogLevel.INF, $"Created directory: {replicaDir}");
        }

        // Copying files
        foreach (var sourceFile in Directory.GetFiles(sourceDir))
        {
            fileName = Path.GetFileName(sourceFile);
            replicaFile = Path.Combine(replicaDir, fileName);

            // File doesn't exist
            if (!File.Exists(replicaFile)) 
            {
                File.Copy(sourceFile, replicaFile, true);
                logger.Log(LogLevel.INF, $"Created file: {fileName}");
            }
            // File was changed
            else if (File.GetLastWriteTimeUtc(sourceFile) != File.GetLastWriteTimeUtc(replicaFile))
            {
                File.Copy(sourceFile, replicaFile, true);
                logger.Log(LogLevel.INF, $"Updated file: {fileName}");
            }
            // File is the same
            else logger.Log(LogLevel.WAR, $"Skipped file: {fileName}");
        }

        // Copying subdirectories
        foreach (var sourceSubDir in Directory.GetDirectories(sourceDir))
        {
            dirName = Path.GetFileName(sourceSubDir);
            replicaSubDir = Path.Combine(replicaDir, dirName);

            SyncDir(sourceSubDir, replicaSubDir);
        }
    }

    private void RemoveExtras(string sourceDir, string replicaDir) 
    {
        string fileName = string.Empty;
        string sourceFile = string.Empty;
        string dirName = string.Empty;
        string sourceSubDir = string.Empty;

        foreach (var replicaFile in Directory.GetFiles(replicaDir))
        {
            fileName = Path.GetFileName(replicaFile);
            sourceFile = Path.Combine(sourceDir, fileName);
            if (!File.Exists(sourceFile))
            {
                File.Delete(replicaFile);
                logger.Log(LogLevel.INF, $"Removed file: {fileName}");
            }
        }

        foreach (var replicaSubDir in Directory.GetDirectories(replicaDir))
        {
            dirName = Path.GetFileName(replicaSubDir);
            sourceSubDir = Path.Combine(sourceDir, dirName);
            if (!Directory.Exists(sourceSubDir))
            {
                Directory.Delete(replicaSubDir, true);
                logger.Log(LogLevel.INF, $"Removed directory: {dirName}");
            }
            else
                RemoveExtras(sourceSubDir, replicaSubDir);
        }
    }
}