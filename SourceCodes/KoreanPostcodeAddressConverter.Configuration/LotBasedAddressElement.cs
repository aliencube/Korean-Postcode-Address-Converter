using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the LOT based address element entity.
    /// </summary>
    public class LotBasedAddressElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the download URL.
        /// </summary>
        [ConfigurationProperty("downloadUrl", IsRequired = true)]
        public DownloadUrlElement DownloadUrl
        {
            get { return (DownloadUrlElement) this["downloadUrl"]; }
            set { this["downloadUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of filenames.
        /// </summary>
        [ConfigurationProperty("filenames", IsRequired = true)]
        [ConfigurationCollection(typeof(FilenamesElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public FilenamesElementCollection Filenames
        {
            get { return (FilenamesElementCollection) this["filenames"]; }
            set { this["filenames"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of filenames.
        /// </summary>
        [ConfigurationProperty("filenameMappings", IsRequired = true)]
        [ConfigurationCollection(typeof(FilenameMappingsElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public FilenameMappingsElementCollection FilenameMappings
        {
            get { return (FilenameMappingsElementCollection)this["filenameMappings"]; }
            set { this["filenameMappings"] = value; }
        }

        /// <summary>
        /// Gets or sets the Excel worksheet name.
        /// </summary>
        [ConfigurationProperty("excelWorksheetName", IsRequired = true)]
        public ExcelWorksheetNameElement ExcelWorksheetName
        {
            get { return (ExcelWorksheetNameElement)this["excelWorksheetName"]; }
            set { this["excelWorksheetName"] = value; }
        }

        /// <summary>
        /// Gets or sets the download directory.
        /// </summary>
        [ConfigurationProperty("downloadDirectory", IsRequired = true)]
        public DirectoryElement DownloadDirectory
        {
            get { return (DirectoryElement)this["downloadDirectory"]; }
            set { this["downloadDirectory"] = value; }
        }

        /// <summary>
        /// Gets or sets the extract directory.
        /// </summary>
        [ConfigurationProperty("extractDirectory", IsRequired = true)]
        public DirectoryElement ExtractDirectory
        {
            get { return (DirectoryElement)this["extractDirectory"]; }
            set { this["extractDirectory"] = value; }
        }

        /// <summary>
        /// Gets or sets the archive directory.
        /// </summary>
        [ConfigurationProperty("archiveDirectory", IsRequired = true)]
        public DirectoryElement ArchiveDirectory
        {
            get { return (DirectoryElement)this["archiveDirectory"]; }
            set { this["archiveDirectory"] = value; }
        }

        /// <summary>
        /// Gets or sets the filename to archive.
        /// </summary>
        [ConfigurationProperty("archiveFilename", IsRequired = true)]
        public FilenameElement ArchiveFilename
        {
            get { return (FilenameElement)this["archiveFilename"]; }
            set { this["archiveFilename"] = value; }
        }
    }
}