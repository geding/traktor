//#define locked

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace InteligentnyTraktor.Model
{
    class Engine
    {
        int timerInterval = 20;
        private object _lock = new object();

        double width;
        double height;

        double destinationX;
        double destinationY;

        double ds = 2;

        public Tractor Tractor { get; private set; }

        public event EventHandler TractorReachedDestination;
        public event EventHandler TractorStopped;

        Timer timer;

        public Engine(double width, double height)
        {
            this.width = width;
            this.height = height;

            timer = new Timer(timerInterval);
            timer.AutoReset = true;
            timer.Start();

            Tractor = new Tractor()
            {
                Position = new Point(75, 75),
                Velocity = new Vector(0, 0),
                VMax = 2,
                Acceleration = 0.02,
            };           
        }


        //TODO:
        //ma sie poruszac z odpowiednia predkoscia do konkretnego miejsca
        //skalowanie wektora to x = (X / sqrt(X^2 + Y^2)) * długość wektora
        public void StartMovingTractor(double destX, double destY, double maxVelocity)
        {
            this.destinationX = destX;
            this.destinationY = destY;

            Tractor.Direction = new Vector(
                this.destinationX - Tractor.Position.X,
                this.destinationY - Tractor.Position.Y
                );

            Tractor.Velocity = new Vector(
                (Tractor.Direction.X == 0 ? 0 : (Tractor.Direction.X / Tractor.Direction.Length) * Tractor.VMax),
                (Tractor.Direction.Y == 0 ? 0 : (Tractor.Direction.Y / Tractor.Direction.Length) * Tractor.VMax)
                );

            timer.Elapsed += TractorMoves;
        }

        private void TractorMoves(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                if (Math.Abs(this.destinationX - Tractor.Position.X) > this.ds / 2
                || Math.Abs(this.destinationY - Tractor.Position.Y) > this.ds / 2)
                {
                    Tractor.Move(ds);
                }
                else
                {
                    StopTractor();
                    OnTractorReached();
                }
            }            
        }

        public void StopTractor()
        {
            Tractor.Velocity = new Vector(0, 0);
            timer.Elapsed -= TractorMoves;

            EventHandler invoker = TractorStopped;
            if (invoker != null)
            {
                invoker(this, new EventArgs());
            }
        }

        private void OnTractorReached()
        {
            EventHandler invoker = TractorReachedDestination;
            if (invoker != null)
            {
                invoker(this, new EventArgs());
            }
        }

        internal void ResetTractorReachedEvent()
        {
            TractorReachedDestination = null;
        }
    }
}
