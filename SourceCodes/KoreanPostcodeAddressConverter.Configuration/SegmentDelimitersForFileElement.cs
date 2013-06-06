using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the segment delimiters for file element.
    /// </summary>
    public class SegmentDelimitersForFileElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the delimiters.
        /// </summary>
        [ConfigurationProperty("delimiters", DefaultValue = "-_.", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`~!@#$%^&*()=+[]{}\\|;:'\",<>/?", MinLength = 1)]
        public string Delimiters
        {
            get { return (string) this["delimiters"]; }
            set { this["delimiters"] = value; }
        }
    }
}