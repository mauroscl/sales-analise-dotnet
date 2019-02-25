using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Business.Domain;
using FileHelpers;

namespace Business.Converters
{
    internal class SaleItemCustomConverter: ConverterBase
    {
        public override object StringToField(string @from)
        {
            var regex = new Regex("^\\[.+\\]$");
            if (!regex.IsMatch(@from))
            {
                throw new Exception("Sale Item must start and terminate with brackets: " + @from);
            }

            var withoutBrackets = @from.Substring(1, @from.Length - 2);

            return withoutBrackets.Split(',')
                .Select(DeserializeItem)
                .ToList();

        }

        private SaleItem DeserializeItem(string item)
        {
            var values = item.Split("-");
            if (values.Length != 3)
            {
                throw new Exception("Sale item must have 3 values: " + item);
            }

            var numberFormatInfo = new NumberFormatInfo { CurrencyDecimalSeparator = "." };

            return new SaleItem(values[0], double.Parse(values[1], numberFormatInfo),  double.Parse(values[2], numberFormatInfo));
        }
    }
}
