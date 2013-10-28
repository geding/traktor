using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace InteligentnyTraktor.Model
{
    class Engine
    {
        double fieldWidth;
        double fieldHeight;

        double fieldItemWidth;
        double fieldItemHeight;

        double destinationX;
        double destinationY;

        double dx = 2;
        double dy = 2;

        bool isXReached;
        bool isYReached;

        public Point TractorPosition { get; private set; }
        public Vector TractorVelocity { get; private set; }

        public event EventHandler TractorStopped;

        Timer timer;

        public Engine(double fieldWidth, double fieldHeight, int rows, int columns)
        {
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.fieldItemWidth = fieldWidth / rows;
            this.fieldItemHeight = fieldHeight / columns;

            timer = new Timer(20);
            timer.Start();

            TractorPosition = new Point(fieldItemHeight / 2, fieldItemWidth / 2);
        }

        public void MoveTractorTo(int row, int column)
        {
            isXReached = false;
            isYReached = false;

            this.destinationX = fieldItemHeight * (column + 0.5);
            this.destinationY = fieldItemWidth * (row + 0.5);            

            timer.Elapsed += MoveTractor;
        }

        private void MoveTractor(object sender, ElapsedEventArgs e)
        {
            if (destinationX < TractorPosition.X)
            {
                MoveTractorLeft();
            }
            else
            {
                MoveTractorRight(); 
            }
            if (isXReached)
            {
                if (destinationY < TractorPosition.Y)
                {
                    MoveTractorDown();
                }
                else
                {
                    MoveTractorUp();
                }
            }

            if (isXReached && isYReached)
            {
                timer.Elapsed -= MoveTractor;
                EventHandler temp = TractorStopped;
                if (temp != null)
                {
                    temp(this, new EventArgs());
                }
            }           
        }        

        private void MoveTractorRight()
        {
            if (Math.Abs(this.destinationX - TractorPosition.X) > dx)
            {
                TractorPosition = new Point(TractorPosition.X + dx, TractorPosition.Y);
            }
            else isXReached = true;
        }

        private void MoveTractorLeft()
        {
            if (Math.Abs(this.destinationX - TractorPosition.X) > dx)
            {
                TractorPosition = new Point(TractorPosition.X - dx, TractorPosition.Y);
            }
            else isXReached = true;
        }

        private void MoveTractorUp()
        {
            if (Math.Abs(this.destinationY - TractorPosition.Y) > dy)
            {
                TractorPosition = new Point(TractorPosition.X, TractorPosition.Y + dy);
            }
            else isYReached = true;
        }

        private void MoveTractorDown()
        {
            if (Math.Abs(this.destinationY - TractorPosition.Y) > dy)
            {
                TractorPosition = new Point(TractorPosition.X, TractorPosition.Y - dy);
            }
            else isYReached = true;
        }
    }
}
