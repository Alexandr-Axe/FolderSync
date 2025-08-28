using System;

public class Synchroniser
{
    public string SourcePath { get; set; }
    public string ReplicaPath { get; set; }
    public int SyncIntervalSeconds { get; set; }
    public string LogFilePath { get; set; }
    public Logger logger { get; set; }

    public Synchroniser(string sourcePath, string replicaPath, int syncIntervalSeconds, string logFilePath, Logger logger)
	{
        this.SourcePath = sourcePath;
        this.ReplicaPath = replicaPath;
        this.SyncIntervalSeconds = syncIntervalSeconds;
        this.LogFilePath = logFilePath;
        this.logger = logger;
	}

    public void Start() 
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                Synchronise();
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex.Message);
            }
            Thread.Sleep(SyncIntervalSeconds * 1000);
        }
        logger.Log("Synchronisation stopped!");
    }

    private void Synchronise() 
    {
        logger.Log("Synchronising...");
    }
}
