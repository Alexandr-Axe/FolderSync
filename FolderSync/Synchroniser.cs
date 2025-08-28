using System;

public class Synchroniser
{
    private readonly SyncConfig config;
    private readonly Logger logger;

    public Synchroniser(SyncConfig config, Logger logger)
	{
        this.config = config;
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
                logger.Log(LogLevel.ERR, ex.Message);
            }
            Thread.Sleep(config.SyncIntervalSeconds * 1000);
        }
        logger.Log(LogLevel.INF, "Synchronisation stopped!");
    }

    private void Synchronise() 
    {
        logger.Log(LogLevel.INF, "Synchronising...");
    }
}
