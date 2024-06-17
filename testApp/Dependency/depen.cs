using ElasticSearchSharp.Services.Services.Elastic;
using SharedDomain.Attributes;
using SharedDomain.Configuration;

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
    //public class ProductContext : ElasticContext
    //{
    //    public ProductContext(ElasticConfig config) : base(config){ }
    //}
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

        /// <summary>
        /// Searches for products by price range asynchronously.
        /// </summary>
        /// <param name="minPrice">The minimum price.</param>
        /// <param name="maxPrice">The maximum price.</param>
        /// <returns>A collection of products.</returns>
        Task<IEnumerable<Product>> SearchByPriceRangeAsync(double minPrice, double maxPrice);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////



    // implement of EntityRepository/////////////////////////////////////////////////////////////////////////////////////
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

        public async Task CreateIndexAsync(string indexName)
        {
            await _context.CreateIndexAsync(indexName);
        }

        [IndexName("products")]
        public virtual async Task<IEnumerable<Product>> SearchByCategoryAsync(string category)
        {
            var res = await _context.SearchAsync<Product>(null, q => q.Term(t => t.Field(f => f.Category).Value(category)));
            return res;
        }

        public virtual async Task<IEnumerable<Product>> SearchByPriceRangeAsync(double minPrice, double maxPrice)
        {
            return await _context.SearchAsync<Product>(null, q => q.Range(r => r.Field(f => f.Price).GreaterThanOrEquals(minPrice).LessThanOrEquals(maxPrice)));
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////


}
