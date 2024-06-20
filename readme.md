if you used  Repository design pattern in your projects you can easily use this package because I use this pattern to provide this package, and all of designs like entityframework.
# How to use ElasticFramework package

   download it from nuget

    Install-Package ElasticFramework
    
 set this code to your program.cs

    //add Assemblies tha inheritance from IElasticContext
	List<Assembly> assemblies = new();
	assemblies.Add(Assembly.GetEntryAssembly());//add more if you need
	//add ElasticFramework
	builder.Services.AddElasticFramework(op =>
	{
    op.Password = "your-password";//string
    op.Port = your-port;//int
    op.Url = "http://localhost";
    op.Username = "your-username";
    op.assemblies = assemblies.ToArray();
	});

# Method Detail

This document provides detailed explanations for each method in the `IElasticContext` interface, which offers various functionalities for interacting with an Elasticsearch instance.

## Interface: `IElasticContext`

### Methods

#### `Task<ElasticResponse> CreateIndexAsync(string indexName)`

Creates an index asynchronously.

- **Parameters**:
  - `indexName`: The name of the index to create.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> DeleteIndexAsync(string indexName)`

Deletes an index asynchronously.

- **Parameters**:
  - `indexName`: The name of the index to delete.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> IndexDocumentAsync<T>(string indexName, T document) where T : class`

Indexes a document asynchronously.

- **Type Parameters**:
  - `T`: The type of the document.
- **Parameters**:
  - `indexName`: The name of the index.
  - `document`: The document to index.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse<T>> GetDocumentAsync<T>(string indexName, string id) where T : class`

Gets a document asynchronously.

- **Type Parameters**:
  - `T`: The type of the document.
- **Parameters**:
  - `indexName`: The name of the index.
  - `id`: The ID of the document.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> DeleteDocumentAsync<T>(string indexName, string id) where T : class`

Deletes a document asynchronously.

- **Type Parameters**:
  - `T`: The type of the document.
- **Parameters**:
  - `indexName`: The name of the index.
  - `id`: The ID of the document.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> UpdateDocumentAsync<TDocument, TPartialDocument>(string indexName, UpdateRequest<TDocument, TPartialDocument> request) where TDocument : class where TPartialDocument : class`

Updates a document asynchronously.

- **Type Parameters**:
  - `TDocument`: The type of the document.
  - `TPartialDocument`: The type of the partial document used for updating.
- **Parameters**:
  - `indexName`: The name of the index.
  - `request`: The update request containing the update details.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<IEnumerable<T>> SearchAsync<T>(string indexName, QueryDescriptor<T> query) where T : class`

Searches documents asynchronously.

- **Type Parameters**:
  - `T`: The type of the documents.
- **Parameters**:
  - `indexName`: The name of the index.
  - `query`: The query used for searching.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the list of found documents.

#### `Task<ElasticResponse> BulkAsync<T>(string indexName, IEnumerable<T> documents) where T : class`

Performs a bulk operation asynchronously.

- **Type Parameters**:
  - `T`: The type of the documents.
- **Parameters**:
  - `indexName`: The name of the index.
  - `documents`: The documents to be included in the bulk operation.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> RefreshIndexAsync(string indexName)`

Refreshes an index asynchronously.

- **Parameters**:
  - `indexName`: The name of the index to refresh.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> ReindexAsync(ReindexRequest request)`

Reindexes documents from one index to another asynchronously.

- **Parameters**:
  - `request`: The reindex request containing the reindex details.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> AliasAsync(string aliasName, string indexName)`

Adds an alias to an index asynchronously.

- **Parameters**:
  - `aliasName`: The name of the alias.
  - `indexName`: The name of the index.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> RemoveAliasAsync(string aliasName, string indexName)`

Removes an alias from an index asynchronously.

- **Parameters**:
  - `aliasName`: The name of the alias.
  - `indexName`: The name of the index.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> IndexExistsAsync(string indexName)`

Checks if an index exists asynchronously.

- **Parameters**:
  - `indexName`: The name of the index.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> IndexRangeDocumentAsync<TDocument>(string indexName, IEnumerable<TDocument> documents) where TDocument : class`

Indexes a range of documents asynchronously.

- **Type Parameters**:
  - `TDocument`: The type of the documents.
- **Parameters**:
  - `indexName`: The name of the index.
  - `documents`: The documents to index.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

#### `Task<ElasticResponse> DeleteIndexRangeDocumentAsync<TDocument>(string indexName, IEnumerable<string> ids) where TDocument : class`

Deletes a range of documents asynchronously.

- **Type Parameters**:
  - `TDocument`: The type of the documents.
- **Parameters**:
  - `indexName`: The name of the index.
  - `ids`: The IDs of the documents to delete.
- **Returns**: 
  - A task that represents the asynchronous operation. The task result contains the response of the operation.

 
