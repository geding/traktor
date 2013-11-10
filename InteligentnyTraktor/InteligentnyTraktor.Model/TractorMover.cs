using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    class TractorMover
    {
        Engine world;
        Queue<TractorDisplacement> moves;

        public event EventHandler HasMoved;

        public TractorMover(Engine world, EventHandler hasMoved)
        {
            this.world = world;
            this.HasMoved = hasMoved;

            world.TractorReachedDestination += OnMoved;
        }

        public void Move(TractorDisplacement move)
        {
            
        }

        public void AddMove(TractorDisplacement move)
        {
            moves.Enqueue(move);
        }

        public void PerformTopMove()
        {
            TractorDisplacement m = moves.Dequeue();
            world.StartMovingTractor(m.DestX, m.DestY, m.MaxVelocity);
        }

        private void OnMoved(object sender, EventArgs e)
        {
            //world.TractorReachedDestination -= this.OnDone;
            HasMoved(this, EventArgs.Empty);
        }
    }
}
