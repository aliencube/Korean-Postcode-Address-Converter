using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers;
using Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Tests
{
    /// <summary>
    /// A test class for the UpdaterServiceFactory class.
    /// </summary>
    [TestFixture]
    public class UpdaterServiceFactoryTest
    {
        private Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services.Settings _settings;
        private string _source;
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
            this._settings = Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services.Settings.Instance;

            this._source = ConfigurationManager.AppSettings["Test.DownloadSourceDirectory"];
            if (!Directory.Exists(this._source))
                Directory.CreateDirectory(this._source);

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
                                                                       .LotBasedAddress
                                                                       .Filenames);

            this._unzippath = this._settings.UnzipPath;
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
        /// Tests LOT-based address service.
        /// </summary>
        [Test]
        public void ProcessRequests_SendLotBasedServiceType_StoreLotBasedAddressResults()
        {
            if (Directory.GetFiles(this._downloads).Any())
                foreach (var file in Directory.GetFiles(this._downloads))
                    File.Delete(file);

            if (Directory.GetFiles(this._extracts).Any())
                foreach (var file in Directory.GetFiles(this._extracts))
                    File.Delete(file);

            File.Copy(String.Format("{0}\\koreapost_zipcode_DB.zip", this._source),
                      String.Format("{0}\\koreapost_zipcode_DB.zip", this._downloads));

            var factory = new UpdaterServiceFactory(this._settings);
            factory.ProcessRequests(ConverterServiceType.Lot);
        }

        /// <summary>
        /// Tests LOT-based address service.
        /// </summary>
        [Test]
        public void ProcessRequests_SendStreetBasedServiceType_StoreStreetBasedAddressResults()
        {
            if (Directory.GetFiles(this._downloads).Any())
                foreach (var file in Directory.GetFiles(this._downloads))
                    File.Delete(file);

            if (Directory.GetFiles(this._extracts).Any())
                foreach (var file in Directory.GetFiles(this._extracts))
                    File.Delete(file);

            foreach (var filepath in Directory.GetFiles(this._source)
                                              .Where(p => p.EndsWith(".zip") && !p.StartsWith("korea")))
            {
                var filename = ConversionHelper.GetFilenameFromFilepath(filepath, this._settings);
                File.Copy(filepath, String.Format("{0}\\{1}", this._downloads, filename));
            }

            var factory = new UpdaterServiceFactory(this._settings);
            factory.ProcessRequests(ConverterServiceType.Street);
        }
    }
}
