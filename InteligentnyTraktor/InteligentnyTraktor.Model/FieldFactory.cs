using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    public partial class Field
    {
        static public class FieldFactory
        {
            static public Field Create(FieldItemType type)
            {
                switch (type)
                {
                    case FieldItemType.Wheat:
                        return new Field(type, 200);
                    case FieldItemType.Rye:
                        return new Field(type, 300);
                    case FieldItemType.Corn:
                        return new Field(type, 400);
                    default:
                        return null;
                }
            }
        }
    }
}
