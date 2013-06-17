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
        private KoreanPostcodeAddressConverterSettings _conversionSettings;
        #endregion

        #region Methods
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
        /// Gets the unzip file path for command line process.
        /// </summary>
        /// <param name="element">Unzip path element.</param>
        /// <exception cref="ConfigurationErrorsException">Throws when unzip file path has not been set.</exception>
        /// <returns>Returns the unzip file path for command line process.</returns>
        public string GetUnzipPath(UnzipPathElement element)
        {
            var path = element.Path;
            if (String.IsNullOrWhiteSpace(path))
                throw new ConfigurationErrorsException("Unzip file path has not been set");

            return path;
        }

        /// <summary>
        /// Gets the absolute path of the given path.
        /// </summary>
        /// <param name="element">Directory element. It can contain either relative or absolute value.</param>
        /// <exception cref="ConfigurationErrorsException">Throws when the directory has not been set.</exception>
        /// <returns>Returns the absolute path of the given path.</returns>
        public string GetAbsoluteDirectory(DirectoryElement element)
        {
            var path = element.Value;
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
        public IList<string> GetFilenamesToDownloadOrExtract(FilenamesElementCollection collection)
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
