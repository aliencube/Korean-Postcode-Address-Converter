using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the address markers element entity.
    /// </summary>
    public class LocationMarkersElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the province markers.
        /// </summary>
        [ConfigurationProperty("province", IsRequired = true)]
        public string Province
        {
            get { return (string)this["province"]; }
            set { this["province"] = value; }
        }

        /// <summary>
        /// Gets or sets the county markers.
        /// </summary>
        [ConfigurationProperty("county", IsRequired = true)]
        public string County
        {
            get { return (string)this["county"]; }
            set { this["county"] = value; }
        }

        /// <summary>
        /// Gets or sets the district markers.
        /// </summary>
        [ConfigurationProperty("district", IsRequired = true)]
        public string District
        {
            get { return (string)this["district"]; }
            set { this["district"] = value; }
        }

        /// <summary>
        /// Gets or sets the suburb markers.
        /// </summary>
        [ConfigurationProperty("suburb", IsRequired = true)]
        public string Suburb
        {
            get { return (string)this["suburb"]; }
            set { this["suburb"] = value; }
        }

        /// <summary>
        /// Gets or sets the village markers.
        /// </summary>
        [ConfigurationProperty("village", IsRequired = true)]
        public string Village
        {
            get { return (string)this["village"]; }
            set { this["village"] = value; }
        }

        /// <summary>
        /// Gets or sets the island markers.
        /// </summary>
        [ConfigurationProperty("island", IsRequired = true)]
        public string Island
        {
            get { return (string)this["island"]; }
            set { this["island"] = value; }
        }

        /// <summary>
        /// Gets or sets the san markers.
        /// </summary>
        [ConfigurationProperty("san", IsRequired = true)]
        public string San
        {
            get { return (string)this["san"]; }
            set { this["san"] = value; }
        }

        /// <summary>
        /// Gets or sets the street markers.
        /// </summary>
        [ConfigurationProperty("street", IsRequired = true)]
        public string Street
        {
            get { return (string)this["street"]; }
            set { this["street"] = value; }
        }

        /// <summary>
        /// Gets or sets the street name corrections collection.
        /// </summary>
        [ConfigurationProperty("streetNameCorrections", IsRequired = true)]
        [ConfigurationCollection(typeof(KeyValuePairElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public KeyValuePairElementCollection StreetNameCorrections
        {
            get { return (KeyValuePairElementCollection)this["streetNameCorrections"]; }
            set { this["streetNameCorrections"] = value; }
        }
    }
}