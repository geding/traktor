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
        private double counter = 0;
        private double growTime = 10;
        private double fertilityRate = 1;
        private bool isIrrigated = false;
        private bool isSowed = false;
        private bool isPlowed = false;

        private object _lock = new object();
        Dictionary<FieldItemState, Action> onUpdateActions;

        public FieldItemState State { get; private set; }
        public FieldItemType Type { get; private set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public delegate void FieldItemChanged(object s, FieldItemEventArgs ea);
        public event FieldItemChanged Changed;

        private Field(FieldItemType type, int growTime, int row, int column)
        {
            this.Row = row;
            this.Column = column;
            this.State = FieldItemState.Bare;
            this.Type = type;
            this.growTime = growTime;

            onUpdateActions = new Dictionary<FieldItemState, Action>()
            {
                { FieldItemState.Bare, (Action)None },
                { FieldItemState.Plowed, (Action)None },
                { FieldItemState.Sowed, (Action)Grow },
                { FieldItemState.EarlyGrowing, (Action)Grow },
                { FieldItemState.MidGrowing, (Action)Grow },
                { FieldItemState.LateGrowing, (Action)Grow },
                { FieldItemState.Mature, (Action)Rot },
                { FieldItemState.Rotten, (Action)None },
                { FieldItemState.Harvested, (Action)Rot }
            };
        }

        static public Field Create(FieldItemType type, int row, int column)
        {
            
            switch (type)
            {
                case FieldItemType.Wheat:
                    return new Field(type, 20,row,column);
                case FieldItemType.Rye:
                    return new Field(type, 30, row, column);
                case FieldItemType.Corn:
                    return new Field(type, 40, row, column);
                default:
                    return null;
            }
        }

        public void Update(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    onUpdateActions[this.State].Invoke();
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
            }
           
            
            /*
            switch (this.State)
            {
                case FieldItemState.Sowed:
                case FieldItemState.EarlyGrowing:
                case FieldItemState.MidGrowing:
                case FieldItemState.LateGrowing: 
                case FieldItemState.Mature: Grow();
                    break;
                case FieldItemState.Harvested:
                    break;
                default:
                    break;
            }
             */ 
        }

        public void OnChanged()
        {
            this.counter = 0;
            Changed(this, new FieldItemEventArgs(this.State, this.Type));
        }

        public void Sow()
        {
            lock (_lock)
            {
                if (isSowed)
                {
                    return;
                }
                if (isPlowed)
                {
                    isSowed = true;
                    this.State = FieldItemState.Sowed;
                    OnChanged();
                }
            }            
        }

        public void Irrigate()
        {
            lock (_lock)
            {
                if (isIrrigated)
                {
                    return;
                }
                else
                {
                    isIrrigated = true;
                }
            }
        }

        public void Fertilize()
        {
            fertilityRate += 0.1;
        }

        public void Harvest()
        {
            lock (_lock)
            {
                if (this.State != FieldItemState.Mature)
                {
                    return;
                }
                else
                {
                    fertilityRate = 0.5;
                    isPlowed = false;
                    isSowed = false;
                    this.State = FieldItemState.Harvested;
                    OnChanged();
                }
            }
        }

        public void Plow()
        {
            lock (_lock)
            {
                if (isPlowed)
                {
                    return;
                }
                else
                {
                    isPlowed = true;
                    this.State = FieldItemState.Plowed;
                    OnChanged();
                }
            }
        }

        private void IncreaseCounter()
        {
            counter += 1 * fertilityRate; 
        }

        private void Grow()
        {
            lock (_lock)
            {
                IncreaseCounter();
                if (this.counter >= this.growTime)
                {
                    this.State++;
                    OnChanged();
                }               
            }            
        }

        public void None() {}

        private void Rot()
        {
            lock (_lock)
            {
                IncreaseCounter();
                if (this.counter >= this.growTime)
                {
                    isSowed = false;
                    isPlowed = false;
                    this.State = FieldItemState.Rotten;
                    OnChanged();
                }
            }
        }
    }
}
