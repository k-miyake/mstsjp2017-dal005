using System;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventSender
{
    public static class Sender
    {
        // Event Hubs接続情報
        private static EventHubClient _eventHubClient;
        private static readonly string EhConnectionString = ConfigurationManager.AppSettings["EhConnectionString"];
        private static readonly string EhEntityPath = ConfigurationManager.AppSettings["EhEntityPath"];

        [FunctionName("Sender")]
        public static void Run([QueueTrigger("demo3", Connection = "AzureWebJobsStorage")]string starterItem, ILogger logger)
        {
            logger.LogInformation($"C# Queue trigger function processed: {starterItem}");
            MainAsync(starterItem, logger).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string reqNum, ILogger logger)
        {
            int.TryParse(reqNum, out var req);

            // Event Hubクライアント設定
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };
            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            // メッセージ送信処理の実行
            await SendMessagesToEventHub(req, logger);

            // Event Hubクライアントの終了
            await _eventHubClient.CloseAsync();
        }

        // メッセージ送信処理
        private static async Task SendMessagesToEventHub(int numMessagesToSend, ILogger logger)
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

                    logger.LogInformation($"Sending message: {i}件目メッセージを送信します");
                    await _eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));
                }
                catch (Exception exception)
                {
                    logger.LogError($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            logger.LogInformation($"{numMessagesToSend} messages sent.");
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
