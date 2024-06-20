namespace ElasticFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class IndexNameAttribute : Attribute
    {
        public string IndexName { get; }

        public IndexNameAttribute(string indexName)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
        }
    }
}
