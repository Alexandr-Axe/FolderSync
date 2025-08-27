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
### What it does
* Copies new and updated files from source to replica
* Removes files and folders from replica that no longer exist in source
* Logs every operation to both console and the log file
* Sync runs automatically every X seconds (interval you set)
### Limitations
* Sync is always one-way (source -> replica)
* No third-party sync libraries used
* Uses built-in .NET features (File, Directory, MD5 for file comparison when needed)
## Author
Bc. Alexandr Sekera\
[LinkedIn profile](https://www.linkedin.com/in/alexandr-sekera/)
