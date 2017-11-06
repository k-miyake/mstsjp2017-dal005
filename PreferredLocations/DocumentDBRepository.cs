using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Linq.Expressions;
using Microsoft.Azure.Documents.Linq;
using Microsoft.IdentityModel.Protocols;

namespace PreferredLocations
{
    public class DocumentDBRepository<T> where T : class
    {
        private static AppOption _option;
        private static DocumentClient _client;

        public DocumentDBRepository(AppOption option)
        {
            _option = option;

            // 接続ポリシーの作成
            ConnectionPolicy cp = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
#if RELEASE
                ConnectionProtocol = Protocol.Tcp
#endif
            };

            // このインスタンスのリージョンを追加
            cp.PreferredLocations.Add(_option.AppRegion);

            // その他のリージョンを追加
            cp = AddPreferredLocations(cp).Result;
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

        // 利用可能リージョンを追加
        private static async Task<ConnectionPolicy> AddPreferredLocations(ConnectionPolicy cp)
        {
            var preClient = new DocumentClient(new Uri(_option.Endpoint), _option.Key);
            DatabaseAccount db = await preClient.GetDatabaseAccountAsync();
            var locations = db.ReadableLocations;

            foreach (var l in locations)
            {
                if (l.Name != _option.AppRegion)
                {
                    cp.PreferredLocations.Add(l.Name);
                }
            }
            preClient.Dispose();
            return cp;
        }
    }
}
