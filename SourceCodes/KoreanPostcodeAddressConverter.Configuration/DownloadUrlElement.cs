using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the download URL element entity.
    /// </summary>
    public class DownloadUrlElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the URL for download.
        /// </summary>
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`@$^*[]{}|'\"<>")]
        public string Url
        {
            get { return (string) this["url"]; }
            set { this["url"] = value; }
        }
    }
}