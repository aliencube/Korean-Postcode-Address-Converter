using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FileHelpers;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Interfaces;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Models;
using Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services.Helpers;

namespace Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services
{
    /// <summary>
    /// This represents the update service factory entity.
    /// </summary>
    public class UpdaterServiceFactory
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the UpdateServiceFactory object.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        public UpdaterServiceFactory(Settings settings)
        {
            this._settings = settings;
        }
        #endregion

        #region Properties
        private readonly Settings _settings;
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
            var handler = this.StatusChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when status changed event is raised.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the status changed event.</param>
        private void Status_Changed(object sender, StatusChangedEventArgs e)
        {
            //  Bubbles up the event to the parent.
            var handler = this.StatusChanged;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// Occurs when exception thrown event is raised.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the exception thrown event.</param>
        private void Exception_Thrown(object sender, ExceptionThrownEventArgs e)
        {
            //  Bubbles up the event to the parent.
            var handler = this.ExceptionThrown;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// Called in read operations just after the record was created from a record string.
        /// </summary>
        /// <param name="engine">The engine that generates the event.</param>
        /// <param name="e">The event data.</param>
        private void Csv_AfterReadRecord(EngineBase engine, AfterReadRecordEventArgs<Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Models.StreetBasedAddress> e)
        {
            var street = e.Record.StreetName.Trim();
            var streetEng = e.Record.StreetNameEng.Trim();

            if (String.IsNullOrWhiteSpace(street))
                return;

            if (street == "0" && this._settings.StreetNameCorrections.ContainsKey(streetEng))
                street = this._settings.StreetNameCorrections[streetEng];
            else
            {
                foreach (var correction in this._settings
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
        /// Processes the update service.
        /// </summary>
        /// <param name="serviceType">Converter service type.</param>
        public void ProcessRequests(ConverterServiceType serviceType)
        {
            var service = this.GetService(serviceType);
            service.StatusChanged += Status_Changed;
            service.ExceptionThrown += Exception_Thrown;

            service.ProcessRequests(service.SkipDownloadingFiles,
                                    service.SkipExtractingFiles,
                                    service.SkipConvertingFiles,
                                    service.SkipGeneratingXmlDocuments,
                                    service.SkipArchivingXmlDocuments,
                                    true);

            if (!service.SkipLoadingDatabase)
                this.LoadDatabase(service.ExtractDirectory, serviceType);

            service.EmptyDirectories(service.SkipEmptyingDirectories);
        }

        /// <summary>
        /// Gets the converter service instance based on the converter service type.
        /// </summary>
        /// <param name="serviceType">Converter service type.</param>
        /// <returns>Returns the converter service instance based on the converter service type.</returns>
        private IBaseAddressService GetService(ConverterServiceType serviceType)
        {
            return BaseAddressService.Create(serviceType, this._settings);
        }

        /// <summary>
        /// Loads extracted XML files to database.
        /// </summary>
        /// <param name="sourceDirectory">Source directory where the XML files are located.</param>
        /// <param name="serviceType">Converter service type.</param>
        private void LoadDatabase(string sourceDirectory, ConverterServiceType serviceType)
        {
            using (var context = new KoreanPostcodeAddressDataContext())
            {
                //  Drops and recreates tables.
                context.DropCreateTables(Convert.ToString(serviceType));
            }

            IList<string> filepaths;
            switch (serviceType)
            {
                case ConverterServiceType.Lot:
                    filepaths = Directory.GetFiles(sourceDirectory).Where(p => p.EndsWith(".xml")).ToList();
                    this.LoadLotBasedAddresses(filepaths);
                    break;
                case ConverterServiceType.Street:
                    filepaths = Directory.GetFiles(sourceDirectory).Where(p => p.EndsWith(".txt")).ToList();
                    this.LoadStreetBasedAddresses(filepaths);
                    break;
            }
        }

        /// <summary>
        /// Loads LOT-based addresses to database.
        /// </summary>
        /// <param name="filepaths">List of XML document paths.</param>
        private void LoadLotBasedAddresses(IEnumerable<string> filepaths)
        {
            var serialiser = new XmlSerializer(typeof(LotBasedAddresses));
            foreach (var filepath in filepaths)
            {
                var filename = ConversionHelper.GetFilenameFromFilepath(filepath, this._settings);

                using (var context = new KoreanPostcodeAddressDataContext())
                {
                    this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Loading a file - {0} - to database", filename)));

                    using (var stream = new FileStream(filepath, FileMode.Open))
                    {
                        var collection = (LotBasedAddresses)serialiser.Deserialize(stream);
                        foreach (var address in collection.LotBasedAddress
                                                          .Select(p => new LotBasedAddress()
                                                              {
                                                                  Postcode = ConversionHelper.ConvertToString(p.Postcode),
                                                                  SequenceNumber = ConversionHelper.ConvertToInt32(p.SequenceNumber),
                                                                  Province = ConversionHelper.GetProvince(p.Province),
                                                                  City = ConversionHelper.GetCity(p.Province),
                                                                  County = ConversionHelper.GetCounty(p.Province, p.County),
                                                                  District = ConversionHelper.GetDistrict(p.Province, p.County),
                                                                  Suburb = ConversionHelper.GetSuburb(p.Suburb),
                                                                  Village = ConversionHelper.GetVillage(p.Village),
                                                                  Island = ConversionHelper.GetIsland(p.Island),
                                                                  San = ConversionHelper.GetSan(p.San),
                                                                  LotNumberMajorFrom = ConversionHelper.ConvertToNullableInt32(p.LotNumberMajorFrom),
                                                                  LotNumberMinorFrom = ConversionHelper.ConvertToNullableInt32(p.LotNumberMinorFrom),
                                                                  LotNumberMajorTo = ConversionHelper.ConvertToNullableInt32(p.LotNumberMajorTo),
                                                                  LotNumberMinorTo = ConversionHelper.ConvertToNullableInt32(p.LotNumberMinorTo),
                                                                  BuildingName = ConversionHelper.ConvertToString(p.BuildingName),
                                                                  BuildingNumberFrom = ConversionHelper.ConvertToNullableInt32(p.BuildingNumberFrom),
                                                                  BuildingNumberTo = ConversionHelper.ConvertToNullableInt32(p.BuildingNumberTo),
                                                                  DateUpdated = p.DateUpdated,
                                                                  Address = ConversionHelper.ConvertToString(p.Address)
                                                              }))
                        {
                            context.LotBasedAddresses.AddObject(address);
                        }
                    }
                    context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);

                    this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Loaded the file - {0} - to database", filename)));
                }
            }
        }

        /// <summary>
        /// Loads street-based addresses to database.
        /// </summary>
        /// <param name="filepaths">List of XML document paths.</param>
        private void LoadStreetBasedAddresses(IEnumerable<string> filepaths)
        {
            foreach (var filepath in filepaths)
            {
                var filename = ConversionHelper.GetFilenameFromFilepath(filepath, this._settings);

                using (var context = new KoreanPostcodeAddressDataContext())
                {
                    this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Loading a file - {0} - to database", filename)));

                    var csv = new DelimitedFileEngine<Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Models.StreetBasedAddress>() { Encoding = Encoding.GetEncoding(949) };
                    csv.AfterReadRecord += Csv_AfterReadRecord;
                    csv.Options.Delimiter = "|";
                    csv.Options.IgnoreFirstLines = 1;

                    //  Loads objects directly from the CSV file.
                    var addresses = csv.ReadFile(filepath);

                    var blockSize = this._settings.GetProcessRequests<int>("databaseLoadingBlockSize");
                    var block = (int) Math.Ceiling((double) addresses.Length/blockSize);
                    for (var i = 0; i < block; i++)
                    {
                        this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Loading a file - {0} - to database ({1}/{2})", filename, i + 1, block)));

                        var records = i < block - 2
                                          ? blockSize
                                          : addresses.Length - (i * block);
                        foreach (var address in addresses.Skip(i * (int)blockSize)
                                                         .Take((int)records)
                                                         .Select(p => new StreetBasedAddress()
                                                         {
                                                             Postcode = ConversionHelper.ConvertToString(p.Postcode),
                                                             SequenceNumber = ConversionHelper.ConvertToString(p.SequenceNumber),
                                                             Province = ConversionHelper.GetProvince(p.Province),
                                                             ProvinceEng = ConversionHelper.GetProvinceEng(p.ProvinceEng),
                                                             City = ConversionHelper.GetCity(p.Province),
                                                             CityEng = ConversionHelper.GetCityEng(p.ProvinceEng),
                                                             County = ConversionHelper.GetCounty(p.Province, p.County),
                                                             CountyEng = ConversionHelper.GetCountyEng(p.ProvinceEng, p.CountyEng),
                                                             District = ConversionHelper.GetDistrict(p.Province, p.County),
                                                             DistrictEng = ConversionHelper.GetDistrictEng(p.ProvinceEng, p.CountyEng),
                                                             Suburb = ConversionHelper.GetSuburb(p.Suburb),
                                                             SuburbEng = ConversionHelper.GetSuburbEng(p.SuburbEng),
                                                             StreetNameCode = ConversionHelper.ConvertToString(p.StreetNameCode),
                                                             StreetName = ConversionHelper.GetStreet(p.StreetName, p.StreetNameEng),
                                                             StreetNameEng = ConversionHelper.GetStreetEng(p.StreetNameEng),
                                                             Basement = ConversionHelper.ConvertToNullableInt32(p.Basement),
                                                             BuildingNumberMajor = ConversionHelper.ConvertToNullableInt32(p.BuildingNumberMajor),
                                                             BuildingNumberMinor = ConversionHelper.ConvertToNullableInt32(p.BuildingNumberMinor),
                                                             BuildingCode = ConversionHelper.ConvertToString(p.BuildingCode),
                                                             BuildingNameForBulk = ConversionHelper.ConvertToString(p.BuildingNameForBulk),
                                                             BuildingName = ConversionHelper.ConvertToString(p.BuildingName),
                                                             RegisteredSuburbCode = ConversionHelper.ConvertToString(p.RegisteredSuburbCode),
                                                             RegisteredSuburb = ConversionHelper.GetSuburb(p.RegisteredSuburb),
                                                             SuburbSequenceNumber = ConversionHelper.ConvertToNullableInt32(p.SuburbSequenceNumber),
                                                             Village = ConversionHelper.GetVillage(p.Village),
                                                             San = ConversionHelper.ConvertToNullableBoolean(p.San),
                                                             LotNumberMajor = ConversionHelper.ConvertToNullableInt32(p.LotNumberMajor),
                                                             LotNumberMinor = ConversionHelper.ConvertToNullableInt32(p.LotNumberMinor)
                                                         }))
                        {
                            context.StreetBasedAddresses.AddObject(address);
                        }
                        context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);

                        this.OnStatusChanged(new StatusChangedEventArgs(String.Format("Loaded the file - {0} - to database ({1}/{2})", filename, i + 1, block)));
                    }
                }
            }
        }
        #endregion
    }
}
