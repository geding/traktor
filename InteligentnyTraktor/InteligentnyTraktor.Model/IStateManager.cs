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
        /// Current position of tractor
        /// </summary>
        Point TractorPosition { get; }

        /// <summary>
        /// Current tractor velocity. You can use that to set angle of tractor's move 
        /// or different sprites depending on it's speed etc
        /// </summary>
        Vector TractorVelocity { get; }

        /// <summary>
        /// Raised when an action occurs inside of the single piece of field, 
        /// i.e. it has just grown, harvesting has started/finished, sow has started etc
        /// </summary>
        event FieldChangedEventHandler FieldChanged;
    }
}
