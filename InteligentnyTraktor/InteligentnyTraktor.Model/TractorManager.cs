using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    class TractorManager
    {
        Engine world;
        LinkedList<IPerformable> tasks;
        public bool isPerforming = false;

        double fieldWidth;
        double fieldHeight;

        double fieldItemWidth;
        double fieldItemHeight;

        double lastDestX;
        double lastDestY;

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

        public TractorManager(double fieldWidth, double fieldHeight, int rows, int columns)
        {
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.fieldItemWidth = fieldWidth / rows;
            this.fieldItemHeight = fieldHeight / columns;

            world = new Engine(fieldWidth, fieldHeight);

            tasks = new LinkedList<IPerformable>();
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
            var asd = tasks.Count;

            double destX = fieldItemHeight * (column + 0.5);
            double destY = fieldItemWidth * (row + 0.5);            

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
                    if (destX != lastDestX)
                    {
                        AddNewMove(destX, lastDestY);
                    }                   
                    AddNewMove(destX, destY);
                }
                else
                {
                    TractorMove lastMove = null;
                    LinkedListNode<IPerformable> lastAdded = tasks.Last;


                    while (lastAdded != null && lastMove == null)
                    {
                        lastMove = lastAdded.Value as TractorMove;
                        lastAdded = lastAdded.Next;
                    }

                    if (lastMove == null)
                    {
                        throw new Exception();
                    }

                    if (destX != lastMove.DestX)
                    {
                        AddNewMove(destX, lastMove.DestY);
                    }
                    //AddNewMove(destX, lastMove.DestY);
                    AddNewMove(destX, destY);
                }
                
            }
        }

        public void Harvest(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            AddNewTask((Action)(fieldItem.Harvest));
        }

        public void Fertilize(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            AddNewTask((Action)(fieldItem.Fertilize));
        }

        public void Irrigate(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            AddNewTask((Action)(fieldItem.Irrigate));
        }

        public void Sow(Field fieldItem, int row, int column)
        {
            MoveTractorTo(row, column);
            AddNewTask((Action)(fieldItem.Sow));
        }

        #region private methods

        private void AddNewTask(Action action)
        {
            var task = new TractorTask(action, TopTaskPerformed);
            //tasks.Enqueue(task);
            tasks.AddLast(task);

            /*
            if (tasks.Count == 1)
            {
                PerformTopTask();
            }
             */ 
        }

        private void AddNewMove(double destX, double destY)
        {
            var move = new TractorMove(world, destX, destY, world.Tractor.VMax, TopTaskPerformed);
            //tasks.Enqueue(move);
            tasks.AddLast(move);

            //if (tasks.Count == 1)
            //{
            //    PerformTopTask();
            //}
        }

        private void PerformTopTask()
        {
            isPerforming = true;
            //tasks.Dequeue().Perform();
            var topTask = tasks.First.Value;
            tasks.RemoveFirst();

            TractorMove t;
            if ((t = topTask as TractorMove) != null)
            {
                lastDestX = t.DestX;
                lastDestY = t.DestY;
            }

            topTask.Perform();
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

        #endregion
    }
}
