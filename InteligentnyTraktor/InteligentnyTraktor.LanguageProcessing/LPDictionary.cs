using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteligentnyTraktor.Model;
using System.Text.RegularExpressions;

namespace InteligentnyTraktor.LanguageProcessing
{
    public class LPDictionary
    {
        public Dictionary<String, Enum> Dict;

        IStateManager _stateManager;
        int _size;

        public LPDictionary(IStateManager stateManager, int size)
        {
            _stateManager = stateManager;
            _size = size;

            Dict = new Dictionary<String, Enum>();

            Dict.Add("Idz", TractorTaskType.Move);
            Dict.Add("Idź", TractorTaskType.Move);
            Dict.Add("Ruszaj", TractorTaskType.Move);

        }
        private bool isGoodCordinates(int r, int c)
        {
            if ((r > _size) || (c > _size))
            {
                return false;
            }
            return true;
        }
        public void CheckActionTypeAndRunIt(string commend)
        {
           
            string[] commends = commend.Split(new Char[] { ' ', ',', '.', ':', '\t' });

            string isNumber;
            int r = 0, c = 0, whichNumber;
            whichNumber = 0;
           
            List<Enum> commendsTypes = new List<Enum>();
            foreach (string com in commends)
            {

                isNumber = Regex.Match(com, @"\d+").Value;
                if ((isNumber != "") && (whichNumber != 2))
                {
                    if (whichNumber == 0)
                        r = Int32.Parse(isNumber);
                    else if (whichNumber == 1)
                        c = Int32.Parse(isNumber);
                    whichNumber++;
                }
                else if (com != "") 
                {
                    var v = Dict.FirstOrDefault(x => x.Key.Contains(com)).Value;
                  
                    commendsTypes.Add(v);
                }
            }
            if (whichNumber == 2 && isGoodCordinates(r, c) && commendsTypes.Contains(TractorTaskType.Move))
            {
                _stateManager.MoveTractorTo(r, c);
            }
        }
    }
}
