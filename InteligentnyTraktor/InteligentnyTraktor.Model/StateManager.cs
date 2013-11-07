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

        public event EventHandler TractorIsBusy;
        //Queue<TractorTask> tasks;

        public StateManager(double fieldWidth, double fieldHeight, int rows, int columns)
        {
            engine = new Engine(fieldWidth, fieldHeight, rows, columns);
        }

        static void Main(string[] args)
        {
            
        }

        public void MoveTractorTo(int row, int column)
        {
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
            throw new NotImplementedException();
        }

        public void FertilizeAt(int row, int column)
        {
            throw new NotImplementedException();
        }

        public void IrrigateAt(int row, int column)
        {
            throw new NotImplementedException();
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

        public event FieldChangedEventHandler FieldChanged;

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
