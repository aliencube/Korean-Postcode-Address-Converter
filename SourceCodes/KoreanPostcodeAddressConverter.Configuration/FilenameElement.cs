using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the filename element entity.
    /// </summary>
    public class FilenameElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the filename for download or extract.
        /// </summary>
        [ConfigurationProperty("filename", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`@#$^&*=[]{}\\|;:'\",<>/?")]
        public string Filename
        {
            get { return (string)this["filename"]; }
            set { this["filename"] = value; }
        }
    }
}