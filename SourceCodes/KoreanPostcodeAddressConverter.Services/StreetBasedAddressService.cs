using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FileHelpers;

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
        {
            this._settings = settings;
        }
        #endregion

        #region Properties
        private readonly Settings _settings;

        private string _downloadUrl;
        /// <summary>
        /// Gets the download URL.
        /// </summary>
        public override string DownloadUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_downloadUrl))
                    _downloadUrl = this._settings
                                       .GetDownloadUrl(this._settings
                                                           .ConversionSettings
                                                           .StreetBasedAddress
                                                           .DownloadUrl);
                return _downloadUrl;
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
                    _downloadDirectory = this._settings
                                             .GetAbsoluteDirectory(this._settings
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
                    _extractDirectory = this._settings
                                            .GetAbsoluteDirectory(this._settings
                                                                      .ConversionSettings
                                                                      .StreetBasedAddress
                                                                      .ExtractDirectory);
                if (!Directory.Exists(_extractDirectory))
                    Directory.CreateDirectory(_extractDirectory);

                return _extractDirectory;
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
                    _filenamesToDownloadOrExtract = this._settings
                                                        .GetFilenamesToDownloadOrExtract(this._settings
                                                                                             .ConversionSettings
                                                                                             .StreetBasedAddress
                                                                                             .Filenames);
                return _filenamesToDownloadOrExtract;
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
            this.UnzipZipFiles(this.FilenamesToDownloadOrExtract, this.DownloadDirectory, this.ExtractDirectory);
        }

        /// <summary>
        /// Converts files extracted to appropriate encoding.
        /// </summary>
        public override void ConvertEncodings()
        {
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
        }

        /// <summary>
        /// Gets XML documents from the extracted and converted files with appropriate encoding.
        /// </summary>
        public override void GetXmlDocuments()
        {
            var csv = new DelimitedFileEngine<StreetBasedAddress>() { Encoding = Encoding.GetEncoding(949) };
            csv.Options.Delimiter = "|";
            csv.Options.IgnoreFirstLines = 1;

            //foreach (var filepath in Directory.GetFiles(this.ExtractDirectory).Where(p => p.EndsWith(".csv")))
            foreach (var filepath in Directory.GetFiles(this.ExtractDirectory).Where(p => p.EndsWith(".txt")))
            {
                var address = csv.ReadFile(filepath);
                var addresses = new StreetBasedAddresses() { StreetBasedAddress = address };

                //using (var writer = new XmlTextWriter(filepath.Replace(".csv", ".xml"), Encoding.UTF8) { Formatting = Formatting.Indented })
                using (var writer = new XmlTextWriter(filepath.Replace(".txt", ".xml"), Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    var serialiser = new XmlSerializer(typeof(StreetBasedAddresses));
                    serialiser.Serialize(writer, addresses);
                }
            }
        }
        #endregion
    }
}
