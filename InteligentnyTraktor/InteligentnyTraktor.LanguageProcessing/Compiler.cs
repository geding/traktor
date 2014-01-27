using InteligentnyTraktor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace InteligentnyTraktor.LanguageProcessing
{
    public class Compiler 
    {
        IStateManager _stateManager;
        int _size;
        Point _nextPosition;
      
        public Compiler(IStateManager stateManager, int size)
        {
            _stateManager = stateManager;
            _size = size;
        }
        public void RunCompiler(string commend)
        {
           Execute(ParseCommend(commend));
        }
        private Phrase ParseCommend(string commend)
        {
            var parser = new Parser(new HardCodedDictionary());
            Phrase phrase = parser.TryParse(parser.Scan(commend));
            return phrase;
        }
        private List<TaskCommand> prepareTaskOrder(List<TaskCommand> tasks)
        {
            foreach (TaskCommand t in tasks)
            {

            }
            // jak beda taski nie tylko natychmiast i nastepnie ale tez "przedtem" to kolejnosc taskow moze ulec zmianie
            return tasks;
        }

        private List<Field> choseFiledsPointedByUser(string fieldType, List<string> filtrs)
        {
            //http://www.codeproject.com/Tips/582450/Build-Where-Clause-Dynamically-in-Linq
            List<Filter> filter = new List<Filter>();

            int n1=-1,n2=-1;
            foreach (string filtr in filtrs)
            {
                int number;
                bool isNumeric = int.TryParse(filtr, out number);
                if (isNumeric)
                {
                    if (n1 == -1)
                        n1 = number;
                    else
                        n2 = number;
                }

                switch (filtr)
                {
                    case "wszystkie":

                        break;
                    case "zaorane":
                        filter.Add(new Filter
                        {
                            PropertyName = "State",
                            Operation = Op.Equals,
                            Value = FieldItemState.Plowed
                        });
                        break;
                    case "niezaorane":
                        filter.Add(new Filter
                        {
                            PropertyName = "State",
                            Operation = Op.NotEquals,
                            Value = FieldItemState.Plowed
                        });
                        break;
                    case "zasiane":
                        filter.Add(new Filter
                        {
                            PropertyName = "State",
                            Operation = Op.Equals,
                            Value = FieldItemState.Harvested
                        });
                     
                        break;
                    case "zgniłe":
                        filter.Add(new Filter
                        {
                            PropertyName = "State",
                            Operation = Op.Equals,
                            Value = FieldItemState.Rotten
                        });
                        break;
                    case "niedaleko":
                       //+1 / -1 w zaleznosci od krawedzi ... trudne ;p
                        Point position = _stateManager.FieldWithTractor;
                            filter.Add(new Filter
                            {
                                PropertyName = "Row",
                                Operation = Op.GreaterThan,
                                Value = (int)position.X -2
                            });
                            filter.Add(new Filter
                            {
                                PropertyName = "Row",
                                Operation = Op.LessThanOrEqual,
                                Value = (int)position.X + 1
                            });

                            filter.Add(new Filter
                            {
                                PropertyName = "Column",
                                Operation = Op.GreaterThan,
                                Value = (int)position.Y -2
                            });
                            filter.Add(new Filter
                            {
                                PropertyName = "Column",
                                Operation = Op.LessThanOrEqual,
                                Value = (int)position.Y  + 1
                            });
                            
                        break;
                    case "kukurydzy":
                        filter.Add(new Filter
                        {
                            PropertyName = "Type",
                            Operation = Op.Equals,
                            Value = FieldItemType.Corn
                        });
                        break;
                     case "pszenicy":
                        filter.Add(new Filter
                        {
                            PropertyName = "Type",
                            Operation = Op.Equals,
                            Value = FieldItemType.Wheat
                        });
                        break;
                }
            }
           
            List<Field> filteredCollection = new List<Field>();
            switch (fieldType)
            {
                case "domyślne":
                case "pole":

                    if (filter.Any())
                    {
                        var deleg = ExpressionBuilder.GetExpression<Field>(filter).Compile();
                        foreach (var t in ((StateManager)_stateManager).fieldItems)
                        {
                            filteredCollection.AddRange(t.Where(deleg).ToList());
                        }
                    }
                    else if (n1 != -1 && n2 == -1) // jest tylko jedna liczba
                    {
                        int wiersz = (int)Math.Ceiling((double)n1 / (double)_size);
                        int kolumna = n1 % _size;
                        wiersz--;  //numerowane od 0
                        if (kolumna == 0) kolumna = _size; //eh indexy od 0
                        kolumna--;
                        if (wiersz <= _size && kolumna <= _size)
                        {
                            Field current = ((StateManager)_stateManager).fieldItems[wiersz][kolumna];
                            filteredCollection.Add(current);
                        }
                    }
                    else if (n1 != -1 && n2 != -1) // sa kordynaty x,y
                    {
                        Field current = ((StateManager)_stateManager).fieldItems[n1][n2];
                        filteredCollection.Add(current);
                    }
                    else
                    {
                        Point position = _stateManager.FieldWithTractor;
                        Field current = ((StateManager)_stateManager).fieldItems[(int)position.Y][(int)position.X];
                        filteredCollection.Add(current);
                    }
                    Field chosen = filteredCollection.First<Field>();
                    filteredCollection.Clear();
                    filteredCollection.Add(chosen);

                    break;
                case "pola":
                        

                        if (filter.Any())
                        {
                            var deleg = ExpressionBuilder.GetExpression<Field>(filter).Compile();
                            foreach (var t in ((StateManager)_stateManager).fieldItems)
                            {
                                filteredCollection.AddRange(t.Where(deleg).ToList());
                            }
                        }
                        else
                        {
                            foreach (var t in ((StateManager)_stateManager).fieldItems)
                            {
                                filteredCollection.AddRange(t.ToList());
                            }
                        }
                      
                    break;
                case "magazyn":

                    break;
                case "traktor":

                    break;
                
                    // foreach(var t in ((StateManager)_stateManager).fieldItems)
                    //    {
                    //        filteredCollection.AddRange(t.ToList());
                    //    }
                    //break;
            }
            return filteredCollection;
        }
        private void runAction(string task, List<Field> fieldsPointed)
        {
          
            switch (task)
            {
                case "jedź" :
                     foreach (Field field in fieldsPointed)
                        _stateManager.MoveTractorTo(field.Row, field.Column);
                    
                    break;
                case "stop":
                    _stateManager.StopTractor();
                    break;
                case "zaoraj":
                  
                    foreach (Field field in fieldsPointed)
                        _stateManager.PlowAt(field.Row, field.Column);
                    break;
                case "zasiej":
                    foreach (Field field in fieldsPointed)
                        _stateManager.SowAt(field.Row, field.Column);
                    break;
                case "zbierz":
                    foreach (Field field in fieldsPointed)
                        _stateManager.HarvestAt(field.Row, field.Column);
                    break;
                case "podlej":
                    foreach (Field field in fieldsPointed)
                        _stateManager.IrrigateAt(field.Row, field.Column);
                    break;
                case "start":
                    //do czego to? ;p
                    break;
            }
        }




        private void Execute(Phrase phrase)
        {
             //String Content = 
             //           + ((StateManager)_stateManager).fieldItems[r][c].Type
             //           + "\n" + ((StateManager)_stateManager).fieldItems[r][c].State,


            var tasks = prepareTaskOrder(phrase.Tasks);
            List<Field> fieldsPointed = new List<Field>();

            foreach (var task in tasks)
            {
                
                System.IO.StreamWriter file = new System.IO.StreamWriter("f:\\test.txt", true);
                file.WriteLine("task v:"+task.Value); //to jest co ma zrobic
                var activeTask = task.Value;
                foreach (var complement in task.Complements)
                {
                    fieldsPointed = choseFiledsPointedByUser(complement.Value, complement.Attributes);
                    runAction(task.Value, fieldsPointed);
                    file.WriteLine(complement.Value); //to jest na czym ma zrobic (szczegoly  w filtrach)

                    foreach (var atr in complement.Attributes)
                    {
                        file.WriteLine("compl atr:" + atr); // to sa filtry
                    }
                }
              
                // potrzebna cala tablica z fildami na ktorych maja byc wykonywane polecenia
                file.WriteLine("field 1 1 state:" + ((StateManager)_stateManager).fieldItems[1][1].State);
                file.WriteLine("field 1 1 type:" + ((StateManager)_stateManager).fieldItems[1][1].Type);

              
                foreach (var Adverb in task.Adverbials)
                {
                    file.WriteLine("adv: "+Adverb);    // to jest kolejnosc (nastepne)
                }
               
                 file.Close();
            }
        }
    }
}
