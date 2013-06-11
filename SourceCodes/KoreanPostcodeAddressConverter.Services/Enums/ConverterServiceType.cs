namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums
{
    /// <summary>
    /// This specifies the type of the converter service.
    /// </summary>
    public enum ConverterServiceType
    {
        /// <summary>
        /// This indicates that no converter service type is defined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// This indicates that LOT-based address converter type is defined.
        /// </summary>
        Lot = 1,

        /// <summary>
        /// This indicates that street-based address converter type is defined.
        /// </summary>
        Street = 2
    }
}
