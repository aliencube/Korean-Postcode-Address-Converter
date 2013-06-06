using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the Korean postcode-address conversion settings configuration entity.
    /// </summary>
    public class KoreanPostcodeAddressConverterSettings : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the segment delimiters for directory element.
        /// </summary>
        [ConfigurationProperty("segmentDelimitersForDirectory", IsRequired = true)]
        public SegmentDelimitersForDirectoryElement SegmentSeparatorForDirectory
        {
            get { return (SegmentDelimitersForDirectoryElement) this["segmentDelimitersForDirectory"]; }
            set { this["segmentDelimitersForDirectory"] = value; }
        }

        /// <summary>
        /// Gets or sets the segment delimiters for file element.
        /// </summary>
        [ConfigurationProperty("segmentDelimitersForFile", IsRequired = true)]
        public SegmentDelimitersForFileElement SegmentSeparatorForFile
        {
            get { return (SegmentDelimitersForFileElement) this["segmentDelimitersForFile"]; }
            set { this["segmentDelimitersForFile"] = value; }
        }

        /// <summary>
        /// Gets or sets the segment delimiters for CSV element.
        /// </summary>
        [ConfigurationProperty("segmentDelemitersForCsv", IsRequired = true)]
        public SegmentDelimitersForCsvElement SegmentSeparatorForCsv
        {
            get { return (SegmentDelimitersForCsvElement) this["segmentDelemitersForCsv"]; }
            set { this["segmentDelemitersForCsv"] = value; }
        }

        /// <summary>
        /// Gets or sets the unzip path element.
        /// </summary>
        [ConfigurationProperty("unzipPath", IsRequired = true)]
        public UnzipPathElement UnzipPath
        {
            get { return (UnzipPathElement)this["unzipPath"]; }
            set { this["unzipPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the LOT based address element.
        /// </summary>
        [ConfigurationProperty("lotBasedAddress", IsRequired = true)]
        public LotBasedAddressElement LotBasedAddress
        {
            get { return (LotBasedAddressElement) this["lotBasedAddress"]; }
            set { this["lotBasedAddress"] = value; }
        }

        /// <summary>
        /// Gets or sets the Street based address element.
        /// </summary>
        [ConfigurationProperty("streetBasedAddress", IsRequired = true)]
        public StreetBasedAddressElement StreetBasedAddress
        {
            get { return (StreetBasedAddressElement) this["streetBasedAddress"]; }
            set { this["streetBasedAddress"] = value; }
        }
    }
}
