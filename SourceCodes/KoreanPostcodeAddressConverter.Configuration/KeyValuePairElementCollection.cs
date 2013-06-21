using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the key/value pair collection entity.
    /// </summary>
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
        /// Gets or sets the key/value pair element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the key/value pair element to remove.</param>
        /// <returns>Returns the key/value pair element at the specified index location.</returns>
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
        /// Gets or sets the key/value pair element having the specified key.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Returns the key/value pair element having the specified key.</returns>
        public new KeyValuePairElement this[string key]
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
        /// Adds an key/value pair element to the ConfigurationElementCollection.
        /// </summary>
        /// <param name="element">Item element.</param>
        public void Add(KeyValuePairElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Removes all key/value pair element objects from the collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Removes an key/value pair element from the collection.
        /// </summary>
        /// <param name="key">Key value.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Removes the key/value pair element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the key/value pair element to remove.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        #endregion
    }
}