using System;
using FileHelpers;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Sanitisers
{
    /// <summary>
    /// This represnets the Int32 value sanitiser entity.
    /// </summary>
    public class Int32Sanitiser : ConverterBase
    {
        /// <summary>
        /// Converts a string in the file to a field value.
        /// </summary>
        /// <param name="from">The string to convert.</param>
        /// <returns>Returns the field value.</returns>
        public override object StringToField(string @from)
        {
            if (String.IsNullOrWhiteSpace(from))
                return Int32.MinValue;

            int ir;
            var result = Int32.MinValue;
            if (Int32.TryParse(from, out ir))
                result = ir;

            return result;
        }
    }
}