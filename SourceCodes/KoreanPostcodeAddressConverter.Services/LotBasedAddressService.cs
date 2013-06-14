using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Excel;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services
{
    /// <summary>
    /// This represents the LOT based address service entity.
    /// </summary>
    public class LotBasedAddressService : BaseAddressService
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the LotBasedAddressService object.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        public LotBasedAddressService(Settings settings)
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
                                                           .LotBasedAddress
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
                                                                                             .LotBasedAddress
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
                                                                       .LotBasedAddress
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
                                                                      .LotBasedAddress
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
                                                                      .LotBasedAddress
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
                                                                         .LotBasedAddress
                                                                         .ArchiveFilename);
                return _filenameForArchive;
            }
        }
        #endregion

        #region Events
        #endregion

        #region Methods
        /// <summary>
        /// Downloads files.
        /// </summary>
        public override void DownloadFiles()
        {
            using (var client = new WebClient())
            {
                foreach (var filename in this.FilenamesToDownloadOrExtract)
                {
                    this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Downloading a file - {0}", filename)));

                    client.DownloadFile(String.Format("{0}/{1}", this.DownloadUrl, filename),
                                            String.Format("{0}\\{1}", this.DownloadDirectory, filename));

                    this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Downloaded the file - {0}", filename)));
                }
            }
        }

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        public override void ExtractFiles()
        {
            this.OnStatusChanged(new StatusChangeEventArgs("Extracting files"));

            //  Extracts .zip files.
            this.UnzipZipFiles(this.FilenamesToDownloadOrExtract, this.DownloadDirectory);

            //  Extracts self-extracting .exe files.
            var unzippath = this.Settings
                                .GetUnzipPath(this.Settings
                                                  .ConversionSettings
                                                  .UnzipPath);
            this.UnzipSfxFiles(this.FilenamesToDownloadOrExtract.Select(p => p.Replace(".zip", ".exe")),
                               unzippath,
                               this.DownloadDirectory,
                               this.ExtractDirectory);

            this.OnStatusChanged(new StatusChangeEventArgs("Extracted files"));
        }

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        public override void ConvertEncodings()
        {
            //  Renames the extracted .xls files.
            var maps = this.Settings
                           .ConversionSettings
                           .LotBasedAddress
                           .FilenameMappings
                           .Cast<FilenameMappingElement>()
                           .ToList();
            var filepaths = Directory.GetFiles(this.ExtractDirectory)
                                     .Where(p => p.EndsWith(".xls"));
            foreach (var filepath in filepaths)
            {
                var segments = filepath.Split(this.Settings
                                                  .ConversionSettings
                                                  .SegmentSeparatorForDirectory
                                                  .Delimiters
                                                  .ToCharArray(),
                                              StringSplitOptions.RemoveEmptyEntries);
                var filename = segments[segments.Length - 1];

                this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Converting a file - {0}", filename)));

                foreach (var map in maps)
                {
                    var converted = filename.Contains(map.Search) ? filepath.Replace(map.Search, map.Replace) : null;
                    if (String.IsNullOrWhiteSpace(converted))
                        continue;

                    if (File.Exists(converted))
                        File.Delete(converted);
                    File.Move(filepath, converted);
                    break;
                }

                this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Converted the file - {0}", filename)));
            }
        }

        /// <summary>
        /// Gets XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        public override void GetXmlDocuments()
        {
            var maps = this.Settings
                           .ConversionSettings
                           .LotBasedAddress
                           .FilenameMappings
                           .Cast<FilenameMappingElement>()
                           .ToList();

            var filepath = Directory.GetFiles(this.ExtractDirectory)
                                    .Single(p => p.EndsWith(".xls") &&
                                                 p.Contains(maps.Single(q => q.Conversion).Replace));

            var segments = filepath.Split(this.Settings
                                              .ConversionSettings
                                              .SegmentSeparatorForDirectory
                                              .Delimiters
                                              .ToCharArray(),
                                          StringSplitOptions.RemoveEmptyEntries);
            var filename = segments[segments.Length - 1];

            this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Generating an XML document from - {0}", filename)));

            using (var stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateBinaryReader(stream))
            using (var ds = reader.AsDataSet())
            using (var dt = ds.Tables[0])
            {
                var addresses = new List<LotBasedAddress>();
                foreach (var dr in dt.Rows.Cast<DataRow>().Skip(2))
                {
                    addresses.Add(new LotBasedAddress()
                    {
                        Postcode = Convert.ToString(dr[0]),
                        SequenceNumber = Convert.ToInt32(dr[1]),
                        Province = Convert.ToString(dr[2]),
                        County = Convert.ToString(dr[3]),
                        Suburb = Convert.ToString(dr[4]),
                        Village = Convert.ToString(dr[5]),
                        Island = Convert.ToString(dr[6]),
                        San = ConversionHelper.ConvertToBoolean(dr[7]),
                        LotNumberMajorFrom = ConversionHelper.ConvertToNullableInt32(dr[8]),
                        LotNumberMajorTo = ConversionHelper.ConvertToNullableInt32(dr[9]),
                        LotNumberMinorFrom = ConversionHelper.ConvertToNullableInt32(dr[10]),
                        LotNumberMinorTo = ConversionHelper.ConvertToNullableInt32(dr[11]),
                        BuildingName = Convert.ToString(dr[12]),
                        BuildingNumberFrom = ConversionHelper.ConvertToNullableInt32(dr[13]),
                        BuildingNumberTo = ConversionHelper.ConvertToNullableInt32(dr[14]),
                        DateUpdated = ConversionHelper.ConvertToDateTime(dr[15]),
                        Address = Convert.ToString(dr[16])
                    });
                }

                var address = new LotBasedAddresses() { LotBasedAddress = addresses.ToArray() };
                using (var writer = new XmlTextWriter(filepath.Replace(".xls", ".xml"), Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    var serialiser = new XmlSerializer(typeof(LotBasedAddresses));
                    serialiser.Serialize(writer, address);
                }
            }

            this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Generated the XML document - {0}", filename.Replace(".xls", ".xml"))));
        }

        /// <summary>
        /// Loads objects to database.
        /// </summary>
        /// <param name="sourceDirectory">Source directory where files for archive are located.</param>
        public override void LoadDatabase(string sourceDirectory)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
