using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FileHelpers;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Models;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services
{
    /// <summary>
    /// This represents the street based address service entity.
    /// </summary>
    public class StreetBasedAddressService : BaseAddressService
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the StreetBasedAddressService object.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        public StreetBasedAddressService(Settings settings)
            : base(settings)
        {
        }
        #endregion

        #region Properties
        private string _downloadUrl;
        /// <summary>
        /// Gets the download URL.
        /// </summary>
        public override string DownloadUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_downloadUrl))
                    _downloadUrl = this.Settings
                                       .GetDownloadUrl(this.Settings
                                                           .ConversionSettings
                                                           .StreetBasedAddress
                                                           .DownloadUrl);
                return _downloadUrl;
            }
        }

        private IList<string> _filenamesToDownloadOrExtract;
        /// <summary>
        /// Gets the list of filenames to download or extract.
        /// </summary>
        public override IList<string> FilenamesToDownloadOrExtract
        {
            get
            {
                if (_filenamesToDownloadOrExtract == null || !_filenamesToDownloadOrExtract.Any())
                    _filenamesToDownloadOrExtract = this.Settings
                                                        .GetFilenamesToDownloadOrExtract(this.Settings
                                                                                             .ConversionSettings
                                                                                             .StreetBasedAddress
                                                                                             .Filenames);
                return _filenamesToDownloadOrExtract;
            }
        }

        private string _downloadDirectory;
        /// <summary>
        /// Gets the directory to download files.
        /// </summary>
        public override string DownloadDirectory
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_downloadDirectory))
                    _downloadDirectory = this.Settings
                                             .GetAbsoluteDirectory(this.Settings
                                                                       .ConversionSettings
                                                                       .StreetBasedAddress
                                                                       .DownloadDirectory);
                if (!Directory.Exists(_downloadDirectory))
                    Directory.CreateDirectory(_downloadDirectory);

                return _downloadDirectory;
            }
        }

        private string _extractDirectory;
        /// <summary>
        /// Gets the directory to extract files.
        /// </summary>
        public override string ExtractDirectory
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_extractDirectory))
                    _extractDirectory = this.Settings
                                            .GetAbsoluteDirectory(this.Settings
                                                                      .ConversionSettings
                                                                      .StreetBasedAddress
                                                                      .ExtractDirectory);
                if (!Directory.Exists(_extractDirectory))
                    Directory.CreateDirectory(_extractDirectory);

                return _extractDirectory;
            }
        }

        private string _archiveDirectory;
        /// <summary>
        /// Gets the directory to archive files.
        /// </summary>
        public override string ArchiveDirectory
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_archiveDirectory))
                    _archiveDirectory = this.Settings
                                            .GetAbsoluteDirectory(this.Settings
                                                                      .ConversionSettings
                                                                      .StreetBasedAddress
                                                                      .ArchiveDirectory);
                if (!Directory.Exists(_archiveDirectory))
                    Directory.CreateDirectory(_archiveDirectory);

                return _archiveDirectory;
            }
        }

        private string _filenameForArchive;
        /// <summary>
        /// Gets the filename for archive.
        /// </summary>
        public override string FilenameForArchive
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_filenameForArchive))
                    _filenameForArchive = this.Settings
                                              .GetFilenameForArchive(this.Settings
                                                                         .ConversionSettings
                                                                         .StreetBasedAddress
                                                                         .ArchiveFilename);
                return _filenameForArchive;
            }
        }
        #endregion

        #region Events
        #endregion

        #region Event Handlers
        /// <summary>
        /// Occurs when an asynchronous download operation successfully transfers some or all of the data.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the download progress changed event.</param>
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage % 5 != 0)
                return;

            this.OnStatusChanged(new StatusChangedEventArgs(" ."));
        }

        /// <summary>
        /// Occurs when an asynchronous file download operation completes.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the download completed event.</param>
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var progress = e.UserState as StatusProgress;
            if (progress == null)
                return;

            this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Downloaded - {0}", progress.Filename)));

            var index = this.FilenamesToDownloadOrExtract.IndexOf(progress.Filename);
            if (index == this.FilenamesToDownloadOrExtract.Count - 1)
                return;

            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;

                var filename = this.FilenamesToDownloadOrExtract[index + 1];
                this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Downloading - {0}", filename)));

                client.DownloadFileAsync(new Uri(String.Format("{0}/{1}", this.DownloadUrl, filename)),
                                         String.Format("{0}\\{1}", this.DownloadDirectory, filename),
                                         new StatusProgress(filename));
            }
        }

        /// <summary>
        /// Called in read operations just after the record was created from a record string.
        /// </summary>
        /// <param name="engine">The engine that generates the event.</param>
        /// <param name="e">The event data.</param>
        private void Csv_AfterReadRecord(EngineBase engine, AfterReadRecordEventArgs<StreetBasedAddress> e)
        {
            var street = e.Record.StreetName.Trim();
            var streetEng = e.Record.StreetNameEng.Trim();

            if (String.IsNullOrWhiteSpace(street))
                return;

            if (street == "0" && this.Settings.StreetNameCorrections.ContainsKey(streetEng))
                street = this.Settings.StreetNameCorrections[streetEng];
            else
            {
                foreach (var correction in this.Settings
                                               .StreetNameCorrections
                                               .Where(p => street.StartsWith(p.Key)))
                {
                    street = street.Replace(correction.Key, correction.Value);
                }
            }

            e.Record.StreetName = street;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Downloads files.
        /// </summary>
        /// <param name="skipDownloading">Value that specifies whether to skip downloading files or not.</param>
        public override void DownloadFiles(bool skipDownloading)
        {
            if (skipDownloading)
            {
                this.OnStatusChanged(new StatusChangedEventArgs("Download skipped"));
                return;
            }

            //using (var client = new WebClient())
            //{
            //    client.DownloadProgressChanged += Client_DownloadProgressChanged;
            //    client.DownloadFileCompleted += Client_DownloadFileCompleted;

            //    var filename = this.FilenamesToDownloadOrExtract[0];

            //    this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Downloading - {0}", filename)));

            //    client.DownloadFileAsync(new Uri(String.Format("{0}/{1}", this.DownloadUrl, filename)),
            //                             String.Format("{0}\\{1}", this.DownloadDirectory, filename),
            //                             new StatusProgress(filename));
            //}

            using (var client = new WebClient())
            {
                if (this.ProxyServer.Use)
                    client.Proxy = new WebProxy(this.ProxyServer.Host, this.ProxyServer.Port);

                foreach (var filename in this.FilenamesToDownloadOrExtract)
                {
                    this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Downloading a file - {0}", filename)));

                    client.DownloadFile(String.Format("{0}/{1}", this.DownloadUrl, filename),
                                            String.Format("{0}\\{1}", this.DownloadDirectory, filename));

                    this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Downloaded the file - {0}", filename)));
                }
            }
        }

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        /// <param name="skipExtracting">Value that specifies whether to skip extracting files or not.</param>
        public override void ExtractFiles(bool skipExtracting)
        {
            if (skipExtracting)
            {
                this.OnStatusChanged(new StatusChangedEventArgs("Extraction skipped"));
                return;
            }

            this.OnStatusChanged(new StatusChangedEventArgs("Extracting files"));

            this.UnzipZipFiles(this.FilenamesToDownloadOrExtract, this.DownloadDirectory, this.ExtractDirectory);

            this.OnStatusChanged(new StatusChangedEventArgs("Extracted files"));
        }

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        /// <param name="skipConverting">Value that specifies whether to skip converting files or not.</param>
        public override void ConvertEncodings(bool skipConverting)
        {
            if (skipConverting)
            {
                this.OnStatusChanged(new StatusChangedEventArgs("Conversion skipped"));
                return;
            }

            this.OnStatusChanged(new StatusChangedEventArgs());

            //foreach (var filepath in Directory.GetFiles(this.ExtractDirectory).Where(p => p.EndsWith(".txt")))
            //{
            //    var cp949 = Encoding.GetEncoding(949);
            //    using (var input = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            //    {
            //        var buffer = new byte[input.Length];
            //        var read = 0;
            //        while (read < buffer.Length)
            //            read += input.Read(buffer, read, buffer.Length - read);

            //        var converted = Encoding.Convert(cp949, Encoding.UTF8, buffer);
            //        using (var output = new FileStream(filepath.Replace(".txt", ".csv"), FileMode.Create, FileAccess.Write))
            //        {
            //            output.Write(converted, 0, converted.Length);
            //        }
            //    }

            //    using (var reader = new StreamReader(filepath, cp949))
            //    using (var writer = new StreamWriter(filepath.Replace(".txt", ".csv"), false, Encoding.UTF8))
            //    {
            //        writer.Write(reader.ReadToEnd());
            //    }
            //}

            this.OnStatusChanged(new StatusChangedEventArgs());
        }

        /// <summary>
        /// Generates XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        /// <param name="skipGenerating">Value that specifies whether to skip generating XML documents or not.</param>
        public override void GenerateXmlDocuments(bool skipGenerating)
        {
            if (skipGenerating)
            {
                this.OnStatusChanged(new StatusChangedEventArgs("XML generation skipped"));
                return;
            }

            var csv = new DelimitedFileEngine<StreetBasedAddress>() { Encoding = Encoding.GetEncoding(949) };
            csv.AfterReadRecord += Csv_AfterReadRecord;
            csv.Options.Delimiter = "|";
            csv.Options.IgnoreFirstLines = 1;

            //foreach (var filepath in Directory.GetFiles(this.ExtractDirectory).Where(p => p.EndsWith(".csv")))
            foreach (var filepath in Directory.GetFiles(this.ExtractDirectory).Where(p => p.EndsWith(".txt")))
            {
                var filename = ConversionHelper.GetFilenameFromFilepath(filepath, this.Settings);

                this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Generating an XML document from - {0}", filename)));

                var address = csv.ReadFile(filepath);
                var addresses = new StreetBasedAddresses() { StreetBasedAddress = address };

                //using (var writer = new XmlTextWriter(filepath.Replace(".csv", ".xml"), Encoding.UTF8) { Formatting = Formatting.Indented })
                using (var writer = new XmlTextWriter(filepath.Replace(".txt", ".xml"), Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    var serialiser = new XmlSerializer(typeof(StreetBasedAddresses));
                    serialiser.Serialize(writer, addresses);
                }

                //this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Generated the XML document - {0}", filename.Replace(".csv", ".xml"))));
                this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Generated the XML document - {0}", filename.Replace(".txt", ".xml"))));
            }
        }

        /// <summary>
        /// Loads objects to database.
        /// </summary>
        /// <param name="skipLoading">Value that specifies whether to skip loading XML documents to database or not.</param>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        /// <param name="blockSize">Number of records to load to database at once.</param>
        public override void LoadDatabase(bool skipLoading, string sourceDirectory, int blockSize)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
