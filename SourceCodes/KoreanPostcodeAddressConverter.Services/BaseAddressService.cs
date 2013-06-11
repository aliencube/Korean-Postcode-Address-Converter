using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Interfaces;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services
{
    /// <summary>
    /// This represents the base class for address service entities.
    /// </summary>
    public abstract class BaseAddressService : IBaseAddressService
    {
        #region Properties
        /// <summary>
        /// Gets the download URL.
        /// </summary>
        public abstract string DownloadUrl { get; }

        /// <summary>
        /// Gets the directory to download files.
        /// </summary>
        public abstract string DownloadDirectory { get; }

        /// <summary>
        /// Gets the directory to extract files.
        /// </summary>
        public abstract string ExtractDirectory { get; }

        /// <summary>
        /// Gets the list of filenames to download or extract.
        /// </summary>
        public abstract IList<string> FilenamesToDownloadOrExtract { get; }
        #endregion

        #region Methods - Abstract
        /// <summary>
        /// Downloads files.
        /// </summary>
        public abstract void DownloadFiles();

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        public abstract void ExtractFiles();

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        public abstract void ConvertEncodings();

        /// <summary>
        /// Gets XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        public abstract void GetXmlDocuments();
        #endregion

        #region Methods - Virtual
        /// <summary>
        /// Unzips the .zip files to extract directory.
        /// </summary>
        /// <param name="filenames">List of .zip files.</param>
        /// <param name="sourceDirectory">Directory where the .zip files are stored.</param>
        /// <param name="destinationDirectory">Directory where the .zip files are extracted. Default value is the same as the source directory.</param>
        protected virtual void UnzipZipFiles(IEnumerable<string> filenames,
                                             string sourceDirectory,
                                             string destinationDirectory = null)
        {
            if (String.IsNullOrWhiteSpace(destinationDirectory))
                destinationDirectory = sourceDirectory;

            foreach (var filename in filenames)
            {
                using (var zip = ZipFile.Read(String.Format("{0}\\{1}", sourceDirectory, filename),
                                              new ReadOptions() { Encoding = Encoding.GetEncoding(949) }))
                {
                    foreach (var entry in zip.Entries)
                        entry.Extract(destinationDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        /// <summary>
        /// Unzips the self-extracting .exe files to extract directory.
        /// </summary>
        /// <param name="filenames">List of .exe files.</param>
        /// <param name="unzippath">Unzip executable path for command line process</param>
        /// <param name="sourceDirectory">Directory where the .exe files are stored.</param>
        /// <param name="destinationDirectory">Directory where the .exe files are extracted. Default value is the same as the source directory.</param>
        protected virtual void UnzipSfxFiles(IEnumerable<string> filenames,
                                             string unzippath,
                                             string sourceDirectory,
                                             string destinationDirectory = null)
        {
            if (String.IsNullOrWhiteSpace(destinationDirectory))
                destinationDirectory = sourceDirectory;

            foreach (var filename in filenames)
            {
                using (var process = new Process())
                {
                    var psi = new ProcessStartInfo(unzippath)
                    {
                        UseShellExecute = false,
                        WorkingDirectory = sourceDirectory,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        Arguments = String.Format("e {0} -o{1} *.xls -y", String.Format("{0}\\{1}", sourceDirectory, filename), destinationDirectory)
                    };
                    process.StartInfo = psi;
                    process.Start();
                    process.WaitForExit();
                }
            }
        }

        /// <summary>
        /// Archives XML documents generated.
        /// </summary>
        public virtual void ZipXmlDocuments(string filename, string sourceDirectory, string destinationDirectory = null)
        {
            if (String.IsNullOrWhiteSpace(destinationDirectory))
                destinationDirectory = sourceDirectory;

            using (var zip = new ZipFile(String.Format("{0}\\{1}", destinationDirectory, filename), Encoding.UTF8))
            {
                foreach (var filepath in Directory.GetFiles(sourceDirectory).Where(p => p.EndsWith(".xml")))
                    zip.AddFile(filepath, String.Empty);
                zip.Save();
            }
        }
        #endregion

        #region Methods - Factory
        /// <summary>
        /// Creates the service based on the service type.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="settings">Configuration settings.</param>
        /// <returns>Returns the service based on the service type.</returns>
        public static BaseAddressService Create(ConverterServiceType serviceType, Settings settings)
        {
            BaseAddressService service;
            switch (serviceType)
            {
                case ConverterServiceType.Lot:
                    service = new LotBasedAddressService(settings);
                    break;
                case ConverterServiceType.Street:
                    service = new StreetBasedAddressService(settings);
                    break;
                default:
                    throw new NotSupportedException("Service type is not defined");
            }
            return service;
        }
        #endregion
    }
}
