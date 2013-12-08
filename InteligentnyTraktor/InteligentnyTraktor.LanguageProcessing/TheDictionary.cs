using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    public interface ITheDictionary
    {
        Char[] Delimiters { get; }
        //public Dictionary<string, ICollection<string>> TaskWordsRepository { get; private set; }
        Dictionary<string, string> TaskWordsRepository { get; }//, key to słowo, a value archetypowy synonim?(to brzmi sensownie)
        Dictionary<string, string> ComplementWordsRepository { get;}
        Dictionary<string, string> AttributeWordsRepository { get; }
        Dictionary<string, string> AdverbialWordsRepository { get; }
        ICollection<string> IgnoredWords { get; }
        ICollection<string> ConjuctionWords { get; }
        Dictionary<string, List<TwoWordExpression>> CommonWordIndexedTwoWordExpressions { get; }
        ICollection<string> CommonWordsForTwoWordExpressions { get; }
    }
}
