using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Configuration;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services
{
    /// <summary>
    /// This represents the configuration settings entity and must be inherited for use.
    /// </summary>
    public abstract class Settings
    {
        #region Constructors
        ///// <summary>
        /////	Initialises a new instance of the Settings object as private.
        ///// </summary>
        //private Settings()
        //{
        //}
        #endregion

        #region Properties
        //private static Settings _instance;
        ///// <summary>
        ///// Gets the instance of the settings object.
        ///// </summary>
        //public static Settings Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new Settings();
        //        return _instance;
        //    }
        //}

        private KoreanPostcodeAddressConverterSettings _conversionSettings;
        /// <summary>
        /// Gets the Korean postcode-address conversion settings.
        /// </summary>
        public KoreanPostcodeAddressConverterSettings ConversionSettings
        {
            get
            {
                if (this._conversionSettings == null)
                    this._conversionSettings = (KoreanPostcodeAddressConverterSettings)ConfigurationManager.GetSection("koreanPostcodeAddressConverterSettings");
                return this._conversionSettings;
            }
        }

        private string _unzipPath;
        /// <summary>
        /// Gets the unzip executable (eg. 7-zip) location path.
        /// </summary>
        public string UnzipPath
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._unzipPath))
                    this._unzipPath = this.ConversionSettings.UnzipPath.Filepath;
                return this._unzipPath;
            }
        }

        private IDictionary<string, string> _streetNameCorrections;
        /// <summary>
        /// Gets the list of street name corrections.
        /// </summary>
        public IDictionary<string, string> StreetNameCorrections
        {
            get
            {
                if (this._streetNameCorrections == null || !this._streetNameCorrections.Any())
                    this._streetNameCorrections = this.ConversionSettings
                                                      .LocationMarkers
                                                      .StreetNameCorrections
                                                      .Cast<KeyValuePairElement>()
                                                      .ToDictionary(p => p.Key, p => p.Value);
                return this._streetNameCorrections;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the value to process requests.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="key">Property key.</param>
        /// <returns>Returns the value to process requests.</returns>
        public T GetProcessRequests<T>(string key)
        {
            var instance = this.ConversionSettings.ProcessRequests;
            var pi = instance.GetType()
                             .GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var value = (T)pi.GetValue(instance, null);

            return value;
        }

        /// <summary>
        /// Gets the list of delimiters.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <returns>Returns the list of delimiters.</returns>
        public char[] GetDelimiters(string key)
        {
            var instance = this.ConversionSettings.SegmentDelimiters;
            var pi = instance.GetType()
                             .GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var value = (string)pi.GetValue(instance, null);

            if (String.IsNullOrWhiteSpace(value))
                return null;

            var result = value.ToCharArray();
            return result;
        }

        /// <summary>
        /// Gets the list of location markers.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <returns>Returns the list of location markers.</returns>
        public char[] GetLocationMarkers(string key)
        {
            var instance = this.ConversionSettings.LocationMarkers;
            var pi = instance.GetType()
                             .GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var value = (string)pi.GetValue(instance, null);

            if (String.IsNullOrWhiteSpace(value))
                return null;

            var result = value.ToCharArray();
            return result;
        }

        /// <summary>
        /// Gets the download URL.
        /// </summary>
        /// <param name="element">Download URL element.</param>
        /// <exception cref="ConfigurationErrorsException">Throws when URL has neither been set nor started with "http://".</exception>
        /// <returns>Returns the download URL.</returns>
        public string GetDownloadUrl(DownloadUrlElement element)
        {
            var url = element.Url;
            if (String.IsNullOrWhiteSpace(url))
                throw new ConfigurationErrorsException("Download URL has not been set");

            if (!url.ToLower().StartsWith("http://"))
                throw new ConfigurationErrorsException("Download URL should start with \"http://\".");

            return url;
        }

        /// <summary>
        /// Gets the absolute path of the given path.
        /// </summary>
        /// <param name="element">Directory element. It can contain either relative or absolute value.</param>
        /// <exception cref="ConfigurationErrorsException">Throws when the directory has not been set.</exception>
        /// <returns>Returns the absolute path of the given path.</returns>
        public string GetAbsoluteDirectory(DirectoryElement element)
        {
            var path = element.Directory;
            if (String.IsNullOrWhiteSpace(path))
                throw new ConfigurationErrorsException("Directory has not been set");

            //  Gets the absolute path using either Server.MapPath or Path.GetDirectoryName.
            var context = HttpContext.Current;
            if (context == null)
            {
                path = path.Replace("/", "\\");

                if (path.Contains(":\\"))
                    return path;

                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                path = String.Format(path.StartsWith("\\") ? "{0}{1}" : "{0}\\{1}",
                                     directory.TrimEnd('\\'),
                                     path.TrimEnd('\\'));
            }
            else
            {
                if (path.StartsWith("~/"))
                    path = context.Server.MapPath(path);
                else
                    path = String.Format("{0}\\{1}",
                                         context.Server.MapPath("~/").TrimEnd('\\'),
                                         path.Replace('/', '\\').Trim('\\'));
            }

            return path;
        }

        /// <summary>
        /// Gets the list of filenames to download or extract.
        /// </summary>
        /// <param name="collection">The filenames collection element.</param>
        /// <exception cref="ConfigurationErrorsException">Throws when no files to download has been set.</exception>
        /// <returns>Returns the list of filenames to download.</returns>
        public IList<string> GetFilenamesToDownloadOrExtract(FilenameElementCollection collection)
        {
            if (collection.Count == 0)
                throw new ConfigurationErrorsException("Files to download or extract have not been set");

            var filenames = collection.Cast<FilenameElement>()
                                      .Select(p => p.Filename)
                                      .ToList();

            if (!filenames.Any())
                throw new ConfigurationErrorsException("Files to download or extract have not been set");

            return filenames;
        }

        /// <summary>
        /// Gets the zip filename for archiving.
        /// </summary>
        /// <param name="element">Archive filename element.</param>
        /// <exception cref="ConfigurationErrorsException">Throws when zip filename has not been set.</exception>
        /// <returns>Returns the zip filename for archiving.</returns>
        public string GetFilenameForArchive(FilenameElement element)
        {
            var filename = element.Filename;
            if (String.IsNullOrWhiteSpace(filename))
                throw new ConfigurationErrorsException("Zip filename for archive has not been set");

            return filename;
        }
        #endregion
    }
}
