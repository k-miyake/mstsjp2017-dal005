using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Starter
{
     public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            int num = 0;
            if (args.Length == 0)
            {
                num = 2;
            }
            else
            {
                num = int.Parse(args[0]);
            }
            Console.WriteLine("キューを" + num + "件作成します");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json");

            Configuration = builder.Build();

            // Create Storage Account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("starter-items");

            // Create a message and add it to the queue.
            int i = 0;
            while (i < num)
            {
                var eventLogs = "10000";
                CloudQueueMessage message = new CloudQueueMessage(eventLogs);
                queue.AddMessageAsync(message).Wait();
                i++;
            }
        }
    }
}
