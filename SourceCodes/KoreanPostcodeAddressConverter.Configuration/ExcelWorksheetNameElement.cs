using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the Excel worksheet name element entity.
    /// </summary>
    public class ExcelWorksheetNameElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the worksheet name.
        /// </summary>
        [ConfigurationProperty("worksheet", DefaultValue = "Sheet1", IsRequired = true)]
        public string Worksheet
        {
            get { return (string)this["worksheet"]; }
            set { this["worksheet"] = value; }
        }
    }
}