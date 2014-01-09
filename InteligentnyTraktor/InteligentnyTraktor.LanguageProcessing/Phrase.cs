using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    class TaskCommand
    {
        public string Value { get; set; } 
        public List<Complement> Complements { get; set; }
        public List<string> Adverbials { get; set; }

        public TaskCommand()
        {
            Complements = new List<Complement>();
            Adverbials = new List<string>();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("orzeczenie: " + Value + "\n" + "okoliczniki: ");
            foreach (var item in Adverbials)
            {
                result.Append(item + ", ");
            }
            result.Append("\ndopełenienia: ");
            foreach (var item in Complements)
            {
                result.Append("\n\t\t" + item.Value + "\n\t\t");
                foreach (var it in item.Attributes)
                {
                    result.Append(it + ", ");
                }
                result.Append("\n");
            }
            return result.ToString();
        }
    }

    class Complement
    {
        public string Value { get; set; }
        public List<string> Attributes { get; set; }

        public Complement()
        {
            Attributes = new List<string>();
        }
    }

    class Phrase
    {
        public List<TaskCommand> Tasks { get; set; }

        public Phrase()
        {
            Tasks = new List<TaskCommand>();
        }

        //dodać analizę semantyczną np można "move" do "pole" lub "sklep", ale nie można "fertilize" "magazyn" itd

        public override string ToString()
        {
            var result = "";
            foreach (var task in Tasks)
            {
                result += task.ToString() + "\n\n";
            }
            return result;
        }
    }
}
