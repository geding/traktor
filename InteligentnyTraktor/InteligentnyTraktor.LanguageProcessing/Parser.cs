using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor.LanguageProcessing
{
    internal class Parser
    {
        readonly ITheDictionary dictionary;

        Dictionary<int, string> indexedTaskWords = new Dictionary<int, string>();
        Dictionary<int, string> indexedComplementWords = new Dictionary<int, string>();
        Dictionary<int, string> indexedAttributeWords = new Dictionary<int, string>();
        Dictionary<int, string> indexedAdverbialWords = new Dictionary<int, string>();       

        public Parser(ITheDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public List<string> Scan(string input)
        {
            input = input.ToLower();
            input = input.Replace(",", " ,"); //odłącza przecinek od wyrazów przy splicie

            List<string> words = input.Split(dictionary.Delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < words.Count; i++)
            {
                //join two worded expressions
                if (i < words.Count - 1)
                {
                    if (IsTwoWordsExpression(words[i], words[i + 1]))
                    {
                        var expression = words[i] + " " + words[i + 1];
                        words.RemoveAt(i + 1);
                        words[i] = expression;
                    }
                }               

                words[i] = this.ReplaceWithSynonymes(words[i]);

                //get words according to type, with indexes in phrase
                this.TryAddIndexedWord(i, words[i], indexedTaskWords, dictionary.TaskWordsRepository.Keys);
                this.TryAddIndexedWord(i, words[i], indexedComplementWords, dictionary.ComplementWordsRepository.Keys);
                this.TryAddIndexedWord(i, words[i], indexedAdverbialWords, dictionary.AdverbialWordsRepository.Keys);
                this.TryAddIndexedWord(i, words[i], indexedAttributeWords, dictionary.AttributeWordsRepository.Keys);
            }

            //foreach (var item in words)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine();
            //foreach (var item in indexedTaskWords)
            //{
            //    Console.WriteLine(item.Key + " " + item.Value);
            //}
            //Console.WriteLine();
            //foreach (var item in indexedComplementWords)
            //{
            //    Console.WriteLine(item.Key + " " + item.Value);
            //}
            //Console.WriteLine();
            //foreach (var item in indexedAttributeWords)
            //{
            //    Console.WriteLine(item.Key + " " + item.Value);
            //}
            //Console.WriteLine();
            //foreach (var item in indexedAdverbialWords)
            //{
            //    Console.WriteLine(item.Key + " " + item.Value);
            //}

            return words;

            //if (indexedTaskWords.Count == 0)
            //{
            //    return;
            //    //"Nie jestem tutaj żeby ucinać pogaduszki, tylko żeby pracować."
            //}

            //if (this.IsPhraseCompound(words))
            //{
            //    List<List<string>> phrases = this.SplitToSimplePhrases(words, indexedTaskWords);
            //}
        }

        public Phrase Parse(List<string> phrase)
        {            
            Dictionary<int, List<KeyValuePair<int, string>>> groupedTasks = this.GetGroupedTasksIn(phrase);

            foreach (var item in groupedTasks)
            {
                Console.WriteLine(item.Key + ":");
                foreach (var x in item.Value)
                {
                    Console.WriteLine(x.Key + " " + x.Value);
                }
            }
            //FindSharedComplement

            return null;
        }

        private Dictionary<int, List<KeyValuePair<int, string>>> GetGroupedTasksIn(List<string> phrase)
        {
            var groupedTasks = new Dictionary<int, List<KeyValuePair<int, string>>>();
            int group = 0;

            for (int i = 0; i < indexedTaskWords.Count - 1; i++)
            {
                var current = indexedTaskWords.ElementAt(i);
                var next = indexedTaskWords.ElementAt(i + 1);
                if (AreConjuctedOrNextToEachOther(current.Key, next.Key, phrase))
                {
                    if (!groupedTasks.ContainsKey(group))
                    {
                        groupedTasks.Add(group, new List<KeyValuePair<int, string>>());
                    }
                    if (!groupedTasks[group].Contains(current))
                    {
                        groupedTasks[group].Add(current);
                    }
                    if (!groupedTasks[group].Contains(next))
                    {
                        groupedTasks[group].Add(next);
                    }
                }
                else group++;
            }

            //foreach (var task in indexedTaskWords)
            //{
            //    var x = indexedTaskWords.SkipWhile(other => other.Equals(task));
            //    //dla każdego taska, który jest dalej niż aktualnie sprawdzany
            //    foreach (var otherTask in x)
            //    {
            //        if (AreConjuctedOrNextToEachOther(task.Key, otherTask.Key, phrase))
            //        {
            //            if (!groupedTasks.ContainsKey(group))
            //            {
            //                groupedTasks.Add(group, new List<KeyValuePair<int, string>>());
            //            }
            //            if (!groupedTasks[group].Contains(task))
            //            {
            //                groupedTasks[group].Add(task);
            //            }
            //            if (!groupedTasks[group].Contains(otherTask))
            //            {
            //                groupedTasks[group].Add(otherTask);
            //            }
            //        }
            //        else group++;
            //    }
            //}

            return groupedTasks;
        }

        private void TryAddIndexedWord(int index, string word, Dictionary<int, string> to, ICollection<string> from)
        {
            foreach (var w in from)
            {
                if (word == w)
                {
                    to.Add(index, word);
                    break;
                }
            }
        }
      
        private string ReplaceWithSynonymes(string word)
        {
            string result = null;
            if ((result = this.TryFindingSynonime(word, dictionary.TaskWordsRepository)) != null)
            {
                return result;
            }

            if ((result = this.TryFindingSynonime(word, dictionary.ComplementWordsRepository)) != null)
            {
                return result;
            }

            if ((result = this.TryFindingSynonime(word, dictionary.AttributeWordsRepository)) != null)
            {
                return result;
            }

            if ((result = this.TryFindingSynonime(word, dictionary.AdverbialWordsRepository)) != null)
            {
                return result;
            }

            return word;
        }

        private string TryFindingSynonime(string word, Dictionary<string, string> wordRepo)
        {
            foreach (var w in wordRepo)
            {
                if (w.Key == word)
                {
                    return w.Value;
                }
            }
            //not found anything
            return null;
        }

        private bool IsPhraseCompound(List<string> phrase)
        {
            //jest zlozone, jesli po spojniku ( wtym przecinek), pojawia sie nowe orzeczenie (task)
            //a wczesniej znalazło się co najmniej jedno orzeczenie i co najmniej jedno dopełnienie
            for (int i = 0; i < phrase.Count; i++)
            {
                //todo
            }
            return true;//temp
        }

        private Dictionary<int, string> GetIndexedTaskWordsFrom(List<string> words)
        {
            var indexedTaskWords = new Dictionary<int, string>();

            for (int i = 0; i < words.Count; i++)
            {
                foreach (var task in dictionary.TaskWordsRepository)
                {
                    if (task.Value.Contains(words[i]))
                    {
                        indexedTaskWords.Add(i, task.Key);
                        break;
                    }
                }
            }

            return indexedTaskWords;
        }


        //jeżeli w drugim zdaniu prostym nie ma dopełnienia, to trzeba je skopiować z pierwszego zdania prostego w zdaniu złożonym
        private List<List<string>> SplitToSimplePhrases(List<string> multitaskPhrase, Dictionary<int, string> indexedTaskWords)
        {
            var neighbourTaskWords = new List<KeyValuePair<int, string>>();

            foreach (var itw in indexedTaskWords)
            {
                foreach (var otheritw in indexedTaskWords.Except(new KeyValuePair<int, string>[] { itw }))
                {
                    if (DoesWordsStandNextToEachOther(itw.Key, otheritw.Key, multitaskPhrase))
                    {
                        //zdublowane słowa
                        if (itw.Value == otheritw.Value)
                        {
                            continue;
                        }

                        if (!neighbourTaskWords.Contains(itw))
                        {
                            neighbourTaskWords.Add(itw);
                        }
                        if (!neighbourTaskWords.Contains(otheritw))
                        {
                            neighbourTaskWords.Add(otheritw);
                        }
                    }
                }
            }

            foreach (var item in neighbourTaskWords)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }

            return null;
        }

        private bool IsTwoWordsExpression(string first, string second)
        {
            foreach (var commonWord in dictionary.CommonWordsForTwoWordExpressions)
            {
                if (commonWord == first)
                {
                    foreach (var expr in dictionary.CommonWordIndexedTwoWordExpressions[commonWord])
                    {
                        if (second == expr.OtherWord)
                        {
                            switch (expr.Order)
                            {
                                case Order.DoesntMatter: return true;
                                case Order.CommonWordFirst: return true;
                                case Order.OtherWordFirst: return false;
                                default: throw new Exception();
                            }
                        }
                    }
                }

                if (commonWord == second)
                {
                    foreach (var expr in dictionary.CommonWordIndexedTwoWordExpressions[commonWord])
                    {
                        if (first == expr.OtherWord)
                        {
                            switch (expr.Order)
                            {
                                case Order.DoesntMatter: return true;
                                case Order.CommonWordFirst: return false;
                                case Order.OtherWordFirst: return true;
                                default: throw new Exception();
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool DoesWordsStandNextToEachOther(int firstIndex, int secondIndex, List<string> phrase)
        {
            int indexDifference = Math.Abs(firstIndex - secondIndex);

            //bezpośredni sąsiedzi
            if (indexDifference == 1)
            {
                return true;
            }

            //todo - przecinek
            var omit = new string[] { "i", "się", "sie" };
            //czy są pomiędzy nimi nieznaczące wyrazy
            if (indexDifference == 2)
            {
                if (omit.Contains(phrase[(firstIndex + secondIndex) / 2]))
                    return true;
            }
            if (indexDifference == 3)
            {
                if (omit.Contains(phrase[(firstIndex + secondIndex) / 2])
                    && omit.Contains(phrase[(firstIndex + secondIndex) / 2 + 1]))
                    return true;
            }

            return false;
        }

        private bool AreConjuctedOrNextToEachOther(int firstIndex, int secondIndex, List<string> phrase)
        {
            int indexDifference = Math.Abs(firstIndex - secondIndex);

            //bezpośredni sąsiedzi
            if (indexDifference == 1)
            {
                return true;
            }

            if (indexDifference == 2)
            {
                if (dictionary.ConjuctionWords.Contains(phrase[(firstIndex + secondIndex) / 2]))
                    return true;
            }

            //więcej niż jeden wyraz pomiędzy nimi lub nie spójnik
            return false;
        }
    }
}
