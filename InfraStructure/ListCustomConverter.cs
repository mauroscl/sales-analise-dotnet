using FileHelpers;
using System;
using System.Collections;

namespace InfraStructure
{
    public class ListCustomConverter: ConverterBase
    {
        public override object StringToField(string @from)
        {
            return @from;
        }

        public override string FieldToString(object @from)
        {
            var list = ((IList) @from);
            return "[" + String.Join(",", list) + "]";
            
        }
    }
}
