﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InteligentnyTraktor.Model;
using InteligentnyTraktor.LanguageProcessing;
using System.Timers;
using InteligentnyTraktor;
using System.Text.RegularExpressions;

namespace InteligentnyTraktor.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IStateManager stateManager;
        Timer timer = new Timer(20);

        LPDictionary LPDict = new LPDictionary();


        

        UIElement[][] fieldItems;
        Rectangle tractor;

        double vx = 0;
        double vy = 0;

        public MainWindow()
        {
            int size = 4;

            InitializeComponent();

            stateManager = new StateManager(fieldCanvas.Width, fieldCanvas.Height, size, size);
            stateManager.TractorIsBusy += (s, e) => labelCommunication.Content = "traktor jest zajęty";

            InitializeFieldGrid(size);
            InitializeTractor();
            InitializeFieldEvents();           

            labelCommunication.Content = fieldItems.Length + " " + fieldItems[0].Length;

            timer.Start();
            timer.Elapsed += timer_Elapsed;
            timer.Elapsed += UpdateTractorProperties;
            
            stateManager.FieldChanged += (s, e) => 
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        (fieldItems[e.row][e.column] as Label).Content =
                            e.row.ToString() + " " + e.column.ToString() + "\n"
                            + ((StateManager)stateManager).fieldItems[e.row][e.column].Type
                            + "\n" + ((StateManager)stateManager).fieldItems[e.row][e.column].State;
                    }));
                };              
        }

        private void UpdateTractorProperties(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                var t = (this.stateManager as StateManager).Tractor;
                labelDirection.Content = "direct. " + t.Direction.X.ToString("0.##") + " " + t.Direction.Y.ToString("0.##");
                labelPosition.Content = "pos. " + t.Position.X.ToString("0.##") + " " + t.Position.Y.ToString("0.##");

                if (t.Velocity.X != 0 || t.Velocity.Y != 0)
                {
                    vx = t.Velocity.X;
                    vy = t.Velocity.Y;
                }

                labelVelocity.Content = "vel. " + vx.ToString("0.##") + " " + vy.ToString("0.##");
                //labelVelocity.Content =  "vel. " + t.Velocity.X.ToString("0.##") + " " + t.Velocity.Y.ToString("0.##");
            }));
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                tractor.SetValue(Canvas.LeftProperty, stateManager.TractorPosition.X - tractor.Width / 2);
                tractor.SetValue(Canvas.TopProperty, stateManager.TractorPosition.Y - tractor.Height / 2);

                var angle = Math.Atan2(stateManager.TractorDirection.Y, stateManager.TractorDirection.X) * (180.0 / Math.PI);
                RotateTransform rotate = new RotateTransform(
                           angle,
                           tractor.Width / 2,
                           tractor.Height / 2
                           );
                tractor.RenderTransform = rotate;
            }));
            
        }

        private void InitializeTractor()
        {
            tractor = new Rectangle()
            {
                Fill = Brushes.Red,
                Width = 50,
                Height = 25,
            };

            fieldCanvas.Children.Add(tractor);
        }

        private void InitializeFieldEvents()
        {
            foreach (var el in gridField.Children)
            {
                var e = el as UIElement;
                e.MouseDown += MoveTractorHere;
            }
        }

        private void MoveTractorHere(object s, MouseButtonEventArgs e)
        {
            labelCommunication.Content = "";

            var el = s as UIElement;
            if (el == null)
	        {
		        throw new NullReferenceException();
	        }

            int r = Grid.GetRow(el);
            int c = Grid.GetColumn(el);

            stateManager.MoveTractorTo(r, c);            
        }

        private void InitializeFieldGrid(int size)
        {
            DefineRowsAndColumns(gridField, size);           
            
            fieldItems = new UIElement[size][];
            for (int i = 0; i < size; i++)
            {
                fieldItems[i] = new UIElement[size];
            } 

            AddContentForEachField(gridField, size);
        }        

        private void DefineRowsAndColumns(Grid grid, int size)
        {
            for (int i = 0; i < size; i++)
            {
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star),
                    });

                grid.RowDefinitions.Add(
                    new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star),
                    });
            }
        }

        private void AddContentForEachField(Grid grid, int size)
        {
            for (int i = 0; i < size * size; i++)
            {
                int r = i % size;
                int c = i / size;

                var ch = grid.Children;
                ch.Add(new Label()
                    {
                        Content = r.ToString() + " " + c.ToString() + "\n"
                        + ((StateManager)stateManager).fieldItems[r][c].Type
                        + "\n" + ((StateManager)stateManager).fieldItems[r][c].State,
                    });
                Grid.SetRow(ch[i], r);
                Grid.SetColumn(ch[i], c);
                this.fieldItems[r][c] = ch[i];
            }                        
        }
        private int _r;
        private int _c;
        private bool is_good()
        {
            int r;
            int c;

            bool firstParse = int.TryParse(textBoxEnterRow.Text, out r);
            bool secondParse = int.TryParse(textBoxEnterColumn.Text, out c);
            bool result = firstParse && secondParse;

            textBoxEnterRow.Text = "";
            textBoxEnterColumn.Text = "";

            if (!result)
            {
                return false;
            }

            else if ((r > fieldItems.Length - 1) || (c > fieldItems[0].Length - 1))
            {
                return false;
            }
            this._r = r;
            this._c = c;
            return true;
        }
         private bool isGoodCordinates(int r, int c)
        {
            if ((r > fieldItems.Length - 1) || (c > fieldItems[0].Length - 1))
            {
                return false;
            }
            this._r = r;
            this._c = c;
            return true;
        }
        private void buttonMoveTractor_Click(object sender, RoutedEventArgs e)
        {
          
            if(is_good())
                stateManager.MoveTractorTo(_r, _c);
            return;
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            labelCommunication.Content = "";
            stateManager.StopTractor();
        }

        private void buttonSow_Click(object sender, RoutedEventArgs e)
        {
            if (is_good())
                stateManager.SowAt(_r, _c);
            return;
                
            
        }

        private void buttonFertilize_Click(object sender, RoutedEventArgs e)
        {
           if (is_good())
               stateManager.FertilizeAt(_r, _c);
            return;
               
            
        }

        private void buttonHarvest_Click(object sender, RoutedEventArgs e)
        {
            if (is_good())
                stateManager.HarvestAt(_r, _c);
            return;
          
        }

        private void ButtonDo_Click(object sender, RoutedEventArgs e)
        {
            string commend =textBoxEnterCommend.Text;
            string[] commends = commend.Split(new Char[] { ' ', ',', '.', ':', '\t' });

            
           
            string isNumber;
            int r=0, c=0, whichNumber;
             whichNumber=0;
            foreach (string com in commends)
            {

                isNumber = Regex.Match(com, @"\d+").Value;
                if(  (isNumber != "") && (whichNumber!=2))
                {
                    if(whichNumber==0)
                       r = Int32.Parse(isNumber);
                    else if(whichNumber==1)
                       c = Int32.Parse(isNumber);
                    whichNumber++;
                }

            }
            if (whichNumber == 2 && isGoodCordinates(r, c))
            {
                stateManager.MoveTractorTo(_r, _c);
            }
            bool cando = LPDict.Dict.ContainsKey(commend);

            if (cando)
            {
                textBoxEnterCommend.Text = "ok";
            }
                 

          
        }
    }
}
