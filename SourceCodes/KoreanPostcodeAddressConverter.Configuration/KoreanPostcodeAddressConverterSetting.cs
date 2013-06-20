using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
	/// <summary>
	/// This represents the Korean postcode-address conversion settings configuration entity.
	/// </summary>
	public class KoreanPostcodeAddressConverterSettings : ConfigurationSection
	{
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
		/// Gets or sets the segment delimiters for directory element.
		/// </summary>
		[ConfigurationProperty("segmentDelimitersForDirectory", IsRequired = true)]
		public SegmentDelimitersForDirectoryElement SegmentSeparatorForDirectory
		{
			get { return (SegmentDelimitersForDirectoryElement)this["segmentDelimitersForDirectory"]; }
			set { this["segmentDelimitersForDirectory"] = value; }
		}

		/// <summary>
		/// Gets or sets the segment delimiters for file element.
		/// </summary>
		[ConfigurationProperty("segmentDelimitersForFile", IsRequired = true)]
		public SegmentDelimitersForFileElement SegmentSeparatorForFile
		{
			get { return (SegmentDelimitersForFileElement)this["segmentDelimitersForFile"]; }
			set { this["segmentDelimitersForFile"] = value; }
		}

		/// <summary>
		/// Gets or sets the segment delimiters for CSV element.
		/// </summary>
		[ConfigurationProperty("segmentDelemitersForCsv", IsRequired = true)]
		public SegmentDelimitersForCsvElement SegmentSeparatorForCsv
		{
			get { return (SegmentDelimitersForCsvElement)this["segmentDelemitersForCsv"]; }
			set { this["segmentDelemitersForCsv"] = value; }
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

		[ConfigurationProperty("addressMarkers", IsRequired = true)]
		public AddressMarkersElement AddressMarkers
		{
			get { return (AddressMarkersElement)this["addressMarkers"]; }
			set { this["addressMarkers"] = value; }
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

	/// <summary>
	/// This represents the address markers element entity.
	/// </summary>
	public class AddressMarkersElement : ConfigurationElement
	{
		/// <summary>
		/// Gets or sets the province markers collection element.
		/// </summary>
		[ConfigurationProperty("province", IsRequired = true)]
		public AddressMarkersElementCollection Province
		{
			get { return (AddressMarkersElementCollection)this["province"]; }
			set { this["province"] = value; }
		}

		/// <summary>
		/// Gets or sets the county markers collection element.
		/// </summary>
		[ConfigurationProperty("county", IsRequired = true)]
		public AddressMarkersElementCollection County
		{
			get { return (AddressMarkersElementCollection)this["county"]; }
			set { this["county"] = value; }
		}

		/// <summary>
		/// Gets or sets the district markers collection element.
		/// </summary>
		[ConfigurationProperty("district", IsRequired = true)]
		public AddressMarkersElementCollection District
		{
			get { return (AddressMarkersElementCollection)this["district"]; }
			set { this["district"] = value; }
		}

		/// <summary>
		/// Gets or sets the suburb markers collection element.
		/// </summary>
		[ConfigurationProperty("suburb", IsRequired = true)]
		public AddressMarkersElementCollection Suburb
		{
			get { return (AddressMarkersElementCollection)this["suburb"]; }
			set { this["suburb"] = value; }
		}

		/// <summary>
		/// Gets or sets the village markers collection element.
		/// </summary>
		[ConfigurationProperty("village", IsRequired = true)]
		public AddressMarkersElementCollection Village
		{
			get { return (AddressMarkersElementCollection)this["village"]; }
			set { this["village"] = value; }
		}

		/// <summary>
		/// Gets or sets the island markers collection element.
		/// </summary>
		[ConfigurationProperty("island", IsRequired = true)]
		public AddressMarkersElementCollection Island
		{
			get { return (AddressMarkersElementCollection)this["island"]; }
			set { this["island"] = value; }
		}

		/// <summary>
		/// Gets or sets the san markers collection element.
		/// </summary>
		[ConfigurationProperty("san", IsRequired = true)]
		public AddressMarkersElementCollection San
		{
			get { return (AddressMarkersElementCollection)this["san"]; }
			set { this["san"] = value; }
		}

		/// <summary>
		/// Gets or sets the street markers collection element.
		/// </summary>
		[ConfigurationProperty("street", IsRequired = true)]
		public AddressMarkersElementCollection Street
		{
			get { return (AddressMarkersElementCollection)this["street"]; }
			set { this["street"] = value; }
		}

		[ConfigurationProperty("correctioins", IsRequired = true)]
		public KeyValuePairElementCollection Corrections
		{
			get { return (KeyValuePairElementCollection)this["correctioins"]; }
			set { this["correctioins"] = value; }
		}
	}

	public class KeyValuePairElementCollection : ConfigurationElementCollection
	{
		#region Properties
		/// <summary>
		/// Gets the type of the ConfigurationElementCollection.
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		/// <summary>
		/// Gets or sets the key/value element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the key/value element to remove.</param>
		/// <returns>Returns the key/value element at the specified index location.</returns>
		public KeyValuePairElement this[int index]
		{
			get { return (KeyValuePairElement)BaseGet(index); }
			set
			{
				if (BaseGet(index) != null)
					BaseRemoveAt(index);
				BaseAdd(index, value);
			}
		}

		/// <summary>
		/// Gets or sets the key/value element having the specified key.
		/// </summary>
		/// <param name="key">Key value.</param>
		/// <returns>Returns the key/value element having the specified key.</returns>
		public KeyValuePairElement this[string key]
		{
			get { return (KeyValuePairElement)BaseGet(key); }
			set
			{
				var item = (KeyValuePairElement)BaseGet(key);
				if (item != null)
				{
					var index = BaseIndexOf(item);
					BaseRemoveAt(index);
					BaseAdd(index, value);
				}
				BaseAdd(value);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Creates a new ConfigurationElement.
		/// </summary>
		/// <returns>Returns a new ConfigurationElement.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new KeyValuePairElement();
		}

		/// <summary>
		/// Gets the element key for a specified configuration element.
		/// </summary>
		/// <param name="element">ConfigurationElement to return for.</param>
		/// <returns>Returns an Object that acts as the key for the specified ConfigurationElement.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((KeyValuePairElement)element).Key;
		}

		/// <summary>
		/// Adds an key/value element to the ConfigurationElementCollection.
		/// </summary>
		/// <param name="element">Item element.</param>
		public void Add(KeyValuePairElement element)
		{
			BaseAdd(element);
		}

		/// <summary>
		/// Removes all key/value element objects from the collection.
		/// </summary>
		public void Clear()
		{
			BaseClear();
		}

		/// <summary>
		/// Removes an key/value element from the collection.
		/// </summary>
		/// <param name="key">Key value.</param>
		public void Remove(string key)
		{
			BaseRemove(key);
		}

		/// <summary>
		/// Removes the key/value element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the key/value element to remove.</param>
		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}
		#endregion
	}

	public class KeyValuePairElement : ConfigurationElement
	{
		[ConfigurationProperty("key", DefaultValue = "", IsRequired = true)]
		public string Key
		{
			get { return (string)this["key"]; }
			set { this["key"] = value; }
		}

		[ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
		public string Value
		{
			get { return (string)this["value"]; }
			set { this["value"] = value; }
		}
	}

	public class AddressMarkersElementCollection : ConfigurationElementCollection
	{
		#region Properties
		/// <summary>
		/// Gets the type of the ConfigurationElementCollection.
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		/// <summary>
		/// Gets or sets the address marker element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the key/value element to remove.</param>
		/// <returns>Returns the key/value element at the specified index location.</returns>
		public AddressMarkersElement this[int index]
		{
			get { return (AddressMarkersElement)BaseGet(index); }
			set
			{
				if (BaseGet(index) != null)
					BaseRemoveAt(index);
				BaseAdd(index, value);
			}
		}

		/// <summary>
		/// Gets or sets the address marker element having the specified key.
		/// </summary>
		/// <param name="key">Key value.</param>
		/// <returns>Returns the key/value element having the specified key.</returns>
		public AddressMarkersElement this[string key]
		{
			get { return (AddressMarkersElement)BaseGet(key); }
			set
			{
				var item = (KeyValuePairElement)BaseGet(key);
				if (item != null)
				{
					var index = BaseIndexOf(item);
					BaseRemoveAt(index);
					BaseAdd(index, value);
				}
				BaseAdd(value);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Creates a new ConfigurationElement.
		/// </summary>
		/// <returns>Returns a new ConfigurationElement.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new AddressMarkersElement();
		}

		/// <summary>
		/// Gets the element key for a specified configuration element.
		/// </summary>
		/// <param name="element">ConfigurationElement to return for.</param>
		/// <returns>Returns an Object that acts as the key for the specified ConfigurationElement.</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AddressMarkersElement)element).Key;
		}

		/// <summary>
		/// Adds an key/value element to the ConfigurationElementCollection.
		/// </summary>
		/// <param name="element">Item element.</param>
		public void Add(AddressMarkersElement element)
		{
			BaseAdd(element);
		}

		/// <summary>
		/// Removes all key/value element objects from the collection.
		/// </summary>
		public void Clear()
		{
			BaseClear();
		}

		/// <summary>
		/// Removes an key/value element from the collection.
		/// </summary>
		/// <param name="key">Key value.</param>
		public void Remove(string key)
		{
			BaseRemove(key);
		}

		/// <summary>
		/// Removes the key/value element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the key/value element to remove.</param>
		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}
		#endregion
	}
}
