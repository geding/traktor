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

        public  HardCodedDictionary()
        {
            this.TaskWordsRepository = new Dictionary<string, string>();
            this.ComplementWordsRepository = new Dictionary<string, string>();
            this.AttributeWordsRepository = new Dictionary<string, string>();
            this.AdverbialWordsRepository = new Dictionary<string, string>();

            Delimiters = new char[] { ' ', '.', ':', ';', '\t', '\n' };

            //initialize task words repository
            string[] move = { "się udać", "udać się", "pojechać", "pojechac", "pójść", "przesunąć się", "się przesunąć", "przesuń się", "idź", "idz", "rusz", "ruszaj", "jedź", 
                            "pojedź", "pojedz", "zawieź", "zawiex", "podążaj", "udaj się", "pofatyguj się", "kopsnij się", "wyrusz", "wróć" };

            string[] stop = { "zatrzymaj", "zatrzymaj się", "zatrzymać się", "się zatrzymać", "stop", "stój", "stoj", "stać", "stac", "przestań", "przestan", "przerwij", "zastopuj", "zahamuj" };

            string[] plow = { "zaorać", "zaorac", "zaoraj", "spulchnij", "przeorać", "przeorac", "przeoraj" };

            string[] sow = { "zasiać", "zasiej", "obsiej", };

            string[] harvest = { "zbierz", "zebrać", "zbieraj", "skoś" };

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

            //initialize complement words repo
            this.ComplementWordsRepository.Add("pole", "pole");
            this.ComplementWordsRepository.Add("pola", "pola");
            this.ComplementWordsRepository.Add("magazyn", "magazyn");
            this.ComplementWordsRepository.Add("magazynu", "magazyn");
            this.ComplementWordsRepository.Add("traktor", "traktor");

            //initialize attribute words repo
            this.AttributeWordsRepository.Add("wszystkie", "wszystkie");
            this.AttributeWordsRepository.Add("każde", "wszystkie");
            this.AttributeWordsRepository.Add("zaorane", "zaorane");
            this.AttributeWordsRepository.Add("niezaorane", "niezaorane");


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
                ",", "i", "a", ", a"
            };

            this.CommonWordsForTwoWordExpressions = new List<string>()
            {
                "się", "po", "na", ","
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

                    }
                },
            };
        }

        /*
        public static ITheDictionary GetDictionary()
        {
            var Delimiters = new char[]{ ' ', '.', ':', ';', '\t', '\n' };

            var TaskWordsRepository = new Dictionary<string, ICollection<string>>()
            {
                { 
                    "jedź", new string[]
                        { "udać", "pojechać", "pojechac", "pójść", "przesunąć się", "się przesunąć", "przesuń się", "idź", "idz", "rusz", "ruszaj", "jedź", 
                        "pojedź", "pojedz", "zawieź", "zawiex", "podążaj", "udaj", "pofatyguj się", "kopsnij się", "wyrusz", "wróć" }
                },
                {
                    "stop", new string[] 
                        { "zatrzymaj się", "zatrzymać się", "się zatrzymać", "stop", "stój", "stoj", "stać", "stac", "przestań", "przestan", "przerwij", "zastopuj", "zahamuj" }
                },
                {
                    "zaoraj", new string[]
                        { "zaorać", "zaorac", "zaoraj", "spulchnij", "przeorać", "przeorac", "przeoraj" }
                },
                {
                    "harvest", new string[] 
                        {}
                },
                {
                    "fertilize", new string[]
                        {}
                },
                {
                    "irrigate", new string[]
                        {}
                },
                {
                    "sow", new string[]
                        {}
                },
            };
            var ComplementWordsRepository = new Dictionary<string, string>();
            var IgnoredWords = new string[1];
            var ConjuctionWords = new string[1];
            
            //var MultiwordConnections = new 

            return new ITheDictionary(
                Delimiters,
                TaskWordsRepository,
                ComplementWordsRepository,
                IgnoredWords,
                ConjuctionWords
            );
         
        }
        */
    }
}
