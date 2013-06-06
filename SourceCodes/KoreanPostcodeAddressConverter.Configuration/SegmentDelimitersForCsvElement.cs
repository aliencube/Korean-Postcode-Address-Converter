using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the segment delimiters for CSV element.
    /// </summary>
    public class SegmentDelimitersForCsvElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the delimiters.
        /// </summary>
        [ConfigurationProperty("delimiters", DefaultValue = "|", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`~!@#$%^&*()-=_+[]{}\\:'\".<>/?", MinLength = 1)]
        public string Delimiters
        {
            get { return (string) this["delimiters"]; }
            set { this["delimiters"] = value; }
        }
    }
}