﻿using System;
using System.Collections.Generic;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the BaseAddressServiceService class.
    /// </summary>
    public interface IBaseAddressService
    {
        #region Properties
        /// <summary>
        /// Gets the proxy server settings element.
        /// </summary>
        ProxyServerElement ProxyServer { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipDownloadingFiles { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipExtractingFiles { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipConvertingFiles { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipGeneratingXmlDocuments { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipArchivingXmlDocuments { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipEmptyingDirectories { get; }

        /// <summary>
        /// Gets the value that specifies whether to download files from the source or not.
        /// </summary>
        bool SkipLoadingDatabase { get; }

        /// <summary>
        /// Gets the block size for database loading at once.
        /// </summary>
        int DatabaseLoadingBlockSize { get; }

        /// <summary>
        /// Gets the list of delimiters for a directory.
        /// </summary>
        char[] DirectoryDelimiters { get; }

        /// <summary>
        /// Gets the list of delimiters for a file.
        /// </summary>
        char[] FileDelimiters { get; }

        /// <summary>
        /// Gets the list of delimiters for a CSV formatted file.
        /// </summary>
        char[] CsvDelimiters { get; }

        /// <summary>
        /// Gets the unzip executable (eg. 7-zip) location path.
        /// </summary>
        string UnzipPath { get; }

        /// <summary>
        /// Gets the list of Province markers.
        /// </summary>
        IList<string> ProvinceMarkers { get; }

        /// <summary>
        /// Gets the list of County markers.
        /// </summary>
        IList<string> CountyMarkers { get; }

        /// <summary>
        /// Gets the list of District markers.
        /// </summary>
        IList<string> DistrictMarkers { get; }

        /// <summary>
        /// Gets the list of Suburb markers.
        /// </summary>
        IList<string> SuburbMarkers { get; }

        /// <summary>
        /// Gets the list of Village markers.
        /// </summary>
        IList<string> VillageMarkers { get; }

        /// <summary>
        /// Gets the list of Island markers.
        /// </summary>
        IList<string> IslandMarkers { get; }

        /// <summary>
        /// Gets the list of San (ie. former forrest area) markers.
        /// </summary>
        IList<string> SanMarkers { get; }

        /// <summary>
        /// Gets the list of Street markers.
        /// </summary>
        IList<string> StreetMarkers { get; }

        /// <summary>
        /// Gets the list of street name corrections.
        /// </summary>
        IDictionary<string, string> StreetNameCorrections { get; }

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
        /// <param name="skipDownloading">Value that specifies whether to skip downloading files or not.</param>
        void DownloadFiles(bool skipDownloading);

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        /// <param name="skipExtracting">Value that specifies whether to skip extracting files or not.</param>
        void ExtractFiles(bool skipExtracting);

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        /// <param name="skipConverting">Value that specifies whether to skip converting files or not.</param>
        void ConvertEncodings(bool skipConverting);

        /// <summary>
        /// Generates XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        /// <param name="skipGenerating">Value that specifies whether to skip generating XML documents or not.</param>
        void GenerateXmlDocuments(bool skipGenerating);

        /// <summary>
        /// Archives XML documents generated.
        /// </summary>
        /// <param name="skipArchiving">Value that specifies whether to skip archiving files or not.</param>
        void ArchiveXmlDocuments(bool skipArchiving);

        /// <summary>
        /// Empty both downloads and extracts directory for cleanup.
        /// </summary>
        /// <param name="skipEmptying">Value that specifies whether to skip emptying working directories or not.</param>
        void EmptyDirectories(bool skipEmptying);

        /// <summary>
        /// Loads objects to database.
        /// </summary>
        /// <param name="skipLoading">Value that specifies whether to skip loading XML documents to database or not.</param>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        /// <param name="blockSize">Number of records to load to database at once.</param>
        void LoadDatabase(bool skipLoading, string sourceDirectory, int blockSize);

        /// <summary>
        /// Processes the requests.
        /// </summary>
        /// <param name="skipDownloading">Value that specifies whether to skip downloading files or not.</param>
        /// <param name="skipExtracting">Value that specifies whether to skip extracting files or not.</param>
        /// <param name="skipConverting">Value that specifies whether to skip converting files or not.</param>
        /// <param name="skipGenerating">Value that specifies whether to skip generating XML documents or not.</param>
        /// <param name="skipArchiving">Value that specifies whether to skip archiving files or not.</param>
        /// <param name="skipEmptying">Value that specifies whether to skip emptying working directories or not.</param>
        void ProcessRequests(bool skipDownloading,
                             bool skipExtracting,
                             bool skipConverting,
                             bool skipGenerating,
                             bool skipArchiving,
                             bool skipEmptying);
        #endregion
    }
}
