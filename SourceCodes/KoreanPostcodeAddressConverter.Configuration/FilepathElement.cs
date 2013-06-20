using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the filepath element entity.
    /// </summary>
    public class FilepathElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the filename for download or extract.
        /// </summary>
        [ConfigurationProperty("filepath", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`@#$^&*=[]{}|;'\",<>/?")]
        public string Filepath
        {
            get { return (string)this["filepath"]; }
            set { this["filepath"] = value; }
        }
    }
}