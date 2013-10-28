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
        public Timer FieldTimer { get; private set; }

        public StateManager(int rows, int columns)
        {
            engine = new Engine(400, 400, rows, columns);
        }

        static void Main(string[] args)
        {
            //MyTimer oTimer = new MyTimer();
            //oTimer.StartTimer();
        }

        public void MoveTractorTo(int row, int column)
        {
            engine.MoveTractorTo(row, column);
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

        public System.Windows.Point TractorPosition
        {
            get { return engine.TractorPosition; }
        }

        public System.Windows.Vector TractorVelocity
        {
            get { throw new NotImplementedException(); }
        }

        public event FieldChangedEventHandler FieldChanged;
    }
 
    /*
    class MyTimer
    {
        private int m_nStart = 0;
 
        public void StartTimer()
        {
            m_nStart = Environment.TickCount;
            Timer oTimer = new Timer();
            oTimer.Elapsed += new ElapsedEventHandler(OnTimeEvent);
            oTimer.Interval = 998;
            oTimer.Enabled = true;
            Console.WriteLine(
                "Naciśnij dowolny przycisk, aby zakończyć program.");
            Console.Read();
            oTimer.Stop();
        }
 
 
        private void OnTimeEvent(object oSource,
            ElapsedEventArgs oElapsedEventArgs)
        {
            Console.WriteLine("Upłyneło {0} milisekud",
                Environment.TickCount - m_nStart);
        }
    }
     */ 
}
