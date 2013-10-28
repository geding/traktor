using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace InteligentnyTraktor.Model
{
    public enum Direction { None, Left, Up, Right, Down };
    public enum Dimension { Horizontal, Vertical };

    class Engine
    {
        int timerInterval = 20;

        double fieldWidth;
        double fieldHeight;

        double fieldItemWidth;
        double fieldItemHeight;

        double destinationX;
        double destinationY;

        double ds = 2;

        Direction currentHorizontalDirection;
        Direction currentVerticalDirection;

        public Point TractorPosition { get; private set; }
        public Vector TractorVelocity { get; private set; }

        public event EventHandler TractorReachedDestination;
        public event EventHandler TractorStopped;

        Timer timer;

        public Engine(double fieldWidth, double fieldHeight, int rows, int columns)
        {
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.fieldItemWidth = fieldWidth / rows;
            this.fieldItemHeight = fieldHeight / columns;

            timer = new Timer(timerInterval);
            timer.Start();

            TractorPosition = new Point(fieldItemHeight / 2, fieldItemWidth / 2);
        }

        public void StartMovingTractorTo(int row, int column)
        {
            this.destinationX = fieldItemHeight * (column + 0.5);
            this.destinationY = fieldItemWidth * (row + 0.5);

            this.currentHorizontalDirection = DetermineDirection(this.destinationX, 
                                                                TractorPosition.X, 
                                                                Dimension.Horizontal);

            this.currentVerticalDirection = DetermineDirection(this.destinationY,
                                                               TractorPosition.Y,
                                                               Dimension.Vertical);

            timer.Elapsed += TractorMoves;
        }

        public void StopTractor()
        {
            timer.Elapsed -= TractorMoves;
            this.currentHorizontalDirection = Direction.None;
            this.currentVerticalDirection = Direction.None;

            EventHandler invoker = TractorStopped;
            if (invoker != null)
            {
                invoker(this, new EventArgs());
            }
        }

        private void TractorMoves(object sender, ElapsedEventArgs e)
        {
            if (Math.Abs(this.destinationX - TractorPosition.X) > this.ds)
            {
                MoveTractor(currentHorizontalDirection);
            }
            else if (Math.Abs(this.destinationY - TractorPosition.Y) > this.ds)
            {
                MoveTractor(currentVerticalDirection);
            }
            else
            {
                StopTractor();
                OnTractorReached();
            }
        }

        private Direction DetermineDirection(double destination, double currentPosition, Dimension dim)
        {
            if (destination < currentPosition)
            {
                switch (dim)
                {
                    case Dimension.Horizontal: return Direction.Left;
                    case Dimension.Vertical: return Direction.Up;
                    default: return Direction.None;
                }
            }
            else if (destination > currentPosition)
            {
                switch (dim)
                {
                    case Dimension.Horizontal: return Direction.Right;
                    case Dimension.Vertical: return Direction.Down;
                    default: return Direction.None;
                }              
            }
            else
            {
                return Direction.None;
            }
        }

        private void MoveTractor(Direction direction)
        {
            double dx = 0;
            double dy = 0;

            switch (direction)
            {
                case Direction.Left: dx = -(this.ds);
                    break;
                case Direction.Up: dy = -(this.ds);
                    break;
                case Direction.Right: dx = this.ds;
                    break;
                case Direction.Down: dy = this.ds;
                    break;
                case Direction.None:
                    break;
            }
            TractorPosition = new Point(TractorPosition.X + dx, TractorPosition.Y + dy);
        }

        private void OnTractorReached()
        {
            this.currentHorizontalDirection = Direction.None;
            this.currentVerticalDirection = Direction.None;

            EventHandler invoker = TractorReachedDestination;
            if (invoker != null)
            {
                invoker(this, new EventArgs());
            }
        }              
    }
}
