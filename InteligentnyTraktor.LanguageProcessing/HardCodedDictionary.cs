using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    public class HardCodedDictionary : ITheDictionary
    {
        public Char[] Delimiters { get; private set; }
        public Dictionary<string, string> TaskWordsRepository { get; private set; }
        public Dictionary<string, string> ComplementWordsRepository { get; private set; } //dopełnienia
        public Dictionary<string, string> AttributeWordsRepository { get; private set; } //przydawki, określenia rzeczownika
        public Dictionary<string, string> AdverbialWordsRepository { get; private set; } //okoliczniki, określenia czasownika
        public ICollection<string> IgnoredWords { get; private set; }
        public ICollection<string> ConjuctionWords { get; private set; }
        public Dictionary<string, List<TwoWordExpression>> CommonWordIndexedTwoWordExpressions { get; private set; }
        public ICollection<string> CommonWordsForTwoWordExpressions { get; private set; }
        public string DefaultComplementWord { get; private set; }

        public  HardCodedDictionary()
        {
            this.TaskWordsRepository = new Dictionary<string, string>();
            this.ComplementWordsRepository = new Dictionary<string, string>();
            this.AttributeWordsRepository = new Dictionary<string, string>();
            this.AdverbialWordsRepository = new Dictionary<string, string>();

            Delimiters = new char[] { ' ', '.', ':', ';', '\t', '\n' };
            DefaultComplementWord = "domyślne";

            //initialize task words repository
            string[] move = { "się udać", "udać się", "pojechać", "pojechac", "pójść", "przesunąć się", "się przesunąć", "przesuń się", "idź", "idz", "rusz", "ruszaj", "jedź", 
                            "pojedź", "pojedz", "zawieź", "zawiex", "podążaj", "udaj się", "pofatyguj się", "kopsnij się", "wyrusz", "wróć" };

            string[] stop = { "zatrzymaj", "zatrzymaj się", "zatrzymać się", "się zatrzymać", "stop", "stój", "stoj", "stać", "stac", "przestań", "przestan", "przerwij", "zastopuj", "zahamuj" };

            string[] plow = { "zaorać", "zaorac", "zaoraj", "spulchnij", "przeorać", "przeorac", "przeoraj" };

<<<<<<< HEAD:InteligentnyTraktor.LanguageProcessing/HardCodedDictionary.cs
            string[] sow = { "zasiać", "zasiej", "obsiej", "posiej" };
=======
            string[] sow = { "zasiać", "zasiej", "obsiej", };
>>>>>>> remotes/origin/compiler:InteligentnyTraktor/InteligentnyTraktor.LanguageProcessing/HardCodedDictionary.cs

            string[] harvest = { "zbierz", "zebrać", "zbieraj", "skoś" };

            string[] irrigate = { "podlej" };

            string[] start = { "zacznij", "rozpocznij", "start" };

            foreach (var word in move)
            {
                this.TaskWordsRepository.Add(word, "jedź");
            }
            foreach (var word in stop)
            {
                this.TaskWordsRepository.Add(word, "stop");
            }
            foreach (var word in plow)
            {
                this.TaskWordsRepository.Add(word, "zaoraj");
            }
            foreach (var word in sow)
            {
                this.TaskWordsRepository.Add(word, "zasiej");
            }
            foreach (var word in harvest)
            {
                this.TaskWordsRepository.Add(word, "zbierz");
            }
            foreach (var word in irrigate)
            {
                this.TaskWordsRepository.Add(word, "podlej");
            }
            foreach (var word in start)
            {
                this.TaskWordsRepository.Add(word, "start");
            }

            //initialize complement words repo
            this.ComplementWordsRepository.Add("pole", "pole");
            this.ComplementWordsRepository.Add("pól", "pola");
            this.ComplementWordsRepository.Add("pola", "pola");
            this.ComplementWordsRepository.Add("magazyn", "magazyn");
            this.ComplementWordsRepository.Add("magazynu", "magazyn");
            this.ComplementWordsRepository.Add("traktor", "traktor");
            this.ComplementWordsRepository.Add("je", "domyślne");

            // czego to dotyczy? :
            this.ComplementWordsRepository.Add("siać", "siać");
            this.ComplementWordsRepository.Add("zasiewać", "siać");
            this.ComplementWordsRepository.Add("zasiewanie", "siać");

            //initialize attribute words repo
            this.AttributeWordsRepository.Add("wszystkie", "wszystkie");
            this.AttributeWordsRepository.Add("każde", "każde"); //wszystkie?
            this.AttributeWordsRepository.Add("zaorane", "zaorane");
            this.AttributeWordsRepository.Add("niezaorane", "niezaorane");
            this.AttributeWordsRepository.Add("niezaoranego", "niezaorane");
            this.AttributeWordsRepository.Add("zasiane", "zasiane");
            this.AttributeWordsRepository.Add("zgniłe", "zgniłe");
            this.AttributeWordsRepository.Add("uschnięte", "zgniłe");
            this.AttributeWordsRepository.Add("w pobliżu", "niedaleko");
            this.AttributeWordsRepository.Add("obok", "niedaleko");
            this.AttributeWordsRepository.Add("pobliskie", "niedaleko");
            this.AttributeWordsRepository.Add("niedaleko", "niedaleko");
            this.AttributeWordsRepository.Add("kukurydze", "kukurydzy");
            this.AttributeWordsRepository.Add("kukurydzy", "kukurydzy");
            this.AttributeWordsRepository.Add("kukurydziane", "kukurydzy");
            this.AttributeWordsRepository.Add("pszenicy", "pszenicy");
            this.AttributeWordsRepository.Add("pszeniczne", "pszenicy");
            this.AttributeWordsRepository.Add("pszenicę", "pszenicy");
            this.AttributeWordsRepository.Add("pszenice", "pszenicy");
            this.AttributeWordsRepository.Add("0", "0");
            this.AttributeWordsRepository.Add("zero", "0");
            this.AttributeWordsRepository.Add("1", "1");
            this.AttributeWordsRepository.Add("2", "2");
            this.AttributeWordsRepository.Add("3", "3");
            this.AttributeWordsRepository.Add("4", "4");




            //initialize adverbial words repository
            string[] then = { "a potem", "potem", "a następnie", "a nastepnie", "następnie", "nastepnie", "a później", "później", "a pozniej", "a poxniej", "pozniej", "poxniej", "po czym", };
            string[] now = { "natychmiast", "teraz", "niezwłocznie" };
            string[] conditional = { "gdy", "kiedy", "jeżeli" };//może się to doda, ale opcja generalnie

            foreach (var word in then)
            {
                this.AdverbialWordsRepository.Add(word, "następnie");
            }
            foreach (var word in now)
            {
                this.AdverbialWordsRepository.Add(word, "natychmiast");
            }
            foreach (var word in conditional)
            {
                this.AdverbialWordsRepository.Add(word, "jeżeli");
            }


            this.IgnoredWords = new List<string>()
            {
                "proszę", "czy", "mógłbyś", 
            };

            this.ConjuctionWords = new List<string>()
            {
                ",", "i", "a", ", a", ", i",
            };

            this.CommonWordsForTwoWordExpressions = new List<string>()
            {
                "się", "po", "na", ",", "w",
            };

            this.CommonWordIndexedTwoWordExpressions = new Dictionary<string, List<TwoWordExpression>>()
            {
                { "się", new List<TwoWordExpression>()
                    {
                        new TwoWordExpression("się", "przesunąć", Order.DoesntMatter),
                        new TwoWordExpression("się", "przesuń", Order.OtherWordFirst),
                        new TwoWordExpression("się", "zatrzymaj", Order.DoesntMatter),
                        new TwoWordExpression("się", "zajmij", Order.DoesntMatter),
                        new TwoWordExpression("się", "udaj", Order.OtherWordFirst),
                        new TwoWordExpression("się", "udać", Order.DoesntMatter)
                    }
                },
                { "po", new List<TwoWordExpression>()
                    {
                        new TwoWordExpression("po", "czym", Order.CommonWordFirst),
                        new TwoWordExpression("po", "lewej", Order.CommonWordFirst),
                        new TwoWordExpression("po", "prawej", Order.CommonWordFirst),
                    }
                },
                { "na", new List<TwoWordExpression>()
                    {
                        new TwoWordExpression("na", "górze", Order.CommonWordFirst),
                        new TwoWordExpression("na", "dole", Order.CommonWordFirst),
                    }
                },
                { ",", new List<TwoWordExpression>()
                    {
                        new TwoWordExpression(",", "a", Order.CommonWordFirst),
                        new TwoWordExpression(",", "i", Order.CommonWordFirst),
                    }
                },
                { "w", new List<TwoWordExpression>()
                    {
                        new TwoWordExpression("w", "pobliżu", Order.CommonWordFirst),
                    }
                },
            };
        }
    }
}
