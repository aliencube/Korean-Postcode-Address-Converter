using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
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
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the BaseAddressService object.
        /// </summary>
        /// <param name="settings">Configuration settings.</param>
        protected BaseAddressService(Settings settings)
        {
            this.Settings = settings;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the configuration settings.
        /// </summary>
        protected Settings Settings { get; private set; }

        /// <summary>
        /// Gets the download URL.
        /// </summary>
        public abstract string DownloadUrl { get; }

        /// <summary>
        /// Gets the list of filenames to download or extract.
        /// </summary>
        public abstract IList<string> FilenamesToDownloadOrExtract { get; }

        /// <summary>
        /// Gets the directory to download files.
        /// </summary>
        public abstract string DownloadDirectory { get; }

        /// <summary>
        /// Gets the directory to extract files.
        /// </summary>
        public abstract string ExtractDirectory { get; }

        /// <summary>
        /// Gets the directory to archive files.
        /// </summary>
        public abstract string ArchiveDirectory { get; }

        /// <summary>
        /// Gets the filename for archive.
        /// </summary>
        public abstract string FilenameForArchive { get; }
        #endregion

        #region Events
        /// <summary>
        /// Occurs before the file download is started.
        /// </summary>
        public event EventHandler<StatusChangeEventArgs> Downloading;

        /// <summary>
        /// Occurs after the file download is completed.
        /// </summary>
        public event EventHandler<StatusChangeEventArgs> Downloaded;
        #endregion

        #region Event Handlers
        /// <summary>
        /// Occurs before the file download is started.
        /// </summary>
        /// <param name="e">Provides data for the file downloading started event.</param>
        protected virtual void OnDownloading(StatusChangeEventArgs e)
        {
            var handler = this.Downloading;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs after the file download is completed.
        /// </summary>
        /// <param name="e">Provides data for the file downloading completed event.</param>
        protected virtual void OnDownloaded(StatusChangeEventArgs e)
        {
            var handler = this.Downloaded;
            if (handler != null)
                handler(this, e);
        }
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

        /// <summary>
        /// Loads objects to database.
        /// </summary>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        public abstract void LoadDatabase(string sourceDirectory);
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
        /// Checks if archived files already exist or not.
        /// </summary>
        /// <param name="extractDirectory">Directory where extracted files are located.</param>
        /// <param name="archiveDirectory">Directory where archived files are located.</param>
        /// <returns>Returns <c>True</c>, if archived files exist; otherwise returns <c>False</c>.</returns>
        protected virtual bool ExistArhives(string extractDirectory, string archiveDirectory)
        {
            var filename = Directory.GetFiles(extractDirectory)
                                    .Select(p => p.Split(this.Settings
                                                             .ConversionSettings
                                                             .SegmentSeparatorForDirectory
                                                             .Delimiters
                                                             .ToCharArray(),
                                                         StringSplitOptions.RemoveEmptyEntries)
                                                  .Last())
                                    .First();
            var timestamp = filename.Split(this.Settings
                                               .ConversionSettings
                                               .SegmentSeparatorForFile
                                               .Delimiters
                                               .ToCharArray(),
                                           StringSplitOptions.RemoveEmptyEntries)
                                    .First();
            return Directory.Exists(String.Format("{0}\\{1}", archiveDirectory, timestamp));
        }

        /// <summary>
        /// Archives XML documents generated.
        /// </summary>
        /// <param name="filename">Filename for archive.</param>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        /// <param name="destinationDirectory">Destination directory where the archive file is stored.</param>
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

        /// <summary>
        /// Empty both downloads and extracts directory for cleanup.
        /// </summary>
        /// <param name="archive">Value that specifies whether processed XML documents are zipped or not. Default value is <c>True</c>.</param>
        public virtual void EmptyDirectories(bool archive = true)
        {
            foreach (var filepath in Directory.GetFiles(this.DownloadDirectory))
                File.Delete(filepath);

            var filename = Directory.GetFiles(this.ExtractDirectory)
                                    .Select(p => p.Split(this.Settings
                                                             .ConversionSettings
                                                             .SegmentSeparatorForDirectory
                                                             .Delimiters
                                                             .ToCharArray(),
                                                         StringSplitOptions.RemoveEmptyEntries)
                                                  .Last())
                                    .First();
            var timestamp = filename.Split(this.Settings
                                               .ConversionSettings
                                               .SegmentSeparatorForFile
                                               .Delimiters
                                               .ToCharArray(),
                                           StringSplitOptions.RemoveEmptyEntries)
                                    .First();
            var archivedirectory = String.Format("{0}\\{1}", this.ArchiveDirectory, timestamp);
            if (!Directory.Exists(archivedirectory))
                Directory.CreateDirectory(archivedirectory);

            foreach (var filepath in Directory.GetFiles(this.ExtractDirectory))
            {
                if (archive || !filepath.EndsWith(".xml"))
                    File.Delete(filepath);
                else if (filepath.EndsWith(".xml"))
                {
                    var archivename = filepath.Split(this.Settings
                                                         .ConversionSettings
                                                         .SegmentSeparatorForDirectory
                                                         .Delimiters
                                                         .ToCharArray(),
                                                     StringSplitOptions.RemoveEmptyEntries)
                                              .Last();
                    File.Move(filepath, String.Format("{0}\\{1}", archivedirectory, archivename));
                }
            }
        }

        /// <summary>
        /// Processes the requests.
        /// </summary>
        /// <param name="archive">Value that specifies whether processed XML documents are zipped or not. Default value is <c>True</c>.</param>
        public virtual void ProcessRequests(bool archive = true)
        {
            this.OnStarting(new StatusChangeEventArgs("Starting the process"));

            this.DownloadFiles();
            this.ExtractFiles();

            if (this.ExistArhives(this.ExtractDirectory, this.ArchiveDirectory))
                return;

            this.ConvertEncodings();
            this.GetXmlDocuments();

            if (archive)
            {
                var filename = Directory.GetFiles(this.ExtractDirectory)
                                        .Select(p => p.Split(this.Settings
                                                                 .ConversionSettings
                                                                 .SegmentSeparatorForDirectory
                                                                 .Delimiters
                                                                 .ToCharArray(),
                                                             StringSplitOptions.RemoveEmptyEntries)
                                                      .Last())
                                        .First();
                var timestamp = filename.Split(this.Settings
                                                   .ConversionSettings
                                                   .SegmentSeparatorForFile
                                                   .Delimiters
                                                   .ToCharArray(),
                                               StringSplitOptions.RemoveEmptyEntries)
                                        .First();
                var archivedirectory = String.Format("{0}\\{1}", this.ArchiveDirectory, timestamp);
                if (!Directory.Exists(archivedirectory))
                    Directory.CreateDirectory(archivedirectory);

                this.ZipXmlDocuments(this.FilenameForArchive, this.ExtractDirectory, archivedirectory);
            }

            this.EmptyDirectories(archive);
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
