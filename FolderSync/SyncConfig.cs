using System;

public class SyncConfig
{
    public string SourcePath { get; set; }
    public string ReplicaPath { get; set; }
    public int SyncIntervalSeconds { get; set; }
    public string LogFilePath { get; set; }
}
