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
        /// Occurs before the file download is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Downloading;

        /// <summary>
        /// Occurs after the file download is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Downloaded;

        /// <summary>
        /// Occurs before the file extraction is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Extracting;

        /// <summary>
        /// Occurs after the file extraction is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Extracted;

        /// <summary>
        /// Occurs before the file unzipping is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Unzipping;

        /// <summary>
        /// Occurs after the file unzipping is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Unzipped;

        /// <summary>
        /// Occurs before the file encoding conversion is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Converting;

        /// <summary>
        /// Occurs after the file encoding conversion is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Converted;

        /// <summary>
        /// Occurs before the XML document generation is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> GeneratingXmlDocument;

        /// <summary>
        /// Occurs after the XML document generation is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> GeneratedXmlDocument;

        /// <summary>
        /// Occurs before the XML documents archiving is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Archiving;

        /// <summary>
        /// Occurs after the XML documents archiving is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Archived;

        /// <summary>
        /// Occurs before emptying directories is started.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Emptying;

        /// <summary>
        /// Occurs after emptying directories is completed.
        /// </summary>
        event EventHandler<StatusChangeEventArgs> Emptied;
        #endregion

        #region Methods
        /// <summary>
        /// Downloads files.
        /// </summary>
        void DownloadFiles();

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
        /// Processes the requests.
        /// </summary>
        /// <param name="archive">Value that specifies whether processed XML documents are zipped or not. Default value is <c>True</c>.</param>
        void ProcessRequests(bool archive = true);
        #endregion
    }
}
