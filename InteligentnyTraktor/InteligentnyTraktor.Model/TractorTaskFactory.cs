using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
#if (task_factory)
    static class TractorTaskFactory
    {
        internal static TractorTask Create(TractorTaskType tractorTaskType, int row, int column)
        {
            switch (tractorTaskType)
            {
                case TractorTaskType.Move:
                    return new TractorTask(new Action<int, int>(MoveTo), row, column);
                case TractorTaskType.Harvest:
                    return new TractorTask(new Action<int, int>(HarvestAt), row, column);
                case TractorTaskType.Fertilize:
                    return new TractorTask(new Action<int, int>(FertilizeAt), row, column);
                case TractorTaskType.Irrigate:
                    return new TractorTask(new Action<int, int>(IrrigateAt), row, column);
                case TractorTaskType.Sow:
                    return new TractorTask(new Action<int, int>(SowAt), row, column);
                default:
                    return null;
            }
            
        }

        private static void SowAt(int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private static void IrrigateAt(int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private static void FertilizeAt(int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private static void HarvestAt(int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private static void MoveTo(int row, int column)
        {
            engine.TractorReachedDestination += (s, e) => isTractorBusy = false;
            engine.StartMovingTractorTo(row, column);
        }
    }
#endif
}
