using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
	/// This represents the Korean postcode-address conversion settings configuration entity.
	/// </summary>
	public class KoreanPostcodeAddressConverterSettings : ConfigurationSection
	{
        /// <summary>
        /// Gets or sets the proxy server element.
        /// </summary>
        [ConfigurationProperty("proxyServer", IsRequired = true)]
        public ProxyServerElement ProxyServer
        {
            get { return (ProxyServerElement)this["proxyServer"]; }
            set { this["proxyServer"] = value; }
        }

		/// <summary>
		/// Gets or sets the process requests element.
		/// </summary>
		[ConfigurationProperty("processRequests", IsRequired = true)]
		public ProcessRequestsElement ProcessRequests
		{
			get { return (ProcessRequestsElement)this["processRequests"]; }
			set { this["processRequests"] = value; }
		}

		/// <summary>
		/// Gets or sets the segment delimiters element.
		/// </summary>
		[ConfigurationProperty("segmentDelimiters", IsRequired = true)]
		public SegmentDelimitersElement SegmentDelimiters
		{
			get { return (SegmentDelimitersElement)this["segmentDelimiters"]; }
			set { this["segmentDelimiters"] = value; }
		}

        /// <summary>
		/// Gets or sets the unzip path element.
		/// </summary>
		[ConfigurationProperty("unzipPath", IsRequired = true)]
		public FilepathElement UnzipPath
		{
			get { return (FilepathElement)this["unzipPath"]; }
			set { this["unzipPath"] = value; }
		}

        /// <summary>
        /// Gets or sets the location markers element.
        /// </summary>
        [ConfigurationProperty("locationMarkers", IsRequired = true)]
		public LocationMarkersElement LocationMarkers
		{
            get { return (LocationMarkersElement)this["locationMarkers"]; }
            set { this["locationMarkers"] = value; }
		}

		/// <summary>
		/// Gets or sets the LOT based address element.
		/// </summary>
		[ConfigurationProperty("lotBasedAddress", IsRequired = true)]
		public LotBasedAddressElement LotBasedAddress
		{
			get { return (LotBasedAddressElement)this["lotBasedAddress"]; }
			set { this["lotBasedAddress"] = value; }
		}

		/// <summary>
		/// Gets or sets the Street based address element.
		/// </summary>
		[ConfigurationProperty("streetBasedAddress", IsRequired = true)]
		public StreetBasedAddressElement StreetBasedAddress
		{
			get { return (StreetBasedAddressElement)this["streetBasedAddress"]; }
			set { this["streetBasedAddress"] = value; }
		}
	}
}
