using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the filenames collection entity.
    /// </summary>
    public class FilenamesElementCollection : ConfigurationElementCollection
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
        /// Gets or sets the filename element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the filename element to remove.</param>
        /// <returns>Returns the filename element at the specified index location.</returns>
        public FilenameElement this[int index]
        {
            get { return (FilenameElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets or sets the filename element having the specified key.
        /// </summary>
        /// <param name="alias">Key value.</param>
        /// <returns>Returns the filename element having the specified key.</returns>
        public FilenameElement this[string alias]
        {
            get { return (FilenameElement)BaseGet(alias); }
            set
            {
                var item = (FilenameElement)BaseGet(alias);
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
            return new FilenameElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        /// <param name="element">ConfigurationElement to return for.</param>
        /// <returns>Returns an Object that acts as the key for the specified ConfigurationElement.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilenameElement) element).Filename;
        }

        /// <summary>
        /// Adds an filename element to the ConfigurationElementCollection.
        /// </summary>
        /// <param name="element">Item element.</param>
        public void Add(FilenameElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Removes all filename element objects from the collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Removes an filename element from the collection.
        /// </summary>
        /// <param name="name">Document type alias.</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// Removes the filename element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the filename element to remove.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        #endregion
    }
}