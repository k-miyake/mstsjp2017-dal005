using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ChangeFeed
{
    public static class EventFeed
    {
        [FunctionName("EventFeed")]
        public static void Run([CosmosDBTrigger("msts2017", "eventlogs", ConnectionStringSetting = "AzureWebJobsDocumentDBConnectionString",
                LeaseCollectionName = "leases", LeaseDatabaseName = "msts2017")]IReadOnlyList<Document> changeList, ILogger logger)
        {
            if (changeList != null && changeList.Count > 0)
            {
                logger.LogInformation("Documents modified " + changeList.Count);
                var i = 0;

                foreach (var change in changeList)
                {
                    logger.LogInformation("document Id of $i: " + change.Id);
                    logger.LogInformation(change.ToString());
                    i++;
                }
            }
        }
    }
}
