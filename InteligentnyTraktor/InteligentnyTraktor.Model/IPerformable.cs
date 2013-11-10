using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    interface IPerformable
    {
        void Perform();
        event EventHandler Done;
    }
}
