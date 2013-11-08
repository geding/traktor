using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace InteligentnyTraktor.Model
{
    public class StateManager : IStateManager
    {
        Engine engine;
        bool isTractorBusy = false;
        public Timer FieldTimer { get; private set; }
        public Field[][] fieldItems;

        public event EventHandler TractorIsBusy;
        public event FieldChangedEventHandler FieldChanged;
        //Queue<TractorTask> tasks;

        private double timerInterval = 1000;

        public StateManager(double fieldWidth, double fieldHeight, int rows, int columns)
        {
            engine = new Engine(fieldWidth, fieldHeight, rows, columns);
            FieldTimer = new Timer(timerInterval);
            FieldTimer.Start();
            fieldItems = new Field[rows][];
            for (int i = 0; i < columns; i++)
            {
                fieldItems[i] = new Field[columns];
            }

            CreateSampleField();
        }

        private void CreateSampleField()
        {
            for (int i = 0; i < fieldItems.Length; i++)
            {
                for (int j = 0; j < fieldItems[i].Length; j++)
                {
                    if (i % 3 == 0 && i % 2 == 0)
                    {
                        fieldItems[i][j] = FieldFactory.CreateField(FieldItemType.Wheat);
                    }
                    else
                    {
                        fieldItems[i][j] = i % 2 == 0 && j % 3 == 0
                                           ? FieldFactory.CreateField(FieldItemType.Rye)
                                           : FieldFactory.CreateField(FieldItemType.Corn);
                    }
                }
            }

            SetFieldItemsEvents();
        }

        private void SetFieldItemsEvents()
        {
            foreach (var fRow in fieldItems)
            {
                foreach (var f in fRow)
                {
                    f.Changed += OnFieldChanged;
                    FieldTimer.Elapsed += f.Update;
                }               
            }
        }

        private void OnFieldChanged(object s, FieldItemEventArgs ea)
        {
            var f = s as Field;
            if (f == null)
            {
                return;
            }
            var fRow = (from r in fieldItems
                           where r.Contains(f)
                           select r).SingleOrDefault();
            int column = Array.IndexOf(fRow, f);
            int row = Array.IndexOf(fieldItems, fRow);

            FieldChanged(this, new FieldEventArgs(row, column, ea.newState, ea.type));
        }

        static void Main(string[] args)
        {
            
        }

        public void MoveTractorTo(int row, int column)
        {
            engine.ResetTractorReachedEvent();
            if (isTractorBusy == false)
            {
                isTractorBusy = true;
                engine.TractorReachedDestination += (s, e) => isTractorBusy = false;
                engine.StartMovingTractorTo(row, column);
            }
            else OnTractorIsBusy();
        }       

        public void HarvestAt(int row, int column)
        {
            fieldItems[row][column].Harvest();
        }

        public void FertilizeAt(int row, int column)
        {
            fieldItems[row][column].Fertilize();
        }

        public void IrrigateAt(int row, int column)
        {
            fieldItems[row][column].Irrigate();
        }

        public void SowAt(int row, int column)
        {
            MoveTractorTo(row, column);
            engine.TractorReachedDestination += (s, e) => fieldItems[row][column].Sow();
        }

        public void StopTractor()
        {
            engine.TractorStopped += (s, e) => isTractorBusy = false;
            engine.StopTractor();
        }

        public System.Windows.Point TractorPosition
        {
            get { return engine.Tractor.Position; }
        }

        public System.Windows.Vector TractorDirection
        {
            get { return engine.Tractor.Direction; }
        }        

        private void OnTractorIsBusy()
        {
            EventHandler temp = TractorIsBusy;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }        
    } 
}
