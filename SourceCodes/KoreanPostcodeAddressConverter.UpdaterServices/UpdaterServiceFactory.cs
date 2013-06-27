﻿using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Interfaces;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Models;

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
                                    service.SkipLoadingDatabase ? service.SkipEmptyingDirectories : true);

            if (service.SkipLoadingDatabase)
                return;

            switch (serviceType)
            {
                case ConverterServiceType.Lot:
                    this.LoadDatabase(service.ArchiveDirectory, serviceType);
                    break;
                case ConverterServiceType.Street:
                    this.LoadDatabase(service.ExtractDirectory, serviceType);
                    break;
            }
            service.EmptyDirectories(service.SkipArchivingXmlDocuments, false);
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
                    var archiveDirectory = Directory.GetDirectories(sourceDirectory)
                                                    .OrderBy(p => p)
                                                    .Last();
                    filepaths = Directory.GetFiles(archiveDirectory).Where(p => p.EndsWith(".xml")).ToList();
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
        /// <param name="context">Database context.</param>
        /// <param name="filepaths">List of XML document paths.</param>
        private void LoadLotBasedAddresses(KoreanPostcodeAddressDataContext context, IEnumerable<string> filepaths)
        {
            var serialiser = new XmlSerializer(typeof(LotBasedAddresses));
            foreach (var filepath in filepaths)
            {
                var segments = filepath.Split(this._settings
                                                  .ConversionSettings
                                                  .SegmentSeparatorForDirectory
                                                  .Delimiters
                                                  .ToCharArray(),
                                              StringSplitOptions.RemoveEmptyEntries);
                var filename = segments[segments.Length - 1];

                this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Loading a file - {0} - to database", filename)));

                using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    var collection = (LotBasedAddresses)serialiser.Deserialize(stream);
                    foreach (var address in collection.LotBasedAddress
                                                      .Select(p => new LotBasedAddress()
                                                      {
                                                          Postcode = p.Postcode,
                                                          Address = p.Address,
                                                          Province = p.Province,
                                                          County = p.County,
                                                          Suburb = p.Suburb,
                                                          Village = p.Village,
                                                          Island = p.Island,
                                                          San = p.San ? "산" : String.Empty,
                                                          LotNumberMajorFrom = p.LotNumberMajorFrom,
                                                          LotNumberMinorFrom = p.LotNumberMinorFrom,
                                                          LotNumberMajorTo = p.LotNumberMajorTo,
                                                          LotNumberMinorTo = p.LotNumberMinorTo,
                                                          BuildingName = p.BuildingName,
                                                          BuildingNumberFrom = p.BuildingNumberFrom,
                                                          BuildingNumberTo = p.BuildingNumberTo
                                                      }))
                    {
                        context.LotBasedAddresses.AddObject(address);
                    }
                }
                context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);

                this.OnStatusChanged(new StatusChangeEventArgs(String.Format("Loaded the file - {0} - to database", filename)));
            }
        }

        /// <summary>
        /// Loads street-based addresses to database.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="filepaths">List of XML document paths.</param>
        private void LoadStreetBasedAddresses(KoreanPostcodeAddressDataContext context, IEnumerable<string> filepaths)
        {
            var serialiser = new XmlSerializer(typeof(StreetBasedAddresses));
            foreach (var filepath in filepaths)
            {
                using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    var collection = (StreetBasedAddresses)serialiser.Deserialize(stream);
                    foreach (var address in collection.StreetBasedAddress
                                                      .Select(p => new StreetBasedAddress()
                                                      {
                                                          Postcode = p.Postcode,
                                                          Province = p.Province,
                                                          ProvinceEng = p.ProvinceEng,
                                                          County = p.County,
                                                          CountyEng = p.CountyEng,
                                                          Suburb = p.Suburb,
                                                          SuburbEng = p.SuburbEng,
                                                          StreetName = p.StreetName,
                                                          StreetNameEng = p.StreetNameEng,
                                                          Basement = p.Basement > 0,
                                                          BuildingNumberMajor = p.BuildingNumberMajor,
                                                          BuildingNumberMinor = p.BuildingNumberMinor,
                                                          BuildingNameForBulk = p.BuildingNameForBulk,
                                                          BuildingName = p.BuildingName,
                                                          RegisteredSuburb = p.RegisteredSuburb,
                                                          Village = p.Village,
                                                          San = p.San,
                                                          LotNumberMajor = p.LotNumberMajor,
                                                          LotNumberMinor = p.LotNumberMinor
                                                      }))
                    {
                        context.StreetBasedAddresses.AddObject(address);
                    }
                }
                context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
            }
        }
        #endregion
    }
}