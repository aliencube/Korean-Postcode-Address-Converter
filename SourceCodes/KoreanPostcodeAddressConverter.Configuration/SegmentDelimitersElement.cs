using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the segment delimiters element entity.
    /// </summary>
    public class SegmentDelimitersElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the delimiters for directory.
        /// </summary>
        [ConfigurationProperty("forDirectory", DefaultValue = "\\", IsRequired = true)]
        public string ForDirectory
        {
            get { return (string)this["forDirectory"]; }
            set { this["forDirectory"] = value; }
        }

        /// <summary>
        /// Gets or sets the delimiters for file.
        /// </summary>
        [ConfigurationProperty("forFile", DefaultValue = "-_.", IsRequired = true)]
        public string ForFile
        {
            get { return (string)this["forFile"]; }
            set { this["forFile"] = value; }
        }

        /// <summary>
        /// Gets or sets the delimiters for CSV.
        /// </summary>
        [ConfigurationProperty("forCsv", DefaultValue = ",", IsRequired = true)]
        public string ForCsv
        {
            get { return (string)this["forCsv"]; }
            set { this["forCsv"] = value; }
        }
    }
}