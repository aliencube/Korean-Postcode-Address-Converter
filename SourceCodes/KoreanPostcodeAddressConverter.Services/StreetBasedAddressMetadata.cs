using System.ComponentModel.DataAnnotations;
using FileHelpers;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Sanitisers;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services
{
    /// <summary>
    /// This represents the StreetBasedAddress XML node entity.
    /// </summary>
    [DelimitedRecord("|")]
    [MetadataType(typeof(StreetBasedAddressMetadata))]
    public partial class StreetBasedAddress
    {
    }

    /// <summary>
    /// This represents the sanitiser entity to convert property values of the StreetBasedAddress class.
    /// </summary>
    public class StreetBasedAddressMetadata
    {
        /// <summary>
        /// Gets or sets the postcode field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string postcodeField;

        /// <summary>
        /// Gets or sets the sequence number field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string sequenceNumberField;

        /// <summary>
        /// Gets or sets the province field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string provinceField;

        /// <summary>
        /// Gets or sets the province field in English.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string provinceEngField;

        /// <summary>
        /// Gets or sets the county field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string countyField;

        /// <summary>
        /// Gets or sets the county field in English.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string countyEngField;

        /// <summary>
        /// Gets or sets the suburb field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string suburbField;

        /// <summary>
        /// Gets or sets the suburb field in English.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string suburbEngField;

        /// <summary>
        /// Gets or sets the street name code field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string streetNameCodeField;

        /// <summary>
        /// Gets or sets the street name field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string streetNameField;

        /// <summary>
        /// Gets or sets the street name field in English.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string streetNameEngField;

        /// <summary>
        /// Gets or sets the basement field.
        /// </summary>
        [FieldConverter(typeof(NullableInt32Sanitiser))]
        public int? basementField;

        /// <summary>
        /// Gets or sets the major part of the building number field.
        /// </summary>
        [FieldConverter(typeof(NullableInt32Sanitiser))]
        public int? buildingNumberMajorField;

        /// <summary>
        /// Gets or sets the minor part of the building number field.
        /// </summary>
        [FieldConverter(typeof(NullableInt32Sanitiser))]
        public int? buildingNumberMinorField;

        /// <summary>
        /// Gets or sets the building code field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string buildingCodeField;

        /// <summary>
        /// Gets or sets the building name field for bulk delivery.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string buildingNameForBulkField;

        /// <summary>
        /// Gets or sets the building name field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string buildingNameField;

        /// <summary>
        /// Gets or sets the registerd suburb code field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string registeredSuburbCodeField;

        /// <summary>
        /// Gets or sets the registered suburb field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string registeredSuburbField;

        /// <summary>
        /// Gets or sets the village field.
        /// </summary>
        [FieldConverter(typeof(WhiteSpaceSanitiser))]
        public string villageField;

        /// <summary>
        /// Gets or sets the field value that specifies whether the address is marked as "San" or not.
        /// </summary>
        [FieldConverter(typeof(NullableBooleanSanitiser))]
        public bool? sanField;

        /// <summary>
        /// Gets or sets the major part of the LOT number field.
        /// </summary>
        [FieldConverter(typeof(NullableInt32Sanitiser))]
        public int? lotNumberMajorField;

        /// <summary>
        /// Gets or sets the minor part of the LOT number field.
        /// </summary>
        [FieldConverter(typeof(NullableInt32Sanitiser))]
        public int? lotNumberMinorField;
    }
}
