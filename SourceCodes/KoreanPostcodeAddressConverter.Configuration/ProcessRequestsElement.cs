using System.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration
{
    /// <summary>
    /// This represents the process requests element entity.
    /// </summary>
    public class ProcessRequestsElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the value that specifies whether to skip downloading files or not.
        /// </summary>
        [ConfigurationProperty("skipDownloadingFiles", DefaultValue = false, IsRequired = true)]
        public bool SkipDownloadingFiles
        {
            get { return (bool)this["skipDownloadingFiles"]; }
            set { this["skipDownloadingFiles"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to skip extracting files or not.
        /// </summary>
        [ConfigurationProperty("skipExtractingFiles", DefaultValue = false, IsRequired = true)]
        public bool SkipExtractingFiles
        {
            get { return (bool)this["skipExtractingFiles"]; }
            set { this["skipExtractingFiles"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to skip converting files or not.
        /// </summary>
        [ConfigurationProperty("skipConvertingFiles", DefaultValue = false, IsRequired = true)]
        public bool SkipConvertingFiles
        {
            get { return (bool)this["skipConvertingFiles"]; }
            set { this["skipConvertingFiles"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to skip generating XML documents or not.
        /// </summary>
        [ConfigurationProperty("skipGeneratingXmlDocuments", DefaultValue = false, IsRequired = true)]
        public bool SkipGeneratingXmlDocuments
        {
            get { return (bool)this["skipGeneratingXmlDocuments"]; }
            set { this["skipGeneratingXmlDocuments"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to skip archiving XML documents or not.
        /// </summary>
        [ConfigurationProperty("skipArchivingXmlDocuments", DefaultValue = false, IsRequired = true)]
        public bool SkipArchivingXmlDocuments
        {
            get { return (bool)this["skipArchivingXmlDocuments"]; }
            set { this["skipArchivingXmlDocuments"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to skip emptying directories or not.
        /// </summary>
        [ConfigurationProperty("skipEmptyingDirectories", DefaultValue = false, IsRequired = true)]
        public bool SkipEmptyingDirectories
        {
            get { return (bool)this["skipEmptyingDirectories"]; }
            set { this["skipEmptyingDirectories"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to skip loading database or not.
        /// </summary>
        [ConfigurationProperty("skipLoadingDatabase", DefaultValue = true, IsRequired = true)]
        public bool SkipLoadingDatabase
        {
            get { return (bool)this["skipLoadingDatabase"]; }
            set { this["skipLoadingDatabase"] = value; }
        }

        /// <summary>
        /// Gets or sets the value the block size for database loading at once.
        /// </summary>
        [ConfigurationProperty("databaseLoadingBlockSize", DefaultValue = 50000, IsRequired = true)]
        public long DatabaseLoadingBlockSize
        {
            get { return (long)this["databaseLoadingBlockSize"]; }
            set { this["databaseLoadingBlockSize"] = value; }
        }
    }
}