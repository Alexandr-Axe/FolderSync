# FolderSync
A simple C# utility for one-way synchronization between a source and replica folder. The program regularly updates the replica so that it matches the source exactly. All changes (creation, copy, or removal of files and folders) are logged to a file and printed to the console.
## How to use
Run the program with:
```
FolderSync.exe <source_folder_path> <replica_folder_path> <sync_interval_seconds> <log_file_path>
```
* source_folder_path = path to folder you want to keep as reference
* replica_folder_path = path to folder that will be updated
* sync_interval_seconds = how often sync runs (in seconds)
* log_file_path = file for logging all operations
### Example
```
FolderSync.exe D:\Source D:\Replica 60 D:\folder_sync.log
```
### How It Works
FolderSync recursively synchronizes all files and subdirectories from source to replica.
* If the file exists in source and not in replica, it is copied.
* If the file exists in both but is updated in source, it is overwritten.
* Subfolders are created as needed and populated recursively.
* Any file or folder in replica that does not exist in source is deleted.
* All operations and errors are logged with timestamps and log levels.
### Features
* One-way synchronization
* Recursively copies new and updated files
* Removes files and folders from replica that no longer exist in source
* Periodic sync (user-specified interval)
* Detailed, timestamped logging to both file and the console
* Error handling
* Supports cancellation (graceful stop with Ctrl+C)
* Uses built-in .NET features (File, Directory, last-write-time comparison)
### Log Output Example
```
29-08-2025 08:03:11 [INF] FolderSync has booted up!
29-08-2025 08:03:11 [DEB] Arguments received:
29-08-2025 08:03:11 [DEB] Source: C:\Users\alexs\OneDrive\Plocha\source
29-08-2025 08:03:11 [DEB] Replica: C:\Users\alexs\OneDrive\Plocha\replica
29-08-2025 08:03:11 [DEB] Interval: 5
29-08-2025 08:03:11 [DEB] Logfile: C:\Users\alexs\Downloads\logFile.txt
29-08-2025 08:03:11 [INF] Starting the program...
29-08-2025 08:03:11 [INF] Starting directory synchronisation...
29-08-2025 08:03:11 [INF] Created file: alexandr.txt
29-08-2025 08:03:11 [INF] Updated file: varta.txt
29-08-2025 08:03:11 [WAR] Skipped file: ya.txt
29-08-2025 08:03:11 [INF] Deleting all extra files from replica...
29-08-2025 08:03:11 [INF] Removed directory: tohle není složka
29-08-2025 08:03:11 [INF] Directory synchronization completed.
29-08-2025 08:03:16 [INF] Starting directory synchronisation...
29-08-2025 08:03:16 [WAR] Skipped file: alexandr.txt
29-08-2025 08:03:16 [WAR] Skipped file: varta.txt
29-08-2025 08:03:16 [WAR] Skipped file: ya.txt
29-08-2025 08:03:16 [INF] Deleting all extra files from replica...
29-08-2025 08:03:16 [INF] Directory synchronization completed.
29-08-2025 08:03:21 [INF] Synchronisation stopped!
```
### Limitations
* Synchronization is always one-way
* Log file must not be inside source or replica folder
* Large/crowded directory trees may impact performance
* No two-way sync or conflict resolution
## Author
Bc. Alexandr Sekera\
[LinkedIn profile](https://www.linkedin.com/in/alexandr-sekera/)
