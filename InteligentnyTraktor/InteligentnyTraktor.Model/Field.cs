using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace InteligentnyTraktor.Model
{
    public partial class Field
    {
        private int counter = 0;
        private int growTime = 2;
        private bool isIrrigated = false;
        private bool isSowed = false;

        public FieldItemState State { get; private set; }
        public FieldItemType Type { get; private set; }

        public delegate void FieldItemChanged(object s, FieldItemEventArgs ea);
        public event FieldItemChanged Changed;

        private Field(FieldItemType type, int growTime)
        {
            this.State = FieldItemState.Bare;
            this.Type = type;
            this.growTime = growTime;
        }

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

        public void Update(object sender, ElapsedEventArgs e)
        {
            counter++;
            switch (this.State)
            {
                case FieldItemState.Sowed: UpdateWhenSowed();
                    break;
                case FieldItemState.EarlyGrowing:
                    break;
                case FieldItemState.MidGrowing:
                    break;
                case FieldItemState.LateGrowing:
                    break;
                case FieldItemState.Mature:
                    break;
                case FieldItemState.Harvested:
                    break;
                default:
                    break;
            }                         
        }

        public void OnChanged()
        {
            this.counter = 0;
            Changed(this, new FieldItemEventArgs(this.State, this.Type));
        }

        public void Sow()
        {
            if (isSowed)
            {
                return;
            }
            else
            {
                isSowed = true;
                this.State = FieldItemState.Sowed;
            }
        }

        public void Irrigate()
        {
            throw new NotImplementedException();
        }

        public void Fertilize()
        {
            throw new NotImplementedException();
        }

        public void Harvest()
        {
            throw new NotImplementedException();
        }

        private void UpdateWhenSowed()
        {
            counter++;
            if (this.counter == this.growTime)
            {
                this.State = FieldItemState.EarlyGrowing;
            }
            OnChanged();
        }
    }
}
