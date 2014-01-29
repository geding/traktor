using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    class Responds
    {
        private List<string> positiveResponds;
        private List<string> negativeResponds;
        private List<string> irritatedResponds;
        private short lastPositive = -1;
        private short lastNegative = -1;
        private short lastIrritated = -1;
        private short negativeRespondsCounter = 0;
        private Random r;

        public Responds()
        {
            positiveResponds = new List<string>();
            negativeResponds = new List<string>();
            irritatedResponds = new List<string>();
            r = new Random();
            AddResponds();
        }

        public string GetPositiveRespond()
        {
            int i;
            do 
            {
                i = r.Next(positiveResponds.Count);
            } while (i == lastPositive);

            lastPositive = (short)i;
            negativeRespondsCounter = 0;
            return positiveResponds[i];
        }

        public string GetNegativeRespond()
        {
            if (negativeRespondsCounter >= 5)
            {
                return GetIrritatedRespond();
            }

            int i;
            do
            {
                i = r.Next(negativeResponds.Count);
            } while (i == lastNegative);

            lastNegative = (short)i;
            negativeRespondsCounter++;
            return negativeResponds[i];
        }

        private string GetIrritatedRespond()
        {
            int i;
            do
            {
                i = r.Next(irritatedResponds.Count);
            } while (i == lastIrritated);

            lastIrritated = (short)i;
            return irritatedResponds[i];
        }

        private void AddResponds()
        {
            positiveResponds.Add("Jasne, już się robi!");
            positiveResponds.Add("Nie ma co zwlekać, robota sama się nie załatwi.");
            positiveResponds.Add("Nie ma problemu, szefie.");
            positiveResponds.Add("Pewnie, ty tu rządzisz.");
            positiveResponds.Add("Dobrze, zaraz to załatwię, szefie.");
            positiveResponds.Add("Uwinę się w try miga!");
            positiveResponds.Add("Uznaj to za zrobione, szefie.");
            positiveResponds.Add("Jeden problem z twojej głowy mniej.");
            positiveResponds.Add("Bezzwłocznie, szefie!");
            positiveResponds.Add("Niech szef sobie odpocznie, a ja zrobię co trzeba.");
            positiveResponds.Add("Wspaniale! To właśnie lubię!");

            negativeResponds.Add("Szefie, mógłbyś wyrażać się jaśniej?");
            negativeResponds.Add("Jak to? Coś mi się tu nie zgadza.");
            negativeResponds.Add("Nie potrafię tego zrobić...");
            negativeResponds.Add("Proszę, sprecyzuj co masz na myśli.");
            negativeResponds.Add("Obawiam się, że nie umiem wykonać tego zadania, szefie.");
            negativeResponds.Add("Wybacz, nie nadążam za tobą, szefie.");

            irritatedResponds.Add("Mam dośc tych zgadywanek! Wyłóż kawę na ławę.");
            irritatedResponds.Add("Zaczynasz mnie irytować...");
            irritatedResponds.Add("Gadaj do rzeczy.");
            irritatedResponds.Add("Ty tu gadu-gadu, a robota stoi.");
            irritatedResponds.Add("Przestań, bądźmy poważni.");
            irritatedResponds.Add("Jestem traktorem, który ma dużo cierpliwości, ale powoli zaczyna mi jej brakować...");
        }
    }
}
