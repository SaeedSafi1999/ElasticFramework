using SharedDomain.DTOs;
using SharedDomain.Attributes;
using SharedDomain.Configuration;
using System.Reflection;
using Elasticsearch.Net;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Elastic.Transport;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using Elastic.Transport.Products.Elasticsearch;

namespace ElasticSearchSharp.Services.Services.Elastic
{
    /// <summary>
    /// Abstract class for the Elasticsearch context.
    /// </summary>
    public abstract class ElasticContext : IElasticContext
    {
        private readonly ElasticsearchClient Client;
        private readonly ElasticConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticContext"/> class with the specified configuration.
        /// </summary>
        /// <param name="config">The Elasticsearch configuration.</param>
        public ElasticContext(ElasticConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            var settings = new ElasticsearchClientSettings(new Uri($"{_config.Url}:{_config.Port}"));

            if (!string.IsNullOrEmpty(_config.ApiKey))
            {
                // Connect to cloud using API Key
                settings = settings.Authentication(new ApiKey(_config.ApiKey));
            }
            else if (!string.IsNullOrEmpty(_config.Username) && !string.IsNullOrEmpty(_config.Password))
            {
                // Connect to local using Basic Authentication
                settings = settings.Authentication(new BasicAuthentication(_config.Username, _config.Password));
            }

            Client = new ElasticsearchClient(settings);
        }

        private string GetIndexName(MethodBase method, string parameterIndexName)
        {
            var attribute = method.GetCustomAttribute<IndexNameAttribute>();
            return attribute?.IndexName ?? parameterIndexName;
        }

        public virtual async Task<ElasticResponse> CreateIndexAsync(string indexName)
        {
            var response = await Client.Indices.CreateAsync(indexName.ToLower());
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> DeleteIndexAsync(string indexName)
        {
            var response = await Client.Indices.DeleteAsync(indexName.ToLower());
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> IndexDocumentAsync<T>(string indexName, T document) where T : class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName.ToLower());

            var response = await Client.IndexAsync(document, idx => idx.Index(resolvedIndexName));
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse<T>> GetDocumentAsync<T>(string indexName, string id) where T : class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName);

            var response = await Client.GetAsync<T>(id, idx => idx.Index(resolvedIndexName));
            return response.Found ? ElasticResponse<T>.Ok(response.Source) : ElasticResponse<T>.Fail("Document not found");
        }

        public virtual async Task<ElasticResponse> DeleteDocumentAsync<T>(string indexName, string id) where T : class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName);

            var response = await Client.DeleteAsync<T>(id, idx => idx.Index(resolvedIndexName));
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> UpdateDocumentAsync<TDocument,TPartialDocument>(string indexName,UpdateRequest<TDocument,TPartialDocument> request) where TDocument : class where TPartialDocument:class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName);

            var response = await Client.UpdateAsync<TDocument, TPartialDocument>(request);
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> IndexRangeDocumentAsync<TDocument>(string indexName,IEnumerable<TDocument> documents) where TDocument : class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName);
            List<string> errors = new();
            foreach (var document in documents)
            {
                var response = await Client.IndexAsync(document, idx => idx.Index(resolvedIndexName));
                if (!response.IsValidResponse)
                    errors.Add(response.ElasticsearchServerError.Error.Reason);
            }
            if (errors.Count() > 0)
            {
                string errorMessages = string.Empty;
                foreach (var error in errors)
                {
                    errorMessages.Concat($"{error} \n");
                }
                return ElasticResponse.Fail($"Insert range has Error \n Errors: {errorMessages}");
            }
            return ElasticResponse.Ok();
        }

        public virtual async Task<ElasticResponse> DeleteIndexRangeDocumentAsync<TDocument>(string indexName, IEnumerable<string> ids) where TDocument : class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName);
            List<string> errors = new();
            foreach (var id in ids)
            {
                var response = await Client.DeleteAsync<TDocument>(id, idx => idx.Index(resolvedIndexName));
                if (!response.IsValidResponse)
                    errors.Add(response.ElasticsearchServerError.Error.Reason);
            }
            if (errors.Count() > 0)
            {
                string errorMessages = string.Empty;
                foreach (var error in errors)
                {
                    errorMessages.Concat($"{error} \n");
                }
                return ElasticResponse.Fail($"Insert range has Error \n Errors: {errorMessages}");
            }
            return ElasticResponse.Ok();
        }



        public virtual async Task<IEnumerable<T>> SearchAsync<T>(string indexName, QueryDescriptor<T> query) where T : class
        {
            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName);

            var response = await Client.SearchAsync<T>(s => s.Index(resolvedIndexName).Query(query));
            return response.Documents;
        }

        // Additional Methods from ElasticsearchClient

        public virtual async Task<ElasticResponse> BulkAsync<T>(string indexName, IEnumerable<T> documents) where T : class
        {
            if (string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentNullException(nameof(indexName));
            }

            if (documents == null || !documents.Any())
            {
                throw new ArgumentNullException(nameof(documents));
            }

            var method = MethodBase.GetCurrentMethod();
            var resolvedIndexName = GetIndexName(method, indexName.ToLower());

            var bulkRequest = new BulkRequest(resolvedIndexName)
            {
                Operations = new List<IBulkOperation>()
            };

            foreach (var doc in documents)
            {
                bulkRequest.Operations.Add(new BulkIndexOperation<T>(doc));
            }

            var response = await Client.BulkAsync(bulkRequest);
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }


        public virtual async Task<ElasticResponse> RefreshIndexAsync(string indexName)
        {
            var resolvedIndexName = GetIndexName(MethodBase.GetCurrentMethod(), indexName);
            var response = await Client.Indices.RefreshAsync(resolvedIndexName);
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.ToString());
        }

        public virtual async Task<ElasticResponse> ReindexAsync(ReindexRequest request)
        {
            var response = await Client.ReindexAsync(request);
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> AliasAsync(string aliasName, string indexName)
        {
            var response = await Client.Indices.PutAliasAsync(indexName, aliasName);
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> RemoveAliasAsync(string aliasName, string indexName)
        {
            var response = await Client.Indices.DeleteAliasAsync(indexName, aliasName);
            return response.IsValidResponse ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ElasticsearchServerError.Error.Reason);
        }

        public virtual async Task<ElasticResponse> IndexExistsAsync(string indexName)
        {
            var response = await Client.Indices.ExistsAsync(indexName);
            return response.Exists ? ElasticResponse.Ok() : ElasticResponse.Fail("Index does not exist");
        }

    }
}
