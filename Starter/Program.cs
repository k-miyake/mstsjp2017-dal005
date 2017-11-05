using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Starter
{
     public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var num = args.Length == 0 ? 2 : int.Parse(args[0]);
            Console.WriteLine("キューを" + num + "件作成します");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json");

            Configuration = builder.Build();

            // Create Storage Account
            var storageAccount = CloudStorageAccount.Parse(Configuration["StorageConnectionString"]);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(Configuration["StarterQueue"]); 

            // Create a message and add it to the queue.
            int i = 0;
            while (i < num)
            {
                var eventLogs = Configuration["NumberOfLogMessages"];
                var message = new CloudQueueMessage(eventLogs);
                queue.AddMessageAsync(message).Wait();
                i++;
                Thread.Sleep(30000);
            }
        }
    }
}
