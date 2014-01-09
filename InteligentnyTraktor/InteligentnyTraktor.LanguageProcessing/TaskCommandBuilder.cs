using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    class TaskCommandBuilder
    {
        TaskCommand task;

        public TaskCommandBuilder()
        {
            task = new TaskCommand();
        }

        public void SetTaskWord(string word)
        {
            task.Value = word;
        }

        public void AppendComplement(string value, IEnumerable<string> attributes)
        {
            Complement c = new Complement()
            {
                Value = value,
                Attributes = attributes.ToList()
            };

            task.Complements.Add(c);
        }

        public void AppendAdverbial(string adverbial)
        {
            task.Adverbials.Add(adverbial);
        }

        public TaskCommand CreateTaskCommand(string value, IEnumerable<Complement> complements, IEnumerable<string> adverbials)
        {
            var tc = new TaskCommand()
            {
                Value = value,
                Complements = complements.ToList(),
                Adverbials = adverbials.ToList(),
            };

            return tc;
        }

        public TaskCommand Build()
        {
            return task;
        }
    }
}
