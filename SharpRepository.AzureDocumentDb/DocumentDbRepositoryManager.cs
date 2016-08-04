using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SharpRepository.AzureDocumentDb
{
    public static class DocumentDbRepositoryManager
    {
        public async static Task<Database> CreateDatabase(DocumentClient client, string databaseId)
        {
            return await client.CreateDatabaseAsync(new Database { Id = databaseId });
        }

        public async static Task<DocumentCollection> CreateCollection(DocumentClient client, Database database, string collectionId)
        {
            return await client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionId });
        }

        public async static void DropDatabase(DocumentClient client, Database database)
        {
            await client.DeleteDatabaseAsync(database.SelfLink);
        }
    }
}