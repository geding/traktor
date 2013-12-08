using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteligentnyTraktor.LanguageProcessing
{
    public class TwoWordExpression
    {
        public string CommonWord { get; private set; }
        public string OtherWord { get; private set; }

        public Order Order { get; private set; }

        public TwoWordExpression(string commonWord, string otherWord, Order order)
        {
            this.CommonWord = commonWord;
            this.OtherWord = otherWord;
            this.Order = order;
        }
    }

    public enum Order
    {
        DoesntMatter,
        CommonWordFirst,
        OtherWordFirst,
    }
}
