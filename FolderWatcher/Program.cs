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
        static void Main(string[] args)
        {
            Watcher watcher = new Watcher();
            watcher.Run();
        }


    }
}
