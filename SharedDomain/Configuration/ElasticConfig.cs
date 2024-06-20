using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.Configuration
{
    /// <summary>
    /// Configuration settings for connecting to Elasticsearch.
    /// </summary>
    public class ElasticConfig
    {
        /// <summary>
        /// Gets or sets the URL of the Elasticsearch server.
        /// </summary>
        public string Url { get; set; } = "http://localhost";

        /// <summary>
        /// Gets or sets the port number of the Elasticsearch server.
        /// </summary>
        public int Port { get; set; } = 9200;

        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// APIKEY for connecting to cloud
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// name of assemblies that inheritance from IElasticContext
        /// </summary>
        public IEnumerable<Assembly> assemblies { get; set; }
    }

}
