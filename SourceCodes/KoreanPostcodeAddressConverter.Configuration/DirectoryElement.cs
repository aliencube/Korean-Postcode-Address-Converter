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
        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "`@#$^&*=[]{}|;'\",<>?")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}