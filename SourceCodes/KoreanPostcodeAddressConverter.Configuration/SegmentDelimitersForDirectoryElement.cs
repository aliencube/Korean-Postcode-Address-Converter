using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the segment delimiters for directory element.
    /// </summary>
    public class SegmentDelimitersForDirectoryElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the delimiters.
        /// </summary>
        [ConfigurationProperty("delimiters", DefaultValue = "\\", IsRequired = true)]
        public string Delimiters
        {
            get { return (string) this["delimiters"]; }
            set { this["delimiters"] = value; }
        }
    }
}