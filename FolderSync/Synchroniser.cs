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
        logger.Log(LogLevel.INF, "Synchronising...");
    }
}