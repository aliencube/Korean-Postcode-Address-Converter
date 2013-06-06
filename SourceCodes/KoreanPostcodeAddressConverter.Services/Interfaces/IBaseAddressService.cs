using System.Collections.Generic;

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
        /// Gets the directory to download files.
        /// </summary>
        string DownloadDirectory { get; }

        /// <summary>
        /// Gets the directory to extract files.
        /// </summary>
        string ExtractDirectory { get; }

        /// <summary>
        /// Gets the list of filenames to download or extract.
        /// </summary>
        IList<string> FilenamesToDownloadOrExtract { get; }
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
        /// Archives XML documents generated.
        /// </summary>
        void ZipXmlDocuments(string filename, string sourceDirectory, string destinationDirectory = null);
        #endregion
    }
}
