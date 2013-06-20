using System;
using System.Collections.Generic;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the AddressServiceService class.
    /// </summary>
    public interface IBaseAddressService
    {
        #region Properties
        /// <summary>
        /// Gets the download URL.
        /// </summary>
        string DownloadUrl { get; }

        /// <summary>
        /// Gets the list of filenames to download or extract.
        /// </summary>
        IList<string> FilenamesToDownloadOrExtract { get; }

        /// <summary>
        /// Gets the directory to download files.
        /// </summary>
        string DownloadDirectory { get; }

        /// <summary>
        /// Gets the directory to extract files.
        /// </summary>
        string ExtractDirectory { get; }

        /// <summary>
        /// Gets the directory to archive files.
        /// </summary>
        string ArchiveDirectory { get; }

        /// <summary>
        /// Gets the filename for archive.
        /// </summary>
        string FilenameForArchive { get; }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when status changed event is raised.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Occurs when exception thrown event is raised.
        /// </summary>
        event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;
        #endregion

        #region Methods
        /// <summary>
        /// Downloads files.
        /// </summary>
        /// <param name="skipDownload">Value that specifies whether to skip this download process or not. Default value is <c>False</c>.</param>
        void DownloadFiles(bool skipDownload = false);

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        void ExtractFiles();

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        void ConvertEncodings();

        /// <summary>
        /// Gets XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        void GetXmlDocuments();

        /// <summary>
        /// Loads objects to database.
        /// </summary>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        void LoadDatabase(string sourceDirectory);

        /// <summary>
        /// Archives XML documents generated.
        /// </summary>
        /// <param name="filename">Filename for archive.</param>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        /// <param name="destinationDirectory">Destination directory where the archive file is stored.</param>
        void ZipXmlDocuments(string filename, string sourceDirectory, string destinationDirectory = null);

        /// <summary>
        /// Empty both downloads and extracts directory for cleanup.
        /// </summary>
        /// <param name="empty">Value that specifies whether directories used for download and extract are emptied or not. Default value is <c>True</c>.</param>
        /// <param name="archive">Value that specifies whether processed XML documents are zipped or not. Default value is <c>True</c>.</param>
        void EmptyDirectories(bool empty = true, bool archive = true);

        /// <summary>
        /// Processes the requests.
        /// </summary>
        /// <param name="skipDownload">Value that specifies whether to skip this download process or not. Default value is <c>False</c>.</param>
        /// <param name="empty">Value that specifies whether directories used for download and extract are emptied or not. Default value is <c>True</c>.</param>
        /// <param name="archive">Value that specifies whether processed XML documents are zipped or not. Default value is <c>True</c>.</param>
        void ProcessRequests(bool skipDownload = false, bool empty = true, bool archive = true);
        #endregion
    }
}
