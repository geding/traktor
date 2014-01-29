/*
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
            string[] move = { "się udać", "udać się", "się udac", "sie udac", "udax sie", "udav sie", "sie udax", "sie udav", "udaj sie",
                              "pojechać", "pojechac", "pojechav", "pojechax", "pojedz", "pojedź", "pojehac", "pojehać", "pojeć",
                              "pójść", "pojsc", "pójśc", "pójsć", "pujść", "pujsc", "pójśv", "pójśx", "pójdć", "pójać", "pójdź", "pójdx", "pojdx", "pojdz",
                              "przesunąć się", "się przesunąć", "przesuń się", "przesun sie", "pżesuń się",
                              "idź", "idz", "idx", "idc",
                              "rusz", "ruszaj", "rusz sie", "sie rusz", "rósz sie", "rosz sie", "rusx", "ruaz", "rudz", "rusc",
                              "zawieź", "zawiex", "zawiec", "zawies", "zawied",
                              "podążaj", "podazaj", "podązaj", "podażaj", "podąrzaj", "podarzaj", "udaj się", "pofatyguj się", "pofatygój się",
                              "pofatyguj sie", "pofatygój sie","kopsnij się", "wyrusz", "wróć", "wroc", "wruć", "wruc", "wróv", "wróx", "wrov", "wrox",
                              "zapieprzaj", "zapieprzak", "zapieprzah", "zapoeiprzaj", "zpaieprzaj", "zapierdalaj", 
                              "zapierdalak", "zapierdalah", "zpaierdalaj", "zapierdzielaj", "podrózuj", "podróżuj", "podrużuj", "podrórzuj", "podróżui",
                              "mknij", "mknać", "pędź", "pedź", "pedz", "popędzaj", "popedzaj", "gazuj", "szoruj"
                            };

            string[] stop = {  "zatrzymaj", "zatrzymaj się", "zatrzymać się", "się zatrzymać", "zatrzymak", "ztatzymaj", "zatrzymaj sei", "halt", 
                               "stop", "sotp", "spot", "sopt", "stój", "stoj", "stać", "stac", "stuj",
                               "przestań", "przestan", "przerwij", "abort",
                               "zastopuj", "zahamuj", "zachamuj", "zahamój", "zachamój", "zachamoj", "trwaj w bezruchu"
                            };
            string[] plow = { "zaorać", "zaorac", "zaoraj", "zaraj", "spulchnij", "przeorać", "przeorac", "przeoraj",
                                "oraj", "orac", "orav", "orax", "oarc", "orca" };

            string[] sow = { "zasiać", "zasiac", "zasaic", "zsaiac", "siej", "seij", "siei", "posiej", "psoiej", "zasiej",
                               "zsaiej", "obsiej", "osbiej", "zaisiej", "zaisiać" };

            string[] harvest = { "zbierz", "zbiezr", "zbież", "zbier", "zebrać", "zbieraj", "skoś", "skos", "tnij", "skroc", "skruc", "skruv", "skruz", "obstrzyż",
                                   "obstrzyrz", "obetnij" };

            string[] irrigate = { "podlej", "podlje", "pdolej", "nawodnij", "nwaodnij", "nawodnj", "nawilż", "nawilz", "lej", "natryskaj", "ntaryskaj",
                                    "natryskah", "natryskal", "napój", "napoj", "napoi", "napuj", "napui", "npaój", "skrop", "skrapiać", "iryguj", "irygój",
                                    "iyrguj", "iyrgój", "irygui", "ztryskaj", "stryskaj" };

            string[] start = { "zacznij", "zacząć", "zaczac", "zacząc", "zaczać", "zaskocz", "zaskozc", "zsakocz", "rozpocznij", "start", "begin", "poczatek" };
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
            this.ComplementWordsRepository.Add("ziemia", "pola");
            this.ComplementWordsRepository.Add("ziemie", "pola");
            this.ComplementWordsRepository.Add("grunt", "pola");
            this.ComplementWordsRepository.Add("poletko", "pola");
            this.ComplementWordsRepository.Add("łan", "pola");
            this.ComplementWordsRepository.Add("użytek", "pola");
            this.ComplementWordsRepository.Add("pólko", "pola");
            this.ComplementWordsRepository.Add("zagon", "pola");
            this.ComplementWordsRepository.Add("rola", "pola");
            this.ComplementWordsRepository.Add("niwa", "pola");
            this.ComplementWordsRepository.Add("rozłóg", "pola");
            this.ComplementWordsRepository.Add("obszar", "pola");
            this.ComplementWordsRepository.Add("magazyn", "magazyn");
            this.ComplementWordsRepository.Add("skład", "magazyn");
            this.ComplementWordsRepository.Add("składnica", "magazyn");
            this.ComplementWordsRepository.Add("składzik", "magazyn");
            this.ComplementWordsRepository.Add("magazynek", "magazyn");
            this.ComplementWordsRepository.Add("przechowywalnia", "magazyn");
            this.ComplementWordsRepository.Add("schowek", "magazyn");
            this.ComplementWordsRepository.Add("magazynu", "magazyn");
            this.ComplementWordsRepository.Add("traktor", "traktor");
            this.ComplementWordsRepository.Add("ciągnik", "traktor");
            this.ComplementWordsRepository.Add("ciagnik", "traktor");
            this.ComplementWordsRepository.Add("je", "domyślne");
            this.ComplementWordsRepository.Add("to", "domyślne");
            this.ComplementWordsRepository.Add("aktualne", "domyślne");
            this.ComplementWordsRepository.Add("siać", "siać");
            this.ComplementWordsRepository.Add("zasiewać", "siać");
            this.ComplementWordsRepository.Add("zasiewanie", "siać");
            this.ComplementWordsRepository.Add("zasiać", "siać");
            this.ComplementWordsRepository.Add("zasiac", "siać");
            this.ComplementWordsRepository.Add("siej", "siać");
            this.ComplementWordsRepository.Add("posiać", "siać");
            this.ComplementWordsRepository.Add("posiej", "siać");
            this.ComplementWordsRepository.Add("zasiej", "siać");
            this.ComplementWordsRepository.Add("obsiać", "siać");
            this.ComplementWordsRepository.Add("obsiej", "siać");
           //initialize attribute words repo
            this.AttributeWordsRepository.Add("wszystkie", "wszystkie");
            this.AttributeWordsRepository.Add("każde", "każde");
            this.AttributeWordsRepository.Add("zaorane", "zaorane");
            this.AttributeWordsRepository.Add("spulchnione", "zaorane");
            this.AttributeWordsRepository.Add("przeorane", "zaorane");
            this.AttributeWordsRepository.Add("niespulchnione", "zaorane");
            this.AttributeWordsRepository.Add("nieprzeorane", "zaorane");
            this.AttributeWordsRepository.Add("niezaorane", "niezaorane");
            this.AttributeWordsRepository.Add("niezaoranego", "niezaorane");
            this.AttributeWordsRepository.Add("zasiane", "zasiane");
            this.AttributeWordsRepository.Add("obsiane", "zasiane");
            this.AttributeWordsRepository.Add("posiane", "zasiane");
            this.AttributeWordsRepository.Add("zgniłe", "zgniłe");
            this.AttributeWordsRepository.Add("martwe", "zgniłe");
            this.AttributeWordsRepository.Add("uschnięte", "zgniłe");
            this.AttributeWordsRepository.Add("w pobliżu", "niedaleko");
            this.AttributeWordsRepository.Add("obok", "niedaleko");
            this.AttributeWordsRepository.Add("koło", "niedaleko");
            this.AttributeWordsRepository.Add("przy", "niedaleko");
            this.AttributeWordsRepository.Add("nieopodal", "niedaleko");
            this.AttributeWordsRepository.Add("tuż", "niedaleko");
            this.AttributeWordsRepository.Add("po sąsiedzku", "niedaleko");
            this.AttributeWordsRepository.Add("w sąsiedztwie", "niedaleko");
            this.AttributeWordsRepository.Add("w zasięgu", "niedaleko");
            this.AttributeWordsRepository.Add("pobliskie", "niedaleko");
            this.AttributeWordsRepository.Add("niedaleko", "niedaleko");
            this.AttributeWordsRepository.Add("kukurydzy", "kukurydzy");
            this.AttributeWordsRepository.Add("kukurydziane", "kukurydzy");
            this.AttributeWordsRepository.Add("pszenicy", "pszenicy");
            this.AttributeWordsRepository.Add("pszeniczne", "pszenicy");
            this.AttributeWordsRepository.Add("pszenicę", "pszenicy");
            this.AttributeWordsRepository.Add("żytnie", "żyta");
            this.AttributeWordsRepository.Add("0", "0");
            this.AttributeWordsRepository.Add("zero", "0");
            this.AttributeWordsRepository.Add("1", "1");
            this.AttributeWordsRepository.Add("jeden", "1");
            this.AttributeWordsRepository.Add("pierwsze", "1");
            this.AttributeWordsRepository.Add("2", "2");
            this.AttributeWordsRepository.Add("drugie", "2");
            this.AttributeWordsRepository.Add("dwa", "2");
            this.AttributeWordsRepository.Add("3", "3");
            this.AttributeWordsRepository.Add("trzy", "3");
            this.AttributeWordsRepository.Add("trzecie", "3");
            this.AttributeWordsRepository.Add("4", "4");
           this.AttributeWordsRepository.Add("cztery", "4");
            this.AttributeWordsRepository.Add("czwarte", "4");




            //initialize adverbial words repository
            string[] then = { "a potem", "potem", "a następnie", "a nastepnie", "następnie", "nastepnie", "a później", "później", "a pozniej", "a poxniej", "pozniej", "poxniej", "po czym", "kolejno",
                                "z kolei", "w dalszym ciągu"};
            string[] now = { "natychmiast", "teraz", "niezwłocznie", "bezwłocznie", "momentalnie", "błyskawicznie", "migiem", "w mig", "zaraz", "rach-ciach", "szast-prast", "trzask-prask", "w                             okamgnieniu",
                           "od razu", "z miejsca", "z marszu", "z mety", "w trybie natychmiastowym"};
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
                "proszę", "czy", "mógłbyś", "debilu", "idioto", "aczkolwiek"
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
                        new TwoWordExpression("się", "udać", Order.DoesntMatter),
                        new TwoWordExpression("się", "ruszyć", Order.DoesntMatter)
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
                       new TwoWordExpression("na", "polu", Order.CommonWordFirst),
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
*/


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

        public HardCodedDictionary()
        {
            this.TaskWordsRepository = new Dictionary<string, string>();
            this.ComplementWordsRepository = new Dictionary<string, string>();
            this.AttributeWordsRepository = new Dictionary<string, string>();
            this.AdverbialWordsRepository = new Dictionary<string, string>();

            Delimiters = new char[] { ' ', '.', ':', ';', '\t', '\n' };
            DefaultComplementWord = "domyślne";

            //initialize task words repository
            string[] move = { 
                            "się udać", "udać się", "pojechać", "pojechac", "pójść", "przesunąć się", "się przesunąć", "przesuń się", "idź", "idz", "rusz", "ruszaj", "jedź", 
                            "pojedź", "pojedz", "zawieź", "zawiex", "podążaj", "udaj się", "pofatyguj się", "kopsnij się", "wyrusz", "wróć", "udax sie", "udav sie", "sie udax", "sie udav", "udaj sie",                                 "pojdz", "przesun sie", "pżesuń się", "idx", "idc", "rusz sie", "sie rusz", "rósz sie", "rosz sie", "rusx", "ruaz", "rudz", "rusc",
                            "zawiec", "zawies", "zawied", "podazaj", "podązaj", "podażaj", "podąrzaj", "podarzaj", "pofatygój się",
                            "pofatyguj sie", "pofatygój sie", "wroc", "wruć", "wruc", "wróv", "wróx", "wrov", "wrox", "zapieprzaj", "zapieprzak", "zapieprzah", "zapoeiprzaj", "zpaieprzaj", "zapierdalaj", 
                             "mknij", "mknać", "pędź", "pedź", "pedz", "popędzaj", "popedzaj", "gazuj", "szoruj", "zapierdalak", "zapierdalah", "zpaierdalaj", "zapierdzielaj", "podrózuj", "podróżuj",                                   "podrużuj", "podrórzuj", "podróżui", "pojsc", "pójśc", "pójsć", "pujść", "pujsc", "pójśv", "pójśx", "pójdć", "pójać", "pójdź", "pójdx", "pojdx",
                             "pojechav", "pojechax", "pojehac", "pojehać", "pojeć"
                            };

            string[] stop = { "zatrzymaj", "zatrzymaj się", "zatrzymać się", "się zatrzymać", "zatrzymak", "ztatzymaj", "zatrzymaj sei", "halt", 
                               "stop", "sotp", "spot", "sopt", "stój", "stoj", "stać", "stac", "stuj",
                               "przestań", "przestan", "przerwij", "abort",
                               "zastopuj", "zahamuj", "zachamuj", "zahamój", "zachamój", "zachamoj", "trwaj w bezruchu"};

            string[] plow = { "zaorać", "zaorac", "zaoraj", "zaraj", "spulchnij", "przeorać", "przeorac", "przeoraj",
                                "oraj", "orac", "orav", "orax", "oarc", "orca" };

            string[] sow = { "zasiać", "zasiac", "zasaic", "zsaiac", "siej", "seij", "siei", "posiej", "psoiej", "zasiej",
                               "zsaiej", "obsiej", "osbiej", "zaisiej", "zaisiać" };

            string[] harvest = { "zbierz", "zbiezr", "zbież", "zbier", "zebrać", "zbieraj", "skoś", "skos", "tnij", "skroc", "skruc", "skruv", "skruz", "obstrzyż",
                                   "obstrzyrz", "obetnij" };

            string[] irrigate = { "podlej", "podlje", "pdolej", "nawodnij", "nwaodnij", "nawodnj", "nawilż", "nawilz", "lej", "natryskaj", "ntaryskaj",
                                    "natryskah", "natryskal", "napój", "napoj", "napoi", "napuj", "napui", "npaój", "skrop", "skrapiać", "iryguj", "irygój",
                                    "iyrguj", "iyrgój", "irygui", "ztryskaj", "stryskaj" };

            string[] start = { "zacznij", "zacząć", "zaczac", "zacząc", "zaczać", "zaskocz", "zaskozc", "zsakocz", "rozpocznij", "start", "begin", "poczatek" };

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
            this.ComplementWordsRepository.Add("ziemia", "pola");
            this.ComplementWordsRepository.Add("ziemie", "pola");
            this.ComplementWordsRepository.Add("grunt", "pola");
            this.ComplementWordsRepository.Add("poletko", "pola");
            this.ComplementWordsRepository.Add("łan", "pola");
            this.ComplementWordsRepository.Add("użytek", "pola");
            this.ComplementWordsRepository.Add("pólko", "pola");
            this.ComplementWordsRepository.Add("zagon", "pola");
            this.ComplementWordsRepository.Add("rola", "pola");
            this.ComplementWordsRepository.Add("niwa", "pola");
            this.ComplementWordsRepository.Add("rozłóg", "pola");
            this.ComplementWordsRepository.Add("obszar", "pola");
            this.ComplementWordsRepository.Add("pole", "pole");
            this.ComplementWordsRepository.Add("pól", "pola");
            this.ComplementWordsRepository.Add("pola", "pola");
            this.ComplementWordsRepository.Add("magazyn", "magazyn");
            this.ComplementWordsRepository.Add("magazynu", "magazyn");
            this.ComplementWordsRepository.Add("skład", "magazyn");
            this.ComplementWordsRepository.Add("składnica", "magazyn");
            this.ComplementWordsRepository.Add("składzik", "magazyn");
            this.ComplementWordsRepository.Add("magazynek", "magazyn");
            this.ComplementWordsRepository.Add("przechowywalnia", "magazyn");
            this.ComplementWordsRepository.Add("schowek", "magazyn");
            this.ComplementWordsRepository.Add("traktor", "traktor");
            this.ComplementWordsRepository.Add("ciągnik", "traktor");
            this.ComplementWordsRepository.Add("ciagnik", "traktor");
            this.ComplementWordsRepository.Add("je", "domyślne");
            this.ComplementWordsRepository.Add("to", "domyślne");
            this.ComplementWordsRepository.Add("aktualne", "domyślne");

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
            this.AttributeWordsRepository.Add("przeorane", "zaorane");
            this.AttributeWordsRepository.Add("niespulchnione", "zaorane");
            this.AttributeWordsRepository.Add("nieprzeorane", "zaorane");
            this.AttributeWordsRepository.Add("obsiane", "zasiane");
            this.AttributeWordsRepository.Add("posiane", "zasiane");
            this.AttributeWordsRepository.Add("martwe", "zgniłe");
            this.AttributeWordsRepository.Add("koło", "niedaleko");
            this.AttributeWordsRepository.Add("przy", "niedaleko");
            this.AttributeWordsRepository.Add("nieopodal", "niedaleko");
            this.AttributeWordsRepository.Add("tuż", "niedaleko");
            this.AttributeWordsRepository.Add("po sąsiedzku", "niedaleko");
            this.AttributeWordsRepository.Add("w sąsiedztwie", "niedaleko");
            this.AttributeWordsRepository.Add("w zasięgu", "niedaleko");
            this.AttributeWordsRepository.Add("żytnie", "żyta");
            this.AttributeWordsRepository.Add("1", "1");
            this.AttributeWordsRepository.Add("jeden", "1");
            this.AttributeWordsRepository.Add("pierwsze", "1");
            this.AttributeWordsRepository.Add("2", "2");
            this.AttributeWordsRepository.Add("drugie", "2");
            this.AttributeWordsRepository.Add("dwa", "2");
            this.AttributeWordsRepository.Add("3", "3");
            this.AttributeWordsRepository.Add("trzy", "3");
            this.AttributeWordsRepository.Add("trzecie", "3");
            this.AttributeWordsRepository.Add("4", "4");
            this.AttributeWordsRepository.Add("cztery", "4");
            this.AttributeWordsRepository.Add("czwarte", "4");

            this.AttributeWordsRepository.Add("5", "5");
            this.AttributeWordsRepository.Add("6", "6");
            this.AttributeWordsRepository.Add("7", "7");
            this.AttributeWordsRepository.Add("8", "8");

            this.AttributeWordsRepository.Add("9", "9");
            this.AttributeWordsRepository.Add("10", "10");
            this.AttributeWordsRepository.Add("11", "11");
            this.AttributeWordsRepository.Add("12", "12");

            this.AttributeWordsRepository.Add("13", "13");
            this.AttributeWordsRepository.Add("14", "14");
            this.AttributeWordsRepository.Add("15", "15");
            this.AttributeWordsRepository.Add("16", "16");

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

