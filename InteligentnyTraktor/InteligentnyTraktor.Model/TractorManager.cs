using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteligentnyTraktor.Model
{
    class TractorManager
    {
        Engine world;
        Queue<IPerformable> tasks;
        public bool isPerforming = false;

        double fieldWidth;
        double fieldHeight;

        double fieldItemWidth;
        double fieldItemHeight;

        double lastMoveDestX;
        double lastMoveDestY;

        public System.Windows.Point Position
        {
            get { return world.Tractor.Position; }
        }

        public System.Windows.Vector Direction
        {
            get { return world.Tractor.Direction; }
        }

        public Tractor Tractor
        {
            get { return world.Tractor; }
        }
        public Point FieldWithTractor()
        {
            var row =  world.Tractor.Position.X / this.fieldItemWidth - 0.5;
            var column =  world.Tractor.Position.Y / this.fieldItemHeight - 0.5;
            return new Point(row, column);
        }
        public TractorManager(double fieldWidth, double fieldHeight, int rows, int columns)
        {
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.fieldItemWidth = fieldWidth / rows;
            this.fieldItemHeight = fieldHeight / columns;

            world = new Engine(fieldWidth, fieldHeight);

            tasks = new Queue<IPerformable>();
        }

        public void StopTractor()
        {
            tasks.Clear();
            world.StopTractor();
            world.ResetTractorReachedEvent();
            isPerforming = false;
            //engine.TractorStopped += (s, e) => isTractorBusy = false;
            //engine.StopTractor();
        }

        public void MoveTractorTo(int row, int column)
        {
            //var asd = tasks.Count;
            double destX = fieldItemHeight * (column + 0.5);
            double destY = fieldItemWidth * (row + 0.5);

            MoveTractor(destX, destY);
        }

        public void Harvest(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            MoveTractorRoundField(row, column);
            AddNewTask((Action)(fieldItem.Harvest));
        }

        public void Fertilize(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            MoveTractorRoundField(row, column);
            AddNewTask((Action)(fieldItem.Fertilize));
        }

        public void Irrigate(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            MoveTractorRoundField(row, column);
            AddNewTask((Action)(fieldItem.Irrigate));
        }

        public void Plow(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            MoveTractorRoundField(row, column);
            AddNewTask((Action)(fieldItem.Plow));
        }

        public void Sow(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            MoveTractorRoundField(row, column);
            AddNewTask((Action)(fieldItem.Sow));
        }

        #region private methods

        private void AddNewTask(Action action)
        {
            var task = new TractorTask(action, TopTaskPerformed);
            tasks.Enqueue(task); 
        }

        private void AddNewMove(double destX, double destY)
        {
            this.lastMoveDestX = destX;
            this.lastMoveDestY = destY;

            var move = new TractorMove(world, destX, destY, world.Tractor.VMax, TopTaskPerformed);
            tasks.Enqueue(move);
        }

        private void PerformTopTask()
        {
            isPerforming = true;
            tasks.Dequeue().Perform();
        }

        private void TopTaskPerformed(object sender, EventArgs e)
        {
            //performs next task in the queue when last task has finished
            if (tasks.Count != 0)
            {
                PerformTopTask();
            }
            else
            {
                isPerforming = false;
            }
        }

        private void MoveTractorRoundField(int row, int column)
        {
            MoveTractorTo(row, column);

            //najpierw lekko do gory
            double destX = fieldItemHeight * (column + 0.5);
            double destY = fieldItemWidth * (row + 0.2);
            MoveTractor(destX, destY);

            //potem w lewo
            destX = fieldItemHeight * (column + 0.2);
            MoveTractor(destX, destY);

            //potem w dół
            destY = fieldItemWidth * (row + 0.8);
            MoveTractor(destX, destY);

            //potem w prawo
            destX = fieldItemHeight * (column + 0.8);
            MoveTractor(destX, destY);

            //potem do góry
            destY = fieldItemWidth * (row + 0.2);
            MoveTractor(destX, destY);

            //potem do polowy w lewo
            destX = fieldItemHeight * (column + 0.5);
            MoveTractor(destX, destY);

            //i w dół na środek pola
            destY = fieldItemWidth * (row + 0.5);
            MoveTractor(destX, destY);
        }

        private void MoveTractor(double destX, double destY)
        {
            if (isPerforming == false)
            {
                if (destX == world.Tractor.Position.X || destY == world.Tractor.Position.Y)
                {
                    AddNewMove(destX, destY);
                }
                else
                {
                    AddNewMove(destX, world.Tractor.Position.Y);
                    AddNewMove(destX, destY);
                }
                PerformTopTask();
            }
            else
            {
                if (tasks.Count == 0)
                {
                    if (destX != lastMoveDestX)
                    {
                        AddNewMove(destX, lastMoveDestY);
                    }
                    AddNewMove(destX, destY);
                }
                else
                {
                    if (destX != this.lastMoveDestX)
                    {
                        AddNewMove(destX, this.lastMoveDestY);
                    }
                    //AddNewMove(destX, lastMove.DestY);
                    AddNewMove(destX, destY);
                }

            }
        }

        #endregion
    }
}
