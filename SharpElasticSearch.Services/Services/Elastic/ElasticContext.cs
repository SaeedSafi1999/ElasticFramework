using SharedDomain.DTOs;
using Nest;
using SharedDomain.Attributes;
using SharedDomain.Configuration;
using System.Reflection;

namespace ElasticSearchSharp.Services.Services.Elastic;

/// <summary>
/// Abstract class for the Elasticsearch context.
/// </summary>
public abstract class ElasticContext : IElasticContext
{
    protected readonly IElasticClient Client;
    private readonly ElasticConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElasticContext"/> class with the specified configuration.
    /// </summary>
    /// <param name="config">The Elasticsearch configuration.</param>
    protected ElasticContext(ElasticConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        var settings = new ConnectionSettings(new Uri($"{_config.Url}:{_config.Port}"));

        if (!string.IsNullOrEmpty(_config.Username) && !string.IsNullOrEmpty(_config.Password))
        {
            settings = settings.BasicAuthentication(_config.Username, _config.Password);
        }

        Client = new ElasticClient(settings);
    }


    private string GetIndexName(MethodBase method, string parameterIndexName)
    {
        var attribute = method.GetCustomAttribute<IndexNameAttribute>();
        return attribute?.IndexName ?? parameterIndexName;
    }

    public virtual async Task<ElasticResponse> CreateIndexAsync(string indexName)
    {
        var response = await Client.Indices.CreateAsync(indexName.ToLower());
        return response.IsValid ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ServerError.ToString());
    }

    public virtual async Task<ElasticResponse> DeleteIndexAsync(string indexName)
    {
        var response = await Client.Indices.DeleteAsync(indexName.ToLower());
        return response.IsValid ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ServerError.ToString());
    }

    public virtual async Task<ElasticResponse> IndexDocumentAsync<T>(string indexName, T document) where T : class
    {
        var method = MethodBase.GetCurrentMethod();
        var resolvedIndexName = GetIndexName(method, indexName.ToLower());

        var response = await Client.IndexAsync(document, idx => idx.Index(resolvedIndexName));
        return response.IsValid ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ServerError.ToString());
    }

    public virtual async Task<ElasticResponse<T>> GetDocumentAsync<T>(string indexName, string id) where T : class
    {
        var method = MethodBase.GetCurrentMethod();
        var resolvedIndexName = GetIndexName(method, indexName);

        var response = await Client.GetAsync<T>(id, idx => idx.Index(resolvedIndexName));
        return response.Found ? ElasticResponse<T>.Ok(response.Source) : ElasticResponse<T>.Fail("nothing found");
    }

    public virtual async Task<ElasticResponse> DeleteDocumentAsync<T>(string indexName, string id) where T : class
    {
        var method = MethodBase.GetCurrentMethod();
        var resolvedIndexName = GetIndexName(method, indexName);

        var response = await Client.DeleteAsync<T>(id, idx => idx.Index(resolvedIndexName));
        return response.IsValid ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ServerError.ToString());
    }

    public virtual async Task<ElasticResponse> UpdateDocumentAsync<T>(string indexName, string id, T document) where T : class
    {
        var method = MethodBase.GetCurrentMethod();
        var resolvedIndexName = GetIndexName(method, indexName);

        var response = await Client.UpdateAsync<T>(id, u => u.Index(resolvedIndexName).Doc(document));
        return response.IsValid ? ElasticResponse.Ok() : ElasticResponse.Fail(response.ServerError.ToString());
    }

    public virtual async Task<IEnumerable<T>> SearchAsync<T>(string indexName, Func<QueryContainerDescriptor<T>, QueryContainer> query) where T : class
    {
        var method = MethodBase.GetCurrentMethod();
        var resolvedIndexName = GetIndexName(method, indexName);

        var response = await Client.SearchAsync<T>(s => s.Index(resolvedIndexName).Query(query));
        return response.Documents;
    }
}