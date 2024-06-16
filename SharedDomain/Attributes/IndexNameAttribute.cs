using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.Attributes
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
