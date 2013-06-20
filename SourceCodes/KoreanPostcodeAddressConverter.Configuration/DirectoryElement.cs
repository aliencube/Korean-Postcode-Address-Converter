using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the directory element entity.
    /// </summary>
    public class DirectoryElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the directory name.
        /// </summary>
        [ConfigurationProperty("directory", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`@#$^&*=[]{}|;'\",<>?")]
        public string Directory
        {
            get { return (string)this["directory"]; }
            set { this["directory"] = value; }
        }
    }
}