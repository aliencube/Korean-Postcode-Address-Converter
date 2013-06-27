using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the proxy server settings element entity.
    /// </summary>
    public class ProxyServerElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the value that specifies whether to use proxy server or not.
        /// </summary>
        [ConfigurationProperty("use", DefaultValue = false, IsRequired = true)]
        public bool Use
        {
            get { return (bool)this["use"]; }
            set { this["use"] = value; }
        }

        /// <summary>
        /// Gets or sets the proxy server host.
        /// </summary>
        [ConfigurationProperty("host", IsRequired = false)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }

        /// <summary>
        /// Gets or sets the proxy server port number.
        /// </summary>
        [ConfigurationProperty("port", IsRequired = false)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }
    }
}