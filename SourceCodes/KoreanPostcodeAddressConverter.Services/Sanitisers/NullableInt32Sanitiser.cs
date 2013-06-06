using FileHelpers;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Helpers;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Sanitisers
{
    /// <summary>
    /// This represnets the Int32 value sanitiser entity.
    /// </summary>
    public class NullableInt32Sanitiser : ConverterBase
    {
        /// <summary>
        /// Converts a string in the file to a field value.
        /// </summary>
        /// <param name="from">The string to convert.</param>
        /// <returns>Returns the field value.</returns>
        public override object StringToField(string @from)
        {
            var result = ConversionHelper.ConvertToNullableInt32(from);
            return result;
        }
    }
}