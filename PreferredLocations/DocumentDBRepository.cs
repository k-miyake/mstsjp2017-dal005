using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Linq.Expressions;
using Microsoft.Azure.Documents.Linq;

namespace PreferredLocations
{
    public class DocumentDBRepository<T> where T : class
    {
        private static AppOption _option;
        private static DocumentClient _client;

        public DocumentDBRepository(AppOption option)
        {
            _option = option;
            _client = new DocumentClient(new Uri(_option.Endpoint), _option.Key);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_option.DatabaseId, _option.CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<T> GetItemAsync(string id)
        {
            Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_option.DatabaseId, _option.CollectionId, id));
            return (T)(dynamic)document;
        }
    }
}
