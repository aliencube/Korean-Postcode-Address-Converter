using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers;
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
        /// Gets the proxy server settings element.
        /// </summary>
        public ProxyServerElement ProxyServer
        {
            get { return this.Settings.ConversionSettings.ProxyServer; }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip downloading files or not.
        /// </summary>
        public bool SkipDownloadingFiles
        {
            get { return this.Settings.GetProcessRequests<bool>("skipDownloadingFiles"); }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip extracting files or not.
        /// </summary>
        public bool SkipExtractingFiles
        {
            get { return this.Settings.GetProcessRequests<bool>("skipExtractingFiles"); }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip converting files or not.
        /// </summary>
        public bool SkipConvertingFiles
        {
            get { return this.Settings.GetProcessRequests<bool>("skipConvertingFiles"); }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip generating XML documents or not.
        /// </summary>
        public bool SkipGeneratingXmlDocuments
        {
            get { return this.Settings.GetProcessRequests<bool>("skipGeneratingXmlDocuments"); }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip archiving XML documents or not.
        /// </summary>
        public bool SkipArchivingXmlDocuments
        {
            get { return this.Settings.GetProcessRequests<bool>("skipArchivingXmlDocuments"); }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip emptying directories or not.
        /// </summary>
        public bool SkipEmptyingDirectories
        {
            get { return this.Settings.GetProcessRequests<bool>("skipEmptyingDirectories"); }
        }

        /// <summary>
        /// Gets the value that specifies whether to skip loading database or not.
        /// </summary>
        public bool SkipLoadingDatabase
        {
            get { return this.Settings.GetProcessRequests<bool>("skipLoadingDatabase"); }
        }

        /// <summary>
        /// Gets the block size for database loading at once.
        /// </summary>
        public int DatabaseLoadingBlockSize
        {
            get { return this.Settings.GetProcessRequests<int>("databaseLoadingBlockSize"); }
        }

        /// <summary>
        /// Gets the list of delimiters for a directory.
        /// </summary>
        public char[] DirectoryDelimiters
        {
            get { return this.Settings.GetDelimiters("forDirectory"); }
        }

        /// <summary>
        /// Gets the list of delimiters for a file.
        /// </summary>
        public char[] FileDelimiters
        {
            get { return this.Settings.GetDelimiters("forFile"); }
        }

        /// <summary>
        /// Gets the list of delimiters for a CSV formatted file.
        /// </summary>
        public char[] CsvDelimiters
        {
            get { return this.Settings.GetDelimiters("forCsv"); }
        }

        /// <summary>
        /// Gets the unzip executable (eg. 7-zip) location path.
        /// </summary>
        public string UnzipPath
        {
            get { return this.Settings.UnzipPath; }
        }

        /// <summary>
        /// Gets the list of Province markers.
        /// </summary>
        public IList<string> ProvinceMarkers
        {
            get { return this.Settings.GetLocationMarkers("province"); }
        }

        /// <summary>
        /// Gets the list of County markers.
        /// </summary>
        public IList<string> CountyMarkers
        {
            get { return this.Settings.GetLocationMarkers("county"); }
        }

        /// <summary>
        /// Gets the list of District markers.
        /// </summary>
        public IList<string> DistrictMarkers
        {
            get { return this.Settings.GetLocationMarkers("district"); }
        }

        /// <summary>
        /// Gets the list of Suburb markers.
        /// </summary>
        public IList<string> SuburbMarkers
        {
            get { return this.Settings.GetLocationMarkers("suburb"); }
        }

        /// <summary>
        /// Gets the list of Village markers.
        /// </summary>
        public IList<string> VillageMarkers
        {
            get { return this.Settings.GetLocationMarkers("village"); }
        }

        /// <summary>
        /// Gets the list of Island markers.
        /// </summary>
        public IList<string> IslandMarkers
        {
            get { return this.Settings.GetLocationMarkers("island"); }
        }

        /// <summary>
        /// Gets the list of San (ie. former forrest area) markers.
        /// </summary>
        public IList<string> SanMarkers
        {
            get { return this.Settings.GetLocationMarkers("san"); }
        }

        /// <summary>
        /// Gets the list of Street markers.
        /// </summary>
        public IList<string> StreetMarkers
        {
            get { return this.Settings.GetLocationMarkers("street"); }
        }


        /// <summary>
        /// Gets the list of street name corrections.
        /// </summary>
        public IDictionary<string, string> StreetNameCorrections
        {
            get { return this.Settings.StreetNameCorrections; }
        }

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
        /// Occurs when status changed event is raised.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Occurs when exception thrown event is raised.
        /// </summary>
        public event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;
        #endregion

        #region Event Handlers
        /// <summary>
        /// Occurs when status changed event is raised.
        /// </summary>
        /// <param name="e">Provides data for the status changed event.</param>
        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            var handler = StatusChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when exception thrown event is raised.
        /// </summary>
        /// <param name="e">Provides data for the exception thrown event.</param>
        protected virtual void OnExceptionThrown(ExceptionThrownEventArgs e)
        {
            var handler = ExceptionThrown;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when extraction progress event is raised.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the extraction progress event.</param>
        private void Zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            //  Bubbles up the event to the parent.
            var handler = StatusChanged;
            if (handler != null && e.EventType == ZipProgressEventType.Extracting_AfterExtractEntry)
                handler(sender, new StatusChangedEventArgs(e.CurrentEntry.FileName));
        }
        #endregion

        #region Methods - Abstract
        /// <summary>
        /// Downloads files.
        /// </summary>
        /// <param name="skipDownloading">Value that specifies whether to skip downloading files or not.</param>
        public abstract void DownloadFiles(bool skipDownloading);

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        /// <param name="skipExtracting">Value that specifies whether to skip extracting files or not.</param>
        public abstract void ExtractFiles(bool skipExtracting);

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        /// <param name="skipConverting">Value that specifies whether to skip converting files or not.</param>
        public abstract void ConvertEncodings(bool skipConverting);

        /// <summary>
        /// Generates XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        /// <param name="skipGenerating">Value that specifies whether to skip generating XML documents or not.</param>
        public abstract void GenerateXmlDocuments(bool skipGenerating);

        /// <summary>
        /// Loads objects to database.
        /// </summary>
        /// <param name="skipLoading">Value that specifies whether to skip loading XML documents to database or not.</param>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        /// <param name="blockSize">Number of records to load to database at once.</param>
        public abstract void LoadDatabase(bool skipLoading, string sourceDirectory, int blockSize);
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
                this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Unzipping - {0}", filename)));

                var entries = new List<string>();

                try
                {
                    using (var zip = ZipFile.Read(String.Format("{0}\\{1}", sourceDirectory, filename),
                                                  new ReadOptions() { Encoding = Encoding.GetEncoding(949) }))
                    {
                        zip.ExtractProgress += Zip_ExtractProgress;

                        foreach (var entry in zip.Entries)
                        {
                            entries.Add(String.Format("  {0}", entry.FileName));
                            entry.Extract(destinationDirectory, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.OnExceptionThrown(new ExceptionThrownEventArgs(ex));
                }

                var sb = new StringBuilder();
                sb.AppendLine("Unzipped to");
                foreach (var entry in entries)
                    sb.AppendLine(entry);
                this.OnStatusChanged(new StatusChangedEventArgs(sb.ToString()));
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
                this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Unzipping - {0}", filename)));

                try
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
                catch (Exception ex)
                {
                    this.OnExceptionThrown(new ExceptionThrownEventArgs(ex));
                }

                this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Unzipped - {0}", filename)));
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
            var timestamp = ConversionHelper.GetTimestampFromFilename(Directory.GetFiles(extractDirectory)
                                                                               .Select(p => ConversionHelper.GetFilenameFromFilepath(p, this.Settings))
                                                                               .First(),
                                                                      this.Settings);
            return Directory.Exists(String.Format("{0}\\{1}", archiveDirectory, timestamp));
        }

        /// <summary>
        /// Archives XML documents generated.
        /// </summary>
        /// <param name="filename">Filename for archive.</param>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        /// <param name="destinationDirectory">Destination directory where the archive file is stored.</param>
        public virtual void ArchiveXmlDocuments(string filename, string sourceDirectory, string destinationDirectory = null)
        {
            if (String.IsNullOrWhiteSpace(destinationDirectory))
                destinationDirectory = sourceDirectory;

            this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Archiving to {0}", destinationDirectory)));

            using (var zip = new ZipFile(String.Format("{0}\\{1}", destinationDirectory, filename), Encoding.UTF8))
            {
                foreach (var filepath in Directory.GetFiles(sourceDirectory).Where(p => p.EndsWith(".xml")))
                    zip.AddFile(filepath, String.Empty);
                zip.Save();
            }

            this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Archived to {0}", destinationDirectory)));
        }

        /// <summary>
        /// Empty both downloads and extracts directory for cleanup.
        /// </summary>
        /// <param name="skipArchiving">Value that specifies whether to skip archiving files or not.</param>
        /// <param name="skipEmptying">Value that specifies whether to skip emptying working directories or not.</param>
        public virtual void EmptyDirectories(bool skipArchiving, bool skipEmptying)
        {
            this.OnStatusChanged(new StatusChangedEventArgs("Emptying directories"));

            //  Deletes files in download directory.
            if (!skipEmptying)
                foreach (var filepath in Directory.GetFiles(this.DownloadDirectory))
                    File.Delete(filepath);

            //  Deletes files in extract directory.
            var timestamp = ConversionHelper.GetTimestampFromFilename(Directory.GetFiles(this.ExtractDirectory)
                                                                               .Select(p => ConversionHelper.GetFilenameFromFilepath(p, this.Settings))
                                                                               .First(),
                                                                      this.Settings);
            var archivedirectory = String.Format("{0}\\{1}", this.ArchiveDirectory, timestamp);
            if (!Directory.Exists(archivedirectory))
                Directory.CreateDirectory(archivedirectory);

            foreach (var filepath in Directory.GetFiles(this.ExtractDirectory))
            {
                if (skipArchiving && filepath.EndsWith(".xml"))
                {
                    var archivename = ConversionHelper.GetFilenameFromFilepath(filepath, this.Settings);

                    //  Deletes existing files first before copying or moving.
                    if (File.Exists(String.Format("{0}\\{1}", archivedirectory, archivename)))
                        File.Delete(String.Format("{0}\\{1}", archivedirectory, archivename));

                    if (skipEmptying)
                        File.Copy(filepath, String.Format("{0}\\{1}", archivedirectory, archivename));
                    else
                        File.Move(filepath, String.Format("{0}\\{1}", archivedirectory, archivename));
                }
                else if (!skipEmptying)
                    File.Delete(filepath);
            }

            this.OnStatusChanged(new StatusChangedEventArgs("Emptied directories"));
        }

        /// <summary>
        /// Processes the requests.
        /// </summary>
        /// <param name="skipDownloading">Value that specifies whether to skip downloading files or not.</param>
        /// <param name="skipExtracting">Value that specifies whether to skip extracting files or not.</param>
        /// <param name="skipConverting">Value that specifies whether to skip converting files or not.</param>
        /// <param name="skipGenerating">Value that specifies whether to skip generating XML documents or not.</param>
        /// <param name="skipArchiving">Value that specifies whether to skip archiving files or not.</param>
        /// <param name="skipEmptying">Value that specifies whether to skip emptying working directories or not.</param>
        public virtual void ProcessRequests(bool skipDownloading,
                                            bool skipExtracting,
                                            bool skipConverting,
                                            bool skipGenerating,
                                            bool skipArchiving,
                                            bool skipEmptying)
        {
            this.DownloadFiles(skipDownloading);
            this.ExtractFiles(skipExtracting);

            if (this.ExistArhives(this.ExtractDirectory, this.ArchiveDirectory))
                return;

            this.ConvertEncodings(skipConverting);
            this.GenerateXmlDocuments(skipGenerating);

            if (!skipArchiving)
            {
                var timestamp = ConversionHelper.GetTimestampFromFilename(Directory.GetFiles(this.ExtractDirectory)
                                                                                   .Select(p => ConversionHelper.GetFilenameFromFilepath(p, this.Settings))
                                                                                   .First(),
                                                                          this.Settings);
                var archivedirectory = String.Format("{0}\\{1}", this.ArchiveDirectory, timestamp);
                if (!Directory.Exists(archivedirectory))
                    Directory.CreateDirectory(archivedirectory);

                this.ArchiveXmlDocuments(this.FilenameForArchive, this.ExtractDirectory, archivedirectory);
            }

            this.EmptyDirectories(skipArchiving, skipEmptying);
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
