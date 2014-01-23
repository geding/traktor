using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    public enum FieldItemState
    {
        Bare,  //gołe
        Plowed,  //zaorane
        Sowed,  //zasiane
        EarlyGrowing,
        MidGrowing,
        LateGrowing,
        Mature,        
        Rotten, //zgniłe
        Harvested,  //zebrany plon
    }
}
