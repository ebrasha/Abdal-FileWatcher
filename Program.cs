using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FileWatcherExample
{
    class Program
    {
        private static List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        static void Main()
        {
            var banner = @"

                                
░█████╗░██████╗░██████╗░░█████╗░██╗░░░░░
██╔══██╗██╔══██╗██╔══██╗██╔══██╗██║░░░░░
███████║██████╦╝██║░░██║███████║██║░░░░░
██╔══██║██╔══██╗██║░░██║██╔══██║██║░░░░░
██║░░██║██████╦╝██████╔╝██║░░██║███████╗
╚═╝░░╚═╝╚═════╝░╚═════╝░╚═╝░░╚═╝╚══════╝

███████╗██╗██╗░░░░░███████╗░██╗░░░░░░░██╗░█████╗░████████╗░█████╗░██╗░░██╗███████╗██████╗░
██╔════╝██║██║░░░░░██╔════╝░██║░░██╗░░██║██╔══██╗╚══██╔══╝██╔══██╗██║░░██║██╔════╝██╔══██╗
█████╗░░██║██║░░░░░█████╗░░░╚██╗████╗██╔╝███████║░░░██║░░░██║░░╚═╝███████║█████╗░░██████╔╝
██╔══╝░░██║██║░░░░░██╔══╝░░░░████╔═████║░██╔══██║░░░██║░░░██║░░██╗██╔══██║██╔══╝░░██╔══██╗
██║░░░░░██║███████╗███████╗░░╚██╔╝░╚██╔╝░██║░░██║░░░██║░░░╚█████╔╝██║░░██║███████╗██║░░██║
╚═╝░░░░░╚═╝╚══════╝╚══════╝░░░╚═╝░░░╚═╝░░╚═╝░░╚═╝░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝╚══════╝╚═╝░░╚═╝
----------------------------------------------------
Programmer: Ebrahim Shafiei (EbraSha)
Email: Prof.Shafiei@Gmail.com
----------------------------------------------------

                ";
          
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            Console.Title = "Abdal FileWatcher " + version.Major + "." + version.Minor;

            Console.WriteLine(banner);

            // Get a list of all available drives
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                // Check if the drive is ready
                if (!drive.IsReady)
                {
                    Console.WriteLine($"Drive {drive.Name} is not ready. Skipping...");
                    continue;
                }

                // Create a FileSystemWatcher for each drive
                var watcher = new FileSystemWatcher
                {
                    Path = drive.Name,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true // Monitor subdirectories as well
                };

                watcher.Created += OnChanged;
                watcher.EnableRaisingEvents = true;

                watchers.Add(watcher);
            }

            Console.WriteLine("Monitoring all drives. Press 'q' to quit.");
            while (Console.Read() != 'q') ;

            // Dispose resources once monitoring is done
            foreach (var watcher in watchers)
            {
                watcher.Dispose();
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Change detected: {e.FullPath}"+"\n");

        }
    }
}
