using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the filename mapping element entity.
    /// </summary>
    public class FilenameMappingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the filename to search.
        /// </summary>
        [ConfigurationProperty("search", DefaultValue = "", IsRequired = true)]
        public string Search
        {
            get { return (string)this["search"]; }
            set { this["search"] = value; }
        }

        /// <summary>
        /// Gets or sets the filename to be replaced.
        /// </summary>
        [ConfigurationProperty("replace", DefaultValue = "", IsRequired = true)]
        public string Replace
        {
            get { return (string)this["replace"]; }
            set { this["replace"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether this file to be converted or not.
        /// </summary>
        [ConfigurationProperty("conversion", DefaultValue = false, IsRequired = false)]
        public bool Conversion
        {
            get { return (bool)this["conversion"]; }
            set { this["conversion"] = value; }
        }
    }
}