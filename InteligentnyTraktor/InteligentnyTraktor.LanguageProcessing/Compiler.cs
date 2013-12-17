using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    public class Compiler 
    {
        IStateManager _stateManager;
        int _size;
      
        public Compiler(IStateManager stateManager, int size)
        {
            _stateManager = stateManager;
            _size = size;
        }
        public void RunCompiler(string commend)
        {
           Execute(ParseCommend(commend));
        }
        private Phrase ParseCommend(string commend)
        {
            var parser = new Parser(new HardCodedDictionary());
            Phrase phrase = parser.TryParse(parser.Scan(commend));
            return phrase;
        }

        private void Execute(Phrase phrase)
        {
            foreach (var task in phrase.Tasks)
            {
                
                System.IO.StreamWriter file = new System.IO.StreamWriter("f:\\test.txt", true);
                file.WriteLine("task v:"+task.Value); //to jest co ma zrobic
                // potrzebna cala tablica z fildami na ktorych maja byc wykonywane polecenia



                foreach (var Adverb in task.Adverbials)
                {
                    file.WriteLine("adv: "+Adverb);    // to jest kolejnosc (nastepne)
                }
                foreach (var complement in task.Complements)
                {
                    file.WriteLine(complement.Value); //to jest na czym ma zrobic (szczegoly  w filtrach)
                    foreach(var atr in complement.Attributes)
                    {
                        file.WriteLine("compl atr:"+atr); // to sa filtry
                    }
                }
                 file.Close();
            }
        }
    }
}
