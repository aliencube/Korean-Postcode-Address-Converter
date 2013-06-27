using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services.Helpers
{
    /// <summary>
    /// This helps to convert value with an appropriate type.
    /// </summary>
    public class ConversionHelper : Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers.ConversionHelper
    {
        /// <summary>
        /// Gets the province.
        /// </summary>
        /// <param name="province">Province field data.</param>
        /// <returns>Returns the province.</returns>
        public static string GetProvince(string province)
        {
            if (String.IsNullOrWhiteSpace(province))
                return null;

            var settings = Settings.Instance;
            var provinceMarker = Convert.ToString(province.Trim().ToCharArray().Last());

            return settings.ProvinceMarkers.Contains(provinceMarker) ? province : null;
        }

        /// <summary>
        /// Gets the province in English.
        /// </summary>
        /// <param name="province">ProvinceEng field data.</param>
        /// <returns>Returns the province in English.</returns>
        public static string GetProvinceEng(string province)
        {
            return GetString(province);
        }

        /// <summary>
        /// Gets the city - metropolitan city only.
        /// </summary>
        /// <param name="province">Province field data.</param>
        /// <returns>Returns the city - metropolitan city only.</returns>
        public static string GetCity(string province)
        {
            //  Returns NULL, if province is empty.
            if (String.IsNullOrWhiteSpace(province))
                return null;

            var settings = Settings.Instance;

            //  Returns the province, if the province marker belongs to county markers, ie. metropolitan city.
            var provinceMarker = Convert.ToString(province.Trim().ToCharArray().Last());
            if (settings.CountyMarkers.Contains(provinceMarker))
                return province.Trim();

            return null;
        }

        /// <summary>
        /// Gets the city in English - metropolitan city only.
        /// </summary>
        /// <param name="province">ProvinceEng field data.</param>
        /// <returns>Returns the city in English - metropolitan city only.</returns>
        public static string GetCityEng(string province)
        {
            return null;
        }

        /// <summary>
        /// Gets the county.
        /// </summary>
        /// <param name="county">County field data.</param>
        /// <returns>Returns the county.</returns>
        public static string GetCounty(string county)
        {
            //  Returns province, if county is empty.
            if (String.IsNullOrWhiteSpace(county))
                return null;

            var settings = Settings.Instance;

            //  Redefines the county.
            var segments = county.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            county = segments[0];

            //  Returns the county, if the county marker belongs to the county markers.
            var countyMarker = Convert.ToString(county.Trim().ToCharArray().Last());
            if (settings.CountyMarkers.Contains(countyMarker))
                return county.Trim();

            return null;
        }

        /// <summary>
        /// Gets the county in English.
        /// </summary>
        /// <param name="province">ProvinceEng field data.</param>
        /// <param name="county">CountyEng field data.</param>
        /// <returns>Returns the county in English.</returns>
        public static string GetCountyEng(string province, string county)
        {
            return null;
        }

        /// <summary>
        /// Gets the district.
        /// </summary>
        /// <param name="province">Province field data.</param>
        /// <param name="county">County field data.</param>
        /// <returns>Returns the district.</returns>
        public static string GetDistrict(string province, string county)
        {
            //  Returns NULL, if county is empty.
            if (String.IsNullOrWhiteSpace(county))
                return null;

            var settings = Settings.Instance;

            //  Returns the county, if the province marker belongs to county markers, ie. metropolitan city.
            var provinceMarker = Convert.ToString(province.Trim().ToCharArray().Last());
            if (settings.CountyMarkers.Contains(provinceMarker))
                return county.Trim();

            //  Defines district from county.
            var segments = county.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var district = segments.Length > 1 ? segments[segments.Length - 1] : segments[0];

            //  Returns the district, derived from the county.
            var districtMarker = Convert.ToString(district.Trim().ToCharArray().Last());
            if (settings.DistrictMarkers.Contains(districtMarker))
                return district;

            return null;
        }

        /// <summary>
        /// Gets the district in English.
        /// </summary>
        /// <param name="province">ProvinceEng field data.</param>
        /// <param name="county">CountyEng field data.</param>
        /// <returns>Returns the district in English.</returns>
        public static string GetDistrictEng(string province, string county)
        {
            return null;
        }

        /// <summary>
        /// Gets the suburb.
        /// </summary>
        /// <param name="suburb">Suburb field data.</param>
        /// <returns>Returns the suburb.</returns>
        public static string GetSuburb(string suburb)
        {
            if (String.IsNullOrWhiteSpace(suburb))
                return null;

            var settings = Settings.Instance;
            var suburbMarker = Convert.ToString(suburb.Trim().ToCharArray().Last());

            return settings.SuburbMarkers.Contains(suburbMarker) ? RemoveDigits(suburb) : null;
        }

        /// <summary>
        /// Gets th suburb in English.
        /// </summary>
        /// <param name="suburb">SuburbEng field data.</param>
        /// <returns>Returns the suburb.</returns>
        public static string GetSuburbEng(string suburb)
        {
            return GetString(suburb);
        }

        /// <summary>
        /// Gets the village.
        /// </summary>
        /// <param name="village">Village field data.</param>
        /// <returns>Returns the village.</returns>
        public static string GetVillage(string village)
        {
            if (String.IsNullOrWhiteSpace(village))
                return null;

            //  Removes parenthesis value.
            village = Regex.Replace(village, @"\(.+\)", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var settings = Settings.Instance;
            var villageMarker = Convert.ToString(village.Trim().ToCharArray().Last());

            return settings.VillageMarkers.Contains(villageMarker) ? RemoveDigits(village) : null;
        }

        /// <summary>
        /// Gets the island.
        /// </summary>
        /// <param name="island">Island field data.</param>
        /// <returns>Returns the island.</returns>
        public static string GetIsland(string island)
        {
            if (String.IsNullOrWhiteSpace(island))
                return null;

            var settings = Settings.Instance;
            var islandMarker = Convert.ToString(island.Trim().ToCharArray().Last());

            return settings.IslandMarkers.Contains(islandMarker) ? island : null;
        }

        /// <summary>
        /// Gets the san.
        /// </summary>
        /// <param name="san">San field data.</param>
        /// <returns>Returns the san.</returns>
        public static string GetSan(bool san)
        {
            return san ? "산" : null;
        }

        /// <summary>
        /// Gets the street.
        /// </summary>
        /// <param name="street">Street field data.</param>
        /// <param name="streetEng">StreetEng field data.</param>
        /// <returns>Returns the street.</returns>
        public static string GetStreet(string street, string streetEng)
        {
            if (String.IsNullOrWhiteSpace(street))
                return null;

            var settings = Settings.Instance;
            var streetMarker = Convert.ToString(street.Trim().ToCharArray().Last());

            return settings.StreetMarkers.Contains(streetMarker)
                       ? RemoveDigits(street)
                       : (streetMarker == "0" ? GetStreetCorrected(streetEng.Trim()) : null);
        }

        /// <summary>
        /// Gets the strint in English.
        /// </summary>
        /// <param name="street">StreetEng field data.</param>
        /// <returns>Returns the street in English.</returns>
        public static string GetStreetEng(string street)
        {
            return GetString(street);
        }

        /// <summary>
        /// Gets the street name corrected.
        /// </summary>
        /// <param name="streetEng">StreetEng field data.</param>
        /// <returns>Returns the street name corrected.</returns>
        private static string GetStreetCorrected(string streetEng)
        {
            var settings = Settings.Instance;
            return settings.StreetNameCorrections.ContainsKey(streetEng)
                       ? settings.StreetNameCorrections[streetEng]
                       : null;
        }

        /// <summary>
        /// Removes digits from the locality value.
        /// </summary>
        /// <param name="value">Locality value.</param>
        /// <returns>Returns the locality value that digits are removed.</returns>
        private static string RemoveDigits(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;

            var pattern = @"^([^d]+)\d+([^\d]+)$";
            var replacement = "$1$2";

            var result = Regex.Replace(value, pattern, replacement, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return result;
        }
    }
}
