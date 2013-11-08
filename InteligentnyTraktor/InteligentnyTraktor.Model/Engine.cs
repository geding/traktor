//#define locked

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

        public Tractor Tractor { get; private set; }

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

            Tractor = new Tractor()
            {
                Position = new Point(fieldItemHeight / 2, fieldItemWidth / 2),
                Velocity = new Vector(0, 0),
                VMax = 2,
                Acceleration = 0.02,
            };           
        }

        public void StartMovingTractorTo(int row, int column)
        {

            this.destinationX = fieldItemHeight * (column + 0.5);
            this.destinationY = fieldItemWidth * (row + 0.5);

            this.currentHorizontalDirection = DetermineDirection(this.destinationX, 
                                                                Tractor.Position.X, 
                                                                Dimension.Horizontal);

            this.currentVerticalDirection = DetermineDirection(this.destinationY,
                                                               Tractor.Position.Y,
                                                               Dimension.Vertical);
            timer.Elapsed += TractorMoves;
        }

        public void StopTractor()
        {
            Tractor.Velocity = new Vector(0, 0);

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
            Tractor.Direction = Tractor.Velocity;
            
#if (locked)
            #region locked
            lock (Tractor)
            {
                if (Math.Abs(this.destinationX - Tractor.Position.X) > this.ds)
                {
                    if (Tractor.Velocity == new Vector(0, 0))
                    {
                        Tractor.Velocity = this.currentHorizontalDirection == Direction.Left
                                           ? new Vector(-2, 0)
                                           : new Vector(2, 0);
                    }
                    //MoveTractor(currentHorizontalDirection);
                    Tractor.Move(ds);
                    if (Tractor.Velocity.LengthSquared < Tractor.VMax)
                    {
                        Tractor.Accelerate();
                    }
                }
                else if (Math.Abs(this.destinationY - Tractor.Position.Y) > this.ds)
                {
                    if (Math.Abs(Tractor.Velocity.X) == 2 || Tractor.Velocity.X == 0)
                    {
                        Tractor.Velocity = this.currentVerticalDirection == Direction.Up
                                          ? new Vector(0, -2)
                                          : new Vector(0, 2);
                    }
                    Tractor.Move(ds);
                }
                else
                {
                    StopTractor();
                    OnTractorReached();
                }
            }
            #endregion
#else

            if (Math.Abs(this.destinationX - Tractor.Position.X) > this.ds)
            {
                if (Tractor.Velocity == new Vector(0, 0))
                {
                    Tractor.Velocity = this.currentHorizontalDirection == Direction.Left
                                       ? new Vector(-2, 0)
                                       : new Vector(2, 0);
                }
                Tractor.Move(ds);
            }
            else if (Math.Abs(this.destinationY - Tractor.Position.Y) > this.ds)
            {
                if (Math.Abs(Tractor.Velocity.X) == 2 || Tractor.Velocity.X == 0)
                {
                    Tractor.Velocity = this.currentVerticalDirection == Direction.Up
                                      ? new Vector(0, -2)
                                      : new Vector(0, 2);
                }
                Tractor.Move(ds);
            }
            else
            {
                StopTractor();
                OnTractorReached();
            }
#endif
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
            Tractor.Position = new Point(Tractor.Position.X + dx, Tractor.Position.Y + dy);
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

        internal void ResetTractorReachedEvent()
        {
            TractorReachedDestination = null;
        }
    }
}
