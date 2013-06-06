using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Tests
{
    /// <summary>
    /// A test class for LotBasedAddress class.
    /// </summary>
    [TestFixture]
    public class LotBasedAddressTest
    {
        private Settings _settings;
        private string _downloads;
        private string _extracts;
        private string _unzippath;
        private IList<string> _filenames;
        private string _zipfilename;

        #region Setup and Tear down
        /// <summary>
        /// This runs only once at the beginning of all tests and is used for all tests in the 
        /// class.
        /// </summary>
        [TestFixtureSetUp]
        public void InitialSetup()
        {
            this._settings = Settings.Instance;

            this._downloads = ConfigurationManager.AppSettings["Test.DownloadDirectory"];
            if (!Directory.Exists(this._downloads))
                Directory.CreateDirectory(this._downloads);

            this._extracts = ConfigurationManager.AppSettings["Test.ExtractDirectory"];
            if (!Directory.Exists(this._extracts))
                Directory.CreateDirectory(this._extracts);

            if (Directory.GetFiles(this._extracts).Any())
                foreach (var file in Directory.GetFiles(this._extracts))
                    File.Delete(file);

            this._filenames = this._settings
                                  .GetFilenamesToDownloadOrExtract(this._settings
                                                                       .ConversionSettings
                                                                       .LotBasedAddress
                                                                       .Filenames);

            this._unzippath = this._settings
                                  .GetUnzipPath(this._settings
                                                    .ConversionSettings
                                                    .UnzipPath);
            this._zipfilename = this._settings
                                    .ConversionSettings
                                    .LotBasedAddress
                                    .ArchiveFilename
                                    .Filename;
        }

        /// <summary>
        /// This runs only once at the end of all tests and is used for all tests in the class.
        /// </summary>
        [TestFixtureTearDown]
        public void FinalTearDown()
        {

        }

        /// <summary>
        /// This setup funcitons runs before each test method
        /// </summary>
        [SetUp]
        public void SetupForEachTest()
        {
        }

        /// <summary>
        /// This setup funcitons runs after each test method
        /// </summary>
        [TearDown]
        public void TearDownForEachTest()
        {
        }
        #endregion

        /// <summary>
        /// Tests the zip files are downloaded.
        /// </summary>
        [Test]
        public void _100_DownloadFiles_SendUrls_DownloadFiles()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["Test.Download"]))
                Assert.Pass("Download ignored");

            var service = new LotBasedAddressService(this._settings);
            service.DownloadFiles();

            foreach (var filename in this._filenames)
                Assert.IsTrue(File.Exists(String.Format("{0}\\{1}", this._downloads, filename)));
        }

        /// <summary>
        /// Tests to extract files from zip files.
        /// </summary>
        [Test]
        public void _200_UnzipFiles_SendFilenames_StoreExeOnly()
        {
            var service = new LotBasedAddressService(this._settings);
            var mi = service.GetType().GetMethod("UnzipZipFiles", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(service, new object[] { this._filenames, this._downloads, null });

            Assert.IsTrue(Directory.GetFiles(this._downloads).Count(p => p.EndsWith(".exe")) == 1);
        }

        /// <summary>
        /// Test the self extracting zip files.
        /// </summary>
        [Test]
        public void _300_UnzipSfxFiles_SendFilenames_StoreXlsOnly()
        {
            var filenames = this._filenames
                                .Select(p => p.Replace(".zip", ".exe"))
                                .ToList();

            var service = new LotBasedAddressService(this._settings);
            var mi = service.GetType().GetMethod("UnzipSfxFiles", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(service, new object[] { filenames, this._unzippath, this._downloads, this._extracts });

            Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".xls")) > 0);
        }

        /// <summary>
        /// Test the extracted files are renamed.
        /// </summary>
        [Test]
        public void _400_RenameExcelFiles_SendFilenames_RenameExcelFiles()
        {
            var maps = this._settings
                           .ConversionSettings
                           .LotBasedAddress
                           .FilenameMappings
                           .Cast<FilenameMappingElement>()
                           .ToList();

            var service = new LotBasedAddressService(this._settings);

            if (!Directory.GetFiles(this._extracts).Any(p => p.EndsWith(".xls")))
            {
                var filenames = this._filenames
                                    .Select(p => p.Replace(".zip", ".exe"))
                                    .ToList();
                var mi = service.GetType().GetMethod("UnzipSfxFiles", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(service, new object[] { filenames, this._unzippath, this._downloads, this._extracts });
            }

            service.ConvertEncodings();

            var count = maps.Count(map => Directory.GetFiles(this._extracts)
                                                   .Count(p => p.Contains(map.Replace) && p.EndsWith(".xls")) == 1);
            Assert.IsTrue(count == maps.Count);
        }

        /// <summary>
        /// Tests the Excel files are converted to XML documents.
        /// </summary>
        [Test]
        public void _500_GetXmlDocuments_SendFilenames_StoreXmlDocuments()
        {
            var maps = this._settings
                           .ConversionSettings
                           .LotBasedAddress
                           .FilenameMappings
                           .Cast<FilenameMappingElement>()
                           .ToList();

            var filepath = Directory.GetFiles(this._extracts)
                                    .Single(p => p.EndsWith(".xls") &&
                                                 p.Contains(maps.Single(q => q.Conversion).Replace));

            var service = new LotBasedAddressService(this._settings);
            if (!Directory.GetFiles(this._extracts).Any(p => p.EndsWith(".xls")))
            {
                var filenames = this._filenames
                                    .Select(p => p.Replace(".zip", ".exe"))
                                    .ToList();
                var mi = service.GetType().GetMethod("UnzipSfxFiles", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(service, new object[] { filenames, this._unzippath, this._downloads, this._extracts });

                service.ConvertEncodings();
            }

            service.GetXmlDocuments();

            Assert.IsTrue(File.Exists(filepath.Replace(".xls", ".xml")));
        }

        /// <summary>
        /// Tests XML documents are zipped.
        /// </summary>
        [Test]
        public void _600_ZipXmlDocuments_SendFilenames_StoreZipFile()
        {
            var service = new StreetBasedAddressService(this._settings);

            if (!Directory.GetFiles(this._extracts).Any(p => p.EndsWith(".xml")))
            {
                if (!Directory.GetFiles(this._extracts).Any(p => p.EndsWith(".xls")))
                {
                    var filenames = this._filenames
                                        .Select(p => p.Replace(".zip", ".exe"))
                                        .ToList();
                    var mi = service.GetType().GetMethod("UnzipSfxFiles", BindingFlags.Instance | BindingFlags.NonPublic);
                    mi.Invoke(service, new object[] { filenames, this._unzippath, this._downloads, this._extracts });

                    service.ConvertEncodings();
                }

                service.GetXmlDocuments();
            }

            service.ZipXmlDocuments(this._zipfilename, this._extracts, this._extracts);

            Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".zip")) == 1);
        }
    }
}
