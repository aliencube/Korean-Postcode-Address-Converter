using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers
{
    /// <summary>
    /// This helps to convert value with an appropriate type.
    /// </summary>
    public class ConversionHelper
    {
        #region Methods
        /// <summary>
        /// Gets the filename from the filepath provided.
        /// </summary>
        /// <param name="filepath">Full filepath.</param>
        /// <param name="settings">Configuration settings instance.</param>
        /// <returns>Returns the filename.</returns>
        public static string GetFilenameFromFilepath(string filepath, Settings settings)
        {
            var segments = filepath.Split(settings.ConversionSettings
                                                  .SegmentSeparatorForDirectory
                                                  .Delimiters
                                                  .ToCharArray(),
                                          StringSplitOptions.RemoveEmptyEntries);
            var filename = segments[segments.Length - 1];
            return filename;
        }

        /// <summary>
        /// Gets the timestamp of the file from the filename provided.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="settings">Configuration settings instance.</param>
        /// <returns>Returns the timestamp.</returns>
        public static string GetTimestampFromFilename(string filename, Settings settings)
        {
            var timestamp = filename.Split(settings.ConversionSettings
                                                   .SegmentSeparatorForFile
                                                   .Delimiters
                                                   .ToCharArray(),
                                           StringSplitOptions.RemoveEmptyEntries)
                                    .First();
            return timestamp;
        }

        /// <summary>
        /// Sanitises all white spaces except space characters specified in Unicode.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>Returns string value removed all white spaces except space characters.</returns>
        public static string SanitiseWhiteSpaces(object value)
        {
            var tmp = value as String;
            if (String.IsNullOrWhiteSpace(tmp))
                return null;

            var converted = tmp;
            var matches = Regex.Matches(tmp, "[\x00-\x1f]");
            foreach (var match in matches.Cast<Match>())
                converted = tmp.Replace(match.Value, String.Format("&#{0:X4};", Convert.ToInt32(match.Value)));

            return converted;
        }

        /// <summary>
        /// Converts the object value to boolean value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns <c>True</c>, if the value corresponds to <c>True</c>; otherwise returns <c>False</c>.</returns>
        public static bool ConvertToBoolean(object value)
        {
            var tmp = value as String;
            if (String.IsNullOrWhiteSpace(tmp))
                return false;

            if (tmp.Trim() == "산")
                return true;

            int ir;
            bool br;

            var result = false;
            if (Int32.TryParse(tmp.Trim(), out ir))
                result = Convert.ToBoolean(ir);
            else if (Boolean.TryParse(tmp.Trim(), out br))
                result = br;

            return result;
        }

        /// <summary>
        /// Converts the object value to nullable integer value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns nullable integer value converted.</returns>
        public static int ConvertToInt32(object value)
        {
            var tmp = value as String;
            if (String.IsNullOrWhiteSpace(tmp))
                return Int32.MinValue;

            int ir;

            var result = 0;
            if (Int32.TryParse(tmp.Trim(), out ir))
                result = ir;

            return result;
        }

        /// <summary>
        /// Converts the string value to nullable boolean value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns NULL, if the value is NULL, returns <c>True</c>, if the value corresponds to <c>True</c>; otherwise returns <c>False</c>.</returns>
        public static bool? ConvertToNullableBoolean(object value)
        {
            var tmp = value as String;
            if (String.IsNullOrWhiteSpace(tmp))
                return null;

            return ConvertToBoolean(value);
        }

        /// <summary>
        /// Converts the object value to nullable integer value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns nullable integer value converted.</returns>
        public static int? ConvertToNullableInt32(object value)
        {
            var tmp = value as String;
            if (String.IsNullOrWhiteSpace(tmp))
                return null;

            var result = ConvertToInt32(value);

            return result <= 0 ? (int?) null : result;
        }

        /// <summary>
        /// Converts the object value to DateTime value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns DateTime value.</returns>
        public static DateTime ConvertToDateTime(object value)
        {
            var tmp = value as String;
            if (String.IsNullOrWhiteSpace(tmp))
                return DateTime.MinValue;

            if (!Regex.IsMatch(tmp.Trim(), @"\d{8}"))
                return DateTime.MinValue;

            var year = Convert.ToInt32(tmp.Trim().Substring(0, 4));
            var month = Convert.ToInt32(tmp.Trim().Substring(4, 2));
            var day = Convert.ToInt32(tmp.Trim().Substring(6, 2));

            var result = new DateTime(year, month, day);
            return result;
        }
        #endregion
    }
}
