using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearchSharp.Services.Services.Elastic;
using Moq;
using SharedDomain.DTOs;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Xunit;
using Elastic.Clients.Elasticsearch;

namespace ElasticSearchSharp.Services.Tests
{


    public class ElasticContextTests
    {
        private readonly Mock<IElasticContext> _mockElasticContext;

        public ElasticContextTests()
        {
            _mockElasticContext = new Mock<IElasticContext>();
        }

        [Fact]
        public async Task CreateIndexAsync_ShouldReturnOk()
        {
            // Arrange
            var indexName = "test_index";
            _mockElasticContext.Setup(x => x.CreateIndexAsync(indexName))
                .ReturnsAsync(ElasticResponse.Ok());

            // Act
            var result = await _mockElasticContext.Object.CreateIndexAsync(indexName);

            // Assert
            Assert.True(result.IsSucces);
        }

        [Fact]
        public async Task IndexDocumentAsync_ShouldReturnOk()
        {
            // Arrange
            var indexName = "test_index";
            var document = new TestDocument { Id = 1, Name = "Test Document" };
            _mockElasticContext.Setup(x => x.IndexDocumentAsync(indexName, document))
                .ReturnsAsync(ElasticResponse.Ok());

            // Act
            var result = await _mockElasticContext.Object.IndexDocumentAsync(indexName, document);

            // Assert
            Assert.True(result.IsSucces);
        }

        [Fact]
        public async Task GetDocumentAsync_ShouldReturnDocument()
        {
            // Arrange
            var indexName = "test_index";
            var documentId = "1";
            var document = new TestDocument { Id = 1, Name = "Test Document" };
            _mockElasticContext.Setup(x => x.GetDocumentAsync<TestDocument>(indexName, documentId))
                .ReturnsAsync(ElasticResponse<TestDocument>.Ok(document));

            // Act
            var result = await _mockElasticContext.Object.GetDocumentAsync<TestDocument>(indexName, documentId);

            // Assert
            Assert.True(result.IsSucces);
            Assert.Equal(document, result.Data);
        }

        [Fact]
        public async Task DeleteDocumentAsync_ShouldReturnOk()
        {
            // Arrange
            var indexName = "test_index";
            var documentId = "1";
            _mockElasticContext.Setup(x => x.DeleteDocumentAsync<TestDocument>(indexName, documentId))
                .ReturnsAsync(ElasticResponse.Ok());

            // Act
            var result = await _mockElasticContext.Object.DeleteDocumentAsync<TestDocument>(indexName, documentId);

            // Assert
            Assert.True(result.IsSucces);
        }

        [Fact]
        public async Task UpdateDocumentAsync_ShouldReturnOk()
        {
            // Arrange
            var indexName = "test_index";
            var documentId = "1";
            var partialDocument = new PartialDocument { Name = "Updated Document" };

            var request = new UpdateRequest<TestDocument, PartialDocument>(indexName, documentId)
            {
                Doc = partialDocument
            };

            _mockElasticContext.Setup(x => x.UpdateDocumentAsync<TestDocument, PartialDocument>(indexName, request))
                .ReturnsAsync(ElasticResponse.Ok());

            // Act
            var result = await _mockElasticContext.Object.UpdateDocumentAsync<TestDocument, PartialDocument>(indexName, request);

            // Assert
            Assert.True(result.IsSucces);
        }


        [Fact]
        public async Task BulkAsync_ShouldReturnOk()
        {
            // Arrange
            var indexName = "test_index";
            var documents = new List<TestDocument>
        {
            new TestDocument { Id = 1, Name = "Test Document 1" },
            new TestDocument { Id = 2, Name = "Test Document 2" }
        };
            _mockElasticContext.Setup(x => x.BulkAsync(indexName, documents))
                .ReturnsAsync(ElasticResponse.Ok());

            // Act
            var result = await _mockElasticContext.Object.BulkAsync(indexName, documents);

            // Assert
            Assert.True(result.IsSucces);
        }
    }

    public class TestDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PartialDocument
    {
        public string Name { get; set; }
    }

}
