using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    public delegate void FieldChangedEventHandler(object sender, FieldEventArgs e);

    public class FieldItemEventArgs : EventArgs
    {
        public readonly FieldItemState newState;
        public readonly FieldItemType type;

        public FieldItemEventArgs(FieldItemState newState, FieldItemType type)
        {
            this.newState = newState;
            this.type = type;
        }
    }

    public class FieldEventArgs : FieldItemEventArgs
    {
        public readonly int row;
        public readonly int column;

        public FieldEventArgs(int row, int column, FieldItemState newState, FieldItemType type)
            : base(newState, type)
        {
            this.row = row;
            this.column = column;
        }
    }
}
