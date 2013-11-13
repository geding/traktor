using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteligentnyTraktor.Model;

namespace InteligentnyTraktor.LanguageProcessing
{
    class LPDictionary
    {
        public Dictionary<String, Enum> Dict;

        LPDictionary()
        {
            Dict = new Dictionary<String, Enum>();

            Dict.Add("Idz", TractorTaskType.Move);
            Dict.Add("Idź", TractorTaskType.Move);
            Dict.Add("Rusz", TractorTaskType.Move);

        }
    }
}
