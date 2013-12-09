﻿using InteligentnyTraktor.Model;
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
        Dictionary<int, string> indexedConjuctionWords = new Dictionary<int, string>();

        //int - group number, dict of indexed task words
        Dictionary<int, Dictionary<int, string>> groupedTasks;

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
                this.TryAddIndexedWord(i, words[i], indexedConjuctionWords, dictionary.ConjuctionWords);
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

        public Phrase TryParse(List<string> phrase)
        {
            

            this.groupedTasks = this.GetGroupedTasksIn(phrase);

            //foreach (var item in groupedTasks)
            //{
            //    Console.WriteLine(item.Key + ":");
            //    foreach (var x in item.Value)
            //    {
            //        Console.WriteLine(x.Key + " " + x.Value);
            //    }
            //}

            List<List<string>> simplePhrases = this.SplitToSimplePhrases(phrase);

            //0. get tasks (from indexed list)
            //1. find complements (get from indexed list)
            //2. split attributes to complements:
            //  a) find complement (get from indexed list)
            //  b) find conjuctions beetween them
            //  c) last conjuction separates attribute groups
            //  d) bind each attribute in attribute group with complement
            //3. append complements to tasks
            //4. find adverbials (get form indexed list)
            //5. append adveribals to tasks
            //6. append info to phrase
            //7. repeat for all subphrases

            int startingIndex = 0;
            var result = new Phrase();
            
            foreach (var phr in simplePhrases)
            {
                SortedDictionary<int, string> tasks = new SortedDictionary<int, string>();
                SortedDictionary<int, string> adverbials = new SortedDictionary<int, string>();
                SortedDictionary<int, string> complements = new SortedDictionary<int, string>();
                SortedDictionary<int, string> conjuctions = new SortedDictionary<int, string>();
                SortedDictionary<int, string> attributes = new SortedDictionary<int, string>();

                //var tasks = this.GetTaskWordsBeetween(startingIndex, startingIndex + phr.Count, phr);


                //+ 1 bo spójnik pomiędzy złożonymi zdaniami
                for (int i = startingIndex; i < startingIndex + phr.Count + 1; i++)
                {
                    if (indexedTaskWords.ContainsKey(i))
                    {
                        tasks.Add(i, indexedTaskWords[i]);
                        continue;
                    }

                    if (indexedComplementWords.ContainsKey(i))
                    {
                        complements.Add(i, indexedComplementWords[i]);
                        continue;
                    }

                    if (indexedAdverbialWords.ContainsKey(i))
                    {
                        adverbials.Add(i, indexedAdverbialWords[i]);
                        continue;
                    }

                    if (indexedConjuctionWords.ContainsKey(i))
                    {
                        conjuctions.Add(i, indexedConjuctionWords[i]);
                        continue;
                    }

                    if (indexedAttributeWords.ContainsKey(i))
                    {
                        attributes.Add(i, indexedAttributeWords[i]);
                        continue;
                    }
                }

                var taskAdverbialConnections = new Dictionary<string, int>();

                foreach (var task in tasks)
                {
                    for (int i = 0; i < adverbials.Count; i++)
                    {
                        //tutaj powinno pobierać ze słownika zbiór okoliczników, które występują bezpośrednio przed czasownikiem
                        //i odnoszą się jedynie do niego
                        if (task.Key == adverbials.ElementAt(i).Key + 1 && adverbials.ElementAt(i).Value == "następnie")
                        {
                            taskAdverbialConnections.Add(adverbials.ElementAt(i).Value, task.Key);
                            adverbials.Remove(adverbials.ElementAt(i).Key);
                        }
                    }
                }

                foreach (var task in tasks)
                {
                    var builder = new TaskCommandBuilder();

                    //for (int i = 0; i < adverbials.Count; i++)
                    //{
                    //    if (task.Key == adverbials.ElementAt(i).Key + 1 && adverbials.ElementAt(i).Value == "następnie")
                    //    {
                    //        builder.AppendAdverbial(adverbials.ElementAt(i).Value);
                    //        adverbials.Remove(adverbials.ElementAt(i).Key);
                    //    }
                    //}

                    //for (int i = 0; i < adverbials.Count; i++)
                    //{
                    //    if (tasks.ContainsKey(adverbials.ElementAt(i).Key + 1) && adverbials.ElementAt(i).Value == "następnie")
                    //    {
                    //        builder.AppendAdverbial(adverbials.ElementAt(i).Value);
                    //        adverbials.Remove(adverbials.ElementAt(i).Key);
                    //    }
                    //}

                    //foreach (var adverbial in adverbials)
                    //{
                    //    //tutaj powinno pobierać ze słownika zbiór okoliczników, które występują bezpośrednio przed czasownikiem
                    //    //i odnoszą się jedynie do niego
                    //    if (tasks.ContainsKey(adverbial.Key + 1) && adverbial.Value == "następnie")
                    //    {
                    //        builder.AppendAdverbial(adverbial.Value);
                    //        adverbialsApplyingToOnlyOneVerbIndexes.Add(adverbial.Key, adverbial.Value);
                    //        //adverbials.Remove(adverbial.Key);
                    //    }
                    //}
                    foreach (var adverbial in adverbials)
                    {
                        builder.AppendAdverbial(adverbial.Value);
                    }
                    foreach (var x in taskAdverbialConnections)
                    {
                        if (x.Value == task.Key)
                        {
                            builder.AppendAdverbial(x.Key);
                        }
                    }


                    if (complements.Count == 0)
                    {
                        builder.AppendComplement(dictionary.DefaultComplementWord, attributes.Values);
                    }
                    if (complements.Count == 1)
                    {
                        builder.AppendComplement(complements.ElementAt(0).Value, attributes.Values);
                    }
                    else if (complements.Count == 2)
                    {
                        int lastConjuctionIndex = (int)indexedConjuctionWords
                                                    .Where(x => x.Key > complements.ElementAt(0).Key && x.Key < complements.ElementAt(1).Key)
                                                    .Max(y => y.Key);

                        List<string> temp = new List<string>();
                        foreach (var attribute in attributes.TakeWhile(x => x.Key < lastConjuctionIndex))
                        {
                            temp.Add(attribute.Value);
                        }
                        builder.AppendComplement(complements.ElementAt(0).Value, temp);

                        temp = new List<string>();
                        foreach (var attribute in attributes.SkipWhile(x => x.Key <= lastConjuctionIndex))
                        {
                            temp.Add(attribute.Value);
                        }
                        builder.AppendComplement(complements.ElementAt(1).Value, temp);
                    }
                    else if (complements.Count > 2)
                    {
                        //nie potrafi takich skomplikowanych
                        return null;
                    }

                    builder.SetTaskWord(task.Value);
                    result.Tasks.Add(builder.Build());
                }

                //przesuwamy indeks startowy o długość zdania + spójnik na koncu
                startingIndex += phr.Count + 1;
            }


            //Console.WriteLine();
            //Console.WriteLine("zdania proste:");

            //foreach (var phr in simplePhrases)
            //{
            //    string res = "";
            //    foreach (var word in phr)
            //    {
            //        res += " " + word;
            //    }
            //    Console.WriteLine(res);
            //    Console.WriteLine();
            //}


            return result;
        }

        private object GetTaskWordsBeetween(int firstIndex, int lastIndex, List<string> phrase)
        {
            throw new NotImplementedException();
        }

        private Dictionary<int, Dictionary<int, string>> GetGroupedTasksIn(List<string> phrase)
        {
            var groupedTasks = new Dictionary<int, Dictionary<int, string>>();
            int group = 0;

            for (int i = 0; i < indexedTaskWords.Count - 1; i++)
            {
                var current = indexedTaskWords.ElementAt(i);
                var next = indexedTaskWords.ElementAt(i + 1);
                if (AreConjuctedOrNextToEachOther(current.Key, next.Key, phrase))
                {
                    if (!groupedTasks.ContainsKey(group))
                    {
                        groupedTasks.Add(group, new Dictionary<int, string>());
                    }
                    if (!groupedTasks[group].Contains(current))
                    {
                        groupedTasks[group].Add(current.Key, current.Value);
                    }
                    if (!groupedTasks[group].Contains(next))
                    {
                        groupedTasks[group].Add(next.Key, next.Value);
                    }
                }
                else group++;
            }

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
            foreach (var repository in new List<Dictionary<string, string>>()
                {
                    dictionary.TaskWordsRepository,
                    dictionary.ComplementWordsRepository,
                    dictionary.AttributeWordsRepository,
                    dictionary.AdverbialWordsRepository,
                })
            {
                foreach (var w in repository)
                {
                    if (w.Key == word)
                    {
                        return w.Value;
                    }
                }
            }

            return word;
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


        //jeżeli w drugim zdaniu prostym nie ma dopełnienia, to trzeba je skopiować z pierwszego zdania prostego w zdaniu złożonym (to chyba zrobi kontekst solwer)
        private List<List<string>> SplitToSimplePhrases(List<string> phrase)
        {
            var result = new List<List<string>>();
            List<string> firstPartialPhrase = null;
            int oldConjuctionIndex = -1;
            int newConjuctionIndex = phrase.Count;
            int taskIndex = -1;
            int currentGroup = -1;

            //zakładamy, że zdanie nie rozpoczyna ani nie kończy się spójnikiem
            for (int i = 1; i < phrase.Count; i++)
            {
                //jeżeli na i-tym miejscu jest spójnik
                if (indexedConjuctionWords.ContainsKey(i))
                {
                    //jeżeli spójnik łączy zgrupowane zadania to nie rozdziela zdań złożonych
                    bool isInGroup = false;
                    if (i + 1 <= phrase.Count)
                    {
                        if (
                            indexedTaskWords.ContainsKey(i - 1)
                            && indexedTaskWords.ContainsKey(i + 1)
                            && AreConjuctedOrNextToEachOther(i - 1, i + 1, phrase)
                           )
                        {
                            isInGroup = true;
                        }
                    }
                    if (i + 2 <= phrase.Count)
                    {
                        if (
                            indexedTaskWords.ContainsKey(i - 1)
                            && indexedTaskWords.ContainsKey(i + 2)
                            && AreConjuctedOrNextToEachOther(i - 1, i + 2, phrase)
                           )
                        {
                            isInGroup = true;
                        }
                    }

                    if (isInGroup)
                    {
                        continue;
                    }


                    //if (groupedTasks.Any(group => 
                    //    {
                    //        bool isInGroup = false;
                    //        if (i + 1 <= phrase.Count)
                    //        {
                    //            if (
                    //                indexedTaskWords.ContainsKey(i - 1) 
                    //                && indexedTaskWords.ContainsKey(i + 1) 
                    //                && AreConjuctedOrNextToEachOther(i - 1, i + 1, phrase)
                    //               )
                    //            {
                    //                isInGroup = true;
                    //            }
                    //        }
                    //        if (i + 2 <= phrase.Count)
                    //        {
                    //            if (
                    //                indexedTaskWords.ContainsKey(i - 1)
                    //                && indexedTaskWords.ContainsKey(i + 2)
                    //                && AreConjuctedOrNextToEachOther(i - 1, i + 2, phrase)
                    //               )
                    //            {
                    //                isInGroup = true;
                    //            }
                    //        }
                    //        return isInGroup;
                    //        //return (group.Value.ContainsKey(i - 1) && group.Value.ContainsKey(i + 1))
                    //        //|| (group.Value.ContainsKey(i - 1) && group.Value.ContainsKey(i + 2));
                    //    }))
                    //    continue;

                    oldConjuctionIndex = newConjuctionIndex;
                    newConjuctionIndex = i;

                    continue;
                }

                if (indexedTaskWords.ContainsKey(i))
                {                    
                    int newGroup = -1;
                    foreach (var group in groupedTasks)
                    {
                        if (group.Value.ContainsKey(i))
                        {
                            newGroup = group.Key;
                        }
                    }
                    if (newGroup != currentGroup || currentGroup == -1)
                    {
                        //zdanie wielokrotnie złożone
                        if (taskIndex > oldConjuctionIndex && taskIndex < newConjuctionIndex)
                        {
                            if (firstPartialPhrase == null)
                            {
                                firstPartialPhrase = new List<string>();
                                for (int j = 0; j < oldConjuctionIndex; j++)
                                {
                                    firstPartialPhrase.Add(phrase[j]);
                                }
                                result.Add(firstPartialPhrase);
                            }

                            var partialPhrase = new List<string>();
                            for (int j = oldConjuctionIndex + 1; j < newConjuctionIndex; j++)
                            {
                                partialPhrase.Add(phrase[j]);
                            }
                            result.Add(partialPhrase);
                        }
                    }
                    currentGroup = newGroup;

                    taskIndex = i;
                }
            }

            //ostatnie podzdanie
            if (taskIndex > newConjuctionIndex)
            {
                if (firstPartialPhrase == null)
                {
                    firstPartialPhrase = new List<string>();
                    for (int j = 0; j < newConjuctionIndex; j++)
                    {
                        firstPartialPhrase.Add(phrase[j]);
                    }
                    result.Add(firstPartialPhrase);
                }

                var partialPhrase = new List<string>();
                for (int j = newConjuctionIndex + 1; j < phrase.Count; j++)
                {
                    partialPhrase.Add(phrase[j]);
                }
                result.Add(partialPhrase);
            }

            return result.Count == 0 ? new List<List<string>>{ phrase } : result;
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

        private bool AreConjuctedOrNextToEachOther(int firstIndex, int secondIndex, List<string> phrase)
        {
            int first, second;

            if (firstIndex < secondIndex)
            {
                first = firstIndex;
                second = secondIndex;
            }
            else if (firstIndex > secondIndex)
            {
                first = secondIndex;
                second = firstIndex;
            }
            else throw new ArgumentException("indexes have to be different");

            int indexDifference = second - first;

            //bezpośredni sąsiedzi
            if (indexDifference == 1)
            {
                return true;
            }

            if (indexDifference == 2)
            {
                //polecenie zatrzymania nie współdzieli dopełnienia
                if (phrase[first] == "stop" || phrase[second] == "stop") return false; //todo: dictionary grupy współdzielące dopełnienie, coś takiego
                if (dictionary.ConjuctionWords.Contains(phrase[(first + second) / 2]))
                    return true;
            }

            if (indexDifference == 3)
            {
                if (phrase[first] == "stop" || phrase[second] == "stop") return false;
                if (dictionary.ConjuctionWords.Contains(phrase[(first + second) / 2])
                    && dictionary.AdverbialWordsRepository.ContainsValue(phrase[(first + second) / 2 + 1]))
                    return true;
            }

            //todo:
            //spójnik + okolicznik pomiędzy

            //więcej niż jeden wyraz pomiędzy nimi lub nie spójnik
            return false;
        }
    }
}
