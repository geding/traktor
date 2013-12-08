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
            string input = Console.ReadLine();
            var parser = new Parser(new HardCodedDictionary());
            
            parser.Parse(parser.Scan(input));
        }
    }
}
