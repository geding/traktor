using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace InteligentnyTraktor.Model
{
    /// <summary>
    /// Main class managing state of the fields, position of the tractor etc.
    /// </summary>
    public interface IStateManager
    {
        /// <summary>
        /// Moves tractor to field specified by row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void MoveTractorTo(int row, int column);

        /// <summary>
        /// Sends tractor to piece of field specified by row and column and harvests it
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void HarvestAt(int row, int column);

        /// <summary>
        /// Sends tractor to piece of field specified by row and column and fertilizes it
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void FertilizeAt(int row, int column);

        /// <summary>
        /// Sends tractor to piece of field specified by row and column and irrigates it
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void IrrigateAt(int row, int column);

        /// <summary>
        /// Sends tractor to piece of field specified by row and column and plows it
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void PlowAt(int row, int column);

        /// <summary>
        /// Sends tractor to piece of field specified by row and column and sows it
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void SowAt(int row, int column);

        /// <summary>
        /// Stops tractor's movement or work
        /// </summary>
        void StopTractor();

        //TODO
        //void AddToOrdersList(TractorAction action);
        //delegate TractorAction

        /// <summary>
        /// Current position of tractor
        /// </summary>
        Point TractorPosition { get; }

        /// <summary>
        /// Current field with tractor
        /// </summary>
        Point FieldWithTractor { get; }

        /// <summary>
        /// Current tractor velocity. You can use that to set angle of tractor's move 
        /// or different sprites depending on it's speed etc
        /// </summary>
        Vector TractorDirection { get; }

        /// <summary>
        /// Raised when an action occurs inside of the single piece of field, 
        /// i.e. it has just grown, harvesting has started/finished, sow has started etc
        /// </summary>
        event FieldChangedEventHandler FieldChanged;

        /// <summary>
        /// Tractor works only on one thing at once, if you ordered it something else
        /// before it would finish this event is raised.
        /// Use StopTractor() and give new orders, or use AddToOrdersList() instead 
        /// </summary>
        event EventHandler TractorIsBusy;
    }
}
