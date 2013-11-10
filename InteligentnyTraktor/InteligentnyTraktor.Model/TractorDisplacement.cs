using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    class TractorDisplacement
    {
        public double DestX { get; private set; }
        public double DestY { get; private set; }
        public double MaxVelocity { get; private set; }

        public TractorDisplacement(double destX, double destY, double maxVelocity)
        {
            this.DestX = destX;
            this.DestY = destY;
            this.MaxVelocity = maxVelocity;
        }
    }
}
