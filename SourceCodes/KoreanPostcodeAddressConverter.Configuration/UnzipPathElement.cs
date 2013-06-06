using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the unzip path element entity.
    /// </summary>
    public class UnzipPathElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the unzip file path for command line process.
        /// </summary>
        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`@#$^&*=[]{}|;'\",<>/?")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }
}