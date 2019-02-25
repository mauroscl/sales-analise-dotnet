using System;
using System.Collections.Generic;
using FileHelpers;

namespace Business.Converters
{
    internal class ListCustomConverter: ConverterBase
    {
        public override object StringToField(string @from)
        {
            return @from;
        }

        public override string FieldToString(object @from)
        {
            var list = ((IList<string>) @from);
            return "[" + String.Join(",", list) + "]";
            
        }
    }
}
