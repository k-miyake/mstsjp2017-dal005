using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ChangeFeed
{
    public static class EventFeed
    {
        [FunctionName("EventFeed")]
        public static void Run([CosmosDBTrigger("msts2017", "eventlogs", ConnectionStringSetting = "AzureWebJobsDocumentDBConnectionString",
                LeaseCollectionName = "leases", LeaseDatabaseName = "msts2017")]IReadOnlyList<Document> changeList, TraceWriter log)
        {
            if (changeList != null && changeList.Count > 0)
            {
                log.Verbose("Documents modified " + changeList.Count);
                var i = 0;

                foreach (var change in changeList)
                {
                    log.Verbose("document Id of $i: " + change.Id);
                    log.Verbose(change.ToString());
                    i++;
                }
            }
        }
    }
}
