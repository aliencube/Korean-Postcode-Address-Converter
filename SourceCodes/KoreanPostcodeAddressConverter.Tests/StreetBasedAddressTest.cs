using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Tests
{
    /// <summary>
    /// A test class for StreetBasedAddress class.
    /// </summary>
    [TestFixture]
    public class StreetBasedAddressTest
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

            //if (Directory.GetFiles(this._extracts).Any())
            //    foreach (var file in Directory.GetFiles(this._extracts))
            //        File.Delete(file);

            this._filenames = this._settings
                                  .GetFilenamesToDownloadOrExtract(this._settings
                                                                       .ConversionSettings
                                                                       .StreetBasedAddress
                                                                       .Filenames);

            this._unzippath = this._settings.UnzipPath;
            this._zipfilename = this._settings
                                    .ConversionSettings
                                    .StreetBasedAddress
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

            var service = new StreetBasedAddressService(this._settings);
            service.DownloadFiles(false);

            foreach (var filename in this._filenames)
                Assert.IsTrue(File.Exists(String.Format("{0}\\{1}", this._downloads, filename)));

        }

        /// <summary>
        /// Tests to extract files from zip files.
        /// </summary>
        [Test]
        public void _200_ExtractFiles_SendFilenames_StoreTxtFiles()
        {
            var service = new StreetBasedAddressService(this._settings);
            service.ExtractFiles(false);

            //Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".csv")) > 0);
            Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".txt")) > 0);
        }

        /// <summary>
        /// Test the extracted files are converted their encoding from cp949 to utf8.
        /// </summary>
        [Test]
        public void _300_ConvertEncodings_SetFilenames_StoreConvertedFiles()
        {
            Assert.Pass("Not necessary");

            //var service = new StreetBasedAddressService(this._settings);

            //if (!Directory.GetFiles(this._extracts).Any(p => p.EndsWith(".csv")))
            //    service.ExtractFiles();

            //service.ConvertEncodings();

            //Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".csv")) > 0);
            //Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".txt")) > 0);
        }

        /// <summary>
        /// Tests the text files are converted to XML documents.
        /// </summary>
        [Test]
        public void _400_GenerateXmlDocuments_SendFilenames_StoreXmlDocuments()
        {
            var service = new StreetBasedAddressService(this._settings);
            service.GenerateXmlDocuments(false);

            Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".xml")) > 0);
        }

        /// <summary>
        /// Tests XML documents are zipped.
        /// </summary>
        [Test]
        public void _500_ArchiveXmlDocuments_SendFilenames_StoreZipFile()
        {
            var service = new StreetBasedAddressService(this._settings);
            service.ArchiveXmlDocuments(false, this._zipfilename, this._extracts, this._extracts);

            Assert.IsTrue(Directory.GetFiles(this._extracts).Count(p => p.EndsWith(".zip")) == 1);
        }
    }
}
