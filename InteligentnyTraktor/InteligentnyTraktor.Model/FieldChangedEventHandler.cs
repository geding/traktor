using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    public delegate void FieldChangedEventHandler(object sender, FieldEventArgs e);

    public class FieldEventArgs : EventArgs
    {
        public readonly int row;
        public readonly int column;
        public readonly FieldItemState newState;
        public readonly FieldItemType type;
    }
}
