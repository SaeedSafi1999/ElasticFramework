using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using ElasticFramework.DTOs;

namespace ElasticFramework.Services.Elastic
{
    /// <summary>
    /// Interface for the Elasticsearch context.
    /// </summary>
    public interface IElasticContext
    {
        /// <summary>
        /// Creates an index asynchronously.
        /// </summary>
        /// <param name="indexName">The name of the index to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> CreateIndexAsync(string indexName);

        /// <summary>
        /// Deletes an index asynchronously.
        /// </summary>
        /// <param name="indexName">The name of the index to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> DeleteIndexAsync(string indexName);

        /// <summary>
        /// Indexes a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="document">The document to index.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> IndexDocumentAsync<T>(string indexName, T document) where T : class;

        /// <summary>
        /// Gets a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="id">The ID of the document.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse<T>> GetDocumentAsync<T>(string indexName, string id) where T : class;

        /// <summary>
        /// Deletes a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="id">The ID of the document.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> DeleteDocumentAsync<T>(string indexName, string id) where T : class;

        /// <summary>
        /// Updates a document asynchronously.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TPartialDocument">The type of the partial document used for updating.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="request">The update request containing the update details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> UpdateDocumentAsync<TDocument, TPartialDocument>(string indexName, UpdateRequest<TDocument, TPartialDocument> request)
            where TDocument : class
            where TPartialDocument : class;

        /// <summary>
        /// Searches documents asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the documents.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="query">The query used for searching.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of found documents.</returns>
        Task<IEnumerable<T>> SearchAsync<T>(string indexName, QueryDescriptor<T> query) where T : class;

        /// <summary>
        /// Performs a bulk operation asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the documents.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="documents">The documents to be included in the bulk operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> BulkAsync<T>(string indexName, IEnumerable<T> documents) where T : class;

        /// <summary>
        /// Refreshes an index asynchronously.
        /// </summary>
        /// <param name="indexName">The name of the index to refresh.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> RefreshIndexAsync(string indexName);

        /// <summary>
        /// Reindexes documents from one index to another asynchronously.
        /// </summary>
        /// <param name="request">The reindex request containing the reindex details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> ReindexAsync(ReindexRequest request);

        /// <summary>
        /// Adds an alias to an index asynchronously.
        /// </summary>
        /// <param name="aliasName">The name of the alias.</param>
        /// <param name="indexName">The name of the index.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> AliasAsync(string aliasName, string indexName);

        /// <summary>
        /// Removes an alias from an index asynchronously.
        /// </summary>
        /// <param name="aliasName">The name of the alias.</param>
        /// <param name="indexName">The name of the index.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> RemoveAliasAsync(string aliasName, string indexName);

        /// <summary>
        /// Checks if an index exists asynchronously.
        /// </summary>
        /// <param name="indexName">The name of the index.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> IndexExistsAsync(string indexName);

        /// <summary>
        /// Indexes a range of documents asynchronously.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="documents">The documents to index.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> IndexRangeDocumentAsync<TDocument>(string indexName, IEnumerable<TDocument> documents) where TDocument : class;

        /// <summary>
        /// Deletes a range of documents asynchronously.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="ids">The IDs of the documents to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation.</returns>
        Task<ElasticResponse> DeleteIndexRangeDocumentAsync<TDocument>(string indexName, IEnumerable<string> ids) where TDocument : class;
    }
}
