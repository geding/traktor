using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    class Phrase
    {
        //dodać analizę semantyczną np można "move" do "pole" lub "sklep", ale nie można "fertilize" "magazyn" itd
        List<TractorTaskType> Tasks { get; set; }

        Dictionary<string, List<string>> Complements { get; set; } //indexed by task
        Dictionary<string, List<string>> ComplementsAttributes { get; set; } //indexed by complement
    }
}
