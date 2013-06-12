using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration;
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

        #region Methods
        /// <summary>
        /// Downloads files.
        /// </summary>
        public override void DownloadFiles()
        {
            using (var client = new WebClient())
            {
                foreach (var filename in this.FilenamesToDownloadOrExtract)
                    client.DownloadFile(String.Format("{0}/{1}", this.DownloadUrl, filename),
                                        String.Format("{0}\\{1}", this.DownloadDirectory, filename));
            }
        }

        /// <summary>
        /// Extracts files downloaded.
        /// </summary>
        public override void ExtractFiles()
        {
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
            foreach (var filepath in Directory.GetFiles(this.ExtractDirectory).Where(p => p.EndsWith(".xls")))
            {
                var segments = filepath.Split(this.Settings
                                                  .ConversionSettings
                                                  .SegmentSeparatorForDirectory
                                                  .Delimiters
                                                  .ToCharArray(),
                                              StringSplitOptions.RemoveEmptyEntries);
                var filename = segments[segments.Length - 1];
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

            var sheet = this.Settings
                            .ConversionSettings
                            .LotBasedAddress
                            .ExcelWorksheetName
                            .Worksheet;

            var filepath = Directory.GetFiles(this.ExtractDirectory)
                                    .Single(p => p.EndsWith(".xls") &&
                                                 p.Contains(maps.Single(q => q.Conversion).Replace));

            var segments = filepath.Split(this.Settings
                                              .ConversionSettings
                                              .SegmentSeparatorForFile
                                              .Delimiters
                                              .ToCharArray(),
                                          StringSplitOptions.RemoveEmptyEntries);

            var extension = segments[segments.Length - 1];

            var connectionString = String.Format("provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel {1}.0; HDR=NO; IMEX=1;\"",
                                                 filepath,
                                                 (extension.ToLower() == "xls" ? "8" : "12"));
            using (var dt = new DataTable())
            using (var connection = new OleDbConnection(connectionString))
            {
                var adapter = new OleDbDataAdapter("SELECT * FROM [" + sheet + "$]", connection);
                adapter.Fill(dt);

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
        }
        #endregion
    }
}
