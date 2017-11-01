using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace EventSender
{
    public static class Sender
    {
        // Event Hubs接続情報
        private static EventHubClient eventHubClient;
        private static readonly string EhConnectionString = ConfigurationManager.AppSettings["EhConnectionString"];
        private static readonly string EhEntityPath = ConfigurationManager.AppSettings["EhEntityPath"];

        [FunctionName("Sender")]
        public static void Run([QueueTrigger("starter-items", Connection = "AzureWebJobsStorage")]string starterItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {starterItem}");
            MainAsync(starterItem, log).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string reqNum, TraceWriter log)
        {
            int req = 0;
            int.TryParse(reqNum, out req);

            // Event Hubクライアント設定
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            // メッセージ送信処理の実行
            await SendMessagesToEventHub(req, log);

            // Event Hubクライアントの終了
            await eventHubClient.CloseAsync();
        }

        // メッセージ送信処理
        private static async Task SendMessagesToEventHub(int numMessagesToSend, TraceWriter log)
        {
            var random = new Random();

            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    // メッセージ作成
                    var message = new DeviceInfo
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Value = random.Next(10),
                        CreateOn = DateTimeOffset.UtcNow.ToString("yyyy/MM/ddTHH:mm:ss.fffffffzzz"),
                    };

                    log.Info($"Sending message: {i}件目メッセージを送信します");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));
                }
                catch (Exception exception)
                {
                    log.Error($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            log.Info($"{numMessagesToSend} messages sent.");
        }
    }

    // メッセージ送信用クラス
    public class DeviceInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "createOn")]
        public string CreateOn { get; set; }
    }
}
