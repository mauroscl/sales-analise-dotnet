using System.Collections.Generic;
using FileHelpers;

namespace SalesProcessor.Adapters.Secondary
{
    public class ListCustomConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return from;
        }

        public override string FieldToString(object from)
        {
            var list = (IList<string>) from;
            return "[" + string.Join(",", list) + "]";
        }
    }
}