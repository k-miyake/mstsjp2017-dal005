using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TodoMonitor
{
    public static class TodoFeed
    {
        [FunctionName("TodoFeed")]
        public static void Run([CosmosDBTrigger("demo2", "items", ConnectionStringSetting = "ManualDocumentDBConnectionString",
                LeaseCollectionName = "leases", LeaseDatabaseName = "demo2")]IReadOnlyList<Document> changeList, TraceWriter log)
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
