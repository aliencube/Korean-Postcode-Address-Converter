using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the key/value pair element entity.
    /// </summary>
    public class KeyValuePairElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [ConfigurationProperty("key", DefaultValue = "", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether this is a default key/value pair or not.
        /// </summary>
        [ConfigurationProperty("default", DefaultValue = false, IsRequired = false)]
        public bool Default
        {
            get { return (bool)this["default"]; }
            set { this["default"] = value; }
        }
    }
}