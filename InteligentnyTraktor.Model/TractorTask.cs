using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    class TractorTask : IPerformable
    {
        Action task;

        public event EventHandler Done;
        //EventHandler raiser;

        /*
        public TractorTask(Action task, EventHandler EventDoneRaiser, EventHandler DoneCallback)
        {
            this.task = task;
            this.Done += DoneCallback;
            this.raiser = EventDoneRaiser;           
        }
         */

        public TractorTask(Action task, EventHandler DoneCallback)
        {
            this.task = task;
            this.Done += DoneCallback;
        }

        public void Perform()
        {
            //if (raiser != null)
            //{
            //    Listen();
            //}

            task();
            Done(this, EventArgs.Empty);

            //if (raiser == null && Done != null)
            //{
            //    Done(this, EventArgs.Empty);
            //}            
        }

        //private void listen()
        //{
        //    raiser += callback;
        //}

        //private void callback(object sender, eventargs e)
        //{
        //    done(this, eventargs.empty);
        //}
    }
}
