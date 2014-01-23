using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.Model
{
    class TractorDictionary
    {
        public Dictionary<String, Enum> Dict;

        TractorDictionary()
        {
            Dict = new Dictionary<String, Enum>();

            Dict.Add("Idz", TractorTaskType.Move);
            Dict.Add("Idź", TractorTaskType.Move);
            Dict.Add("Rusz", TractorTaskType.Move);

        }

        

        

    }
}
