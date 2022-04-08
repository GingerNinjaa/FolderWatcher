using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace FolderWatcher
{
    class Program
    {
        public static Db_Connector connector { get; set; }
        static void Main(string[] args)
        {
            connector = new Db_Connector();
            Console.Write("Set folder path => ");
            string path = Console.ReadLine();

            using var watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

           //watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            //  watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Już działm");
            Console.ReadLine();

            Console.WriteLine("---------------------------");
            connector.ReadData();
            Console.WriteLine("---------------------------");


        }


        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            connector.InsertData($"Changed", e.FullPath, DateTime.UtcNow.ToString());
            Console.WriteLine($"Changed: {e.FullPath}   " + DateTime.UtcNow);
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string value = $"Created: {e.FullPath}  " + DateTime.UtcNow;
            connector.InsertData($"Created", e.FullPath, DateTime.UtcNow.ToString());
            Console.WriteLine(value);
            Console.ResetColor();
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            connector.InsertData($"Deleted", e.FullPath, DateTime.UtcNow.ToString());
            Console.WriteLine($"Deleted: {e.FullPath}   " + DateTime.UtcNow);
            Console.ResetColor();
        }


        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            Console.ResetColor();
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}   " + DateTime.UtcNow);
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }


    }
}
