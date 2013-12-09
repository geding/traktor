using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    class Test
    {
        static void Main(string[] args)
        {
            Console.WriteLine("q + enter - wyjscie");
            var input = "";
            while (input != "q")
            {
                input = Console.ReadLine();
                var parser = new Parser(new HardCodedDictionary());

                Console.WriteLine();
                var phrase = parser.TryParse(parser.Scan(input)).ToString();
                Console.WriteLine(phrase);
                Console.WriteLine();
            }
            
        }
    }
}
