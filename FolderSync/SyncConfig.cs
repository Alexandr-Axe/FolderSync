using System;

/// <summary>
/// Stores all synchronisation configuration parameters.
/// </summary>
public class SyncConfig
{
    public string SourcePath { get; set; }          // Path to the source directory
    public string ReplicaPath { get; set; }         // Path to the replica directory
    public int SyncIntervalSeconds { get; set; }    // Synchronisation interval in secs
    public string LogFilePath { get; set; }         // Path to the log file
}