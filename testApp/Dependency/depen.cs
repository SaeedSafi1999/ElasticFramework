using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticFramework.Attributes;
using ElasticFramework.Configuration;
using ElasticFramework.DTOs;
using ElasticFramework.Services.Elastic;

namespace testApp.Dependency
{

    //Entity//////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Represents a product document in Elasticsearch.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the ID of the product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        public string Category { get; set; }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////



    //EntityContext//////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Context class for interacting with Elasticsearch for product-related operations.
    /// </summary>
    public class ProductContext : ElasticContext
    {
        public ProductContext(ElasticConfig config) : base(config) { }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////




    //interface of entity repository////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Interface for product repository.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Searches for products by category asynchronously.
        /// </summary>
        /// <param name="category">The category to search by.</param>
        /// <returns>A collection of products.</returns>
        Task<IEnumerable<Product>> SearchByCategoryAsync(string category);


        Task<ElasticResponse> Insert(Product product);
        /// <summary>
        /// Searches for products by price range asynchronously.
        /// </summary>
        /// <param name="minPrice">The minimum price.</param>
        /// <param name="maxPrice">The maximum price.</param>
        /// <returns>A collection of products.</returns>
        Task<IEnumerable<Product>> SearchByPriceRangeAsync(double minPrice, double maxPrice);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////



    // implement of EntityRepository//////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Repository class for managing product documents in Elasticsearch.
    /// </summary>
    public class ProductRepository : IProductRepository//or wha ever you called that
    {
        private readonly IElasticContext _context;


        public ProductRepository(IElasticContext context)
        {
            _context = context;
        }


        [IndexName("products")]
        public async Task CreateIndexAsync(string indexName)
        {
            await _context.CreateIndexAsync(indexName);
        }

        [IndexName("products")]
        public async Task<ElasticResponse> Insert(Product product)
        {
            return await _context.IndexDocumentAsync("products", product);
        }

        [IndexName("products")]
        public virtual async Task<IEnumerable<Product>> SearchByCategoryAsync(string category)
        {
            var query = new QueryDescriptor<Product>()
            .Match(m => m.Field(f => f.Category).Query(category));

            var results = await _context.SearchAsync<Product>(null, query);

            return results;
        }

        [IndexName("products")]
        public virtual async Task<IEnumerable<Product>> SearchByPriceRangeAsync(double minPrice, double maxPrice)
        {
            return await _context.SearchAsync<Product>(null, new QueryDescriptor<Product>().Range(r => r.NumberRange(z => z.Field(f => f.Price).Gte(minPrice).Field(z => z.Price).Lte(maxPrice))));
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////


}
