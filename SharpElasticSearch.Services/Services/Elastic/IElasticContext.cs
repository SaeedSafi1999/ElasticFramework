using Nest;
using SharedDomain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchSharp.Services.Services.Elastic
{
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the Elasticsearch context.
    /// </summary>
    public interface IElasticContext
    {
        /// <summary>
        /// Creates an index asynchronously.
        /// </summary>
        /// <param name="indexName">The name of the index.</param>
        /// <returns>The response from Elasticsearch.</returns>
        Task<ElasticResponse> CreateIndexAsync(string indexName);

        /// <summary>
        /// Deletes an index asynchronously.
        /// </summary>
        /// <param name="indexName">The name of the index.</param>
        /// <returns>The response from Elasticsearch.</returns>
        Task<ElasticResponse> DeleteIndexAsync(string indexName);

        /// <summary>
        /// Indexes a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="document">The document to index.</param>
        /// <returns>The response from Elasticsearch.</returns>
        Task<ElasticResponse> IndexDocumentAsync<T>(string indexName, T document) where T : class;

        /// <summary>
        /// Gets a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="id">The ID of the document.</param>
        /// <returns>The document.</returns>
        Task<ElasticResponse<T>> GetDocumentAsync<T>(string indexName, string id) where T : class;

        /// <summary>
        /// Deletes a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="id">The ID of the document.</param>
        /// <returns>The response from Elasticsearch.</returns>
        Task<ElasticResponse> DeleteDocumentAsync<T>(string indexName, string id) where T : class;

        /// <summary>
        /// Updates a document asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="id">The ID of the document.</param>
        /// <param name="document">The document to update.</param>
        /// <returns>The response from Elasticsearch.</returns>
        Task<ElasticResponse> UpdateDocumentAsync<T>(string indexName, string id, T document) where T : class;

        /// <summary>
        /// Searches for documents asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the documents.</typeparam>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="query">The search query.</param>
        /// <returns>A collection of documents.</returns>
        Task<IEnumerable<T>> SearchAsync<T>(string indexName, Func<QueryContainerDescriptor<T>, QueryContainer> query) where T : class;
    }

}
