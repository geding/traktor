using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    class TractorMove : IPerformable
    {
        Engine world;

        public double DestX { get; set; }
        public double DestY { get; set; }
        public double MaxVelocity { get; set; }

        public event EventHandler Done;

        public TractorMove(Engine world, double destX, double destY, double maxVelocity, EventHandler DoneCallback)
        {
            this.world = world;
            this.DestX = destX;
            this.DestY = destY;
            this.MaxVelocity = maxVelocity;
            this.Done = DoneCallback;
        }

        public void Perform()
        {
            world.TractorReachedDestination += OnDone;
            world.StartMovingTractor(DestX, DestY, MaxVelocity);
        }

        private void OnDone(object sender, EventArgs e)
        {
            world.TractorReachedDestination -= OnDone;
            Done(this, EventArgs.Empty);
        }       
    }
}
