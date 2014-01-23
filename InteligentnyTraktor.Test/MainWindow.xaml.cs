using System;
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

        LPDictionary LPDict ;
        Compiler Comp;


        UIElement[][] fieldItems;
        Image tractor;

        double vx = 0;
        double vy = 0;

        int _size = 4; 
        public MainWindow()
        {
            int size = _size;

            InitializeComponent();

            InitializeBackgroundImage(); //dodanie tła; domyślnie powinno wyglądać jak w fullscreenie przy rozdzielczosci 1366x768

            stateManager = new StateManager(fieldCanvas.Width, fieldCanvas.Height, size, size);
            LPDict = new LPDictionary(stateManager, _size);
            Comp = new Compiler(stateManager, _size);

            stateManager.TractorIsBusy += (s, e) => labelCommunication.Content = "traktor jest zajęty";

            InitializeFieldGrid(size);
            InitializeTractor();
            InitializeFieldEvents();
            gridField.ShowGridLines = false;
            //labelCommunication.Content = fieldItems.Length + " " + fieldItems[0].Length;
            //inicjalizacja przezroczystego canvasu; dorobić jakiś slider
            textCanvas.Background = new SolidColorBrush()
            {
                Color = Color.FromRgb(255, 255, 255),
                Opacity = 0.35
            };

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
                        ChangeFieldImage(fieldItems[e.row][e.column] as Label, ((StateManager)stateManager).fieldItems[e.row][e.column].State);
                    }));
                };              
        }

        private void ChangeFieldImage(Label l, FieldItemState state)
        {
            string uri = "pack://application:,,,/InteligentnyTraktor;component/";
            switch (state)
            {
                case FieldItemState.Bare:
                    uri += "Bare.png"; break;
                case FieldItemState.Plowed:
                    uri += "Plowed.png"; break;
                case FieldItemState.Sowed:
                    uri += "Sowed.png"; break;
                case FieldItemState.EarlyGrowing:
                    uri += "EarlyGrowing.png"; break;
                case FieldItemState.MidGrowing:
                    uri += "MidGrowing.png"; break;
                case FieldItemState.LateGrowing:
                    uri += "LateGrowing.png"; break;
                case FieldItemState.Mature:
                    uri += "Mature.png"; break;
                case FieldItemState.Rotten:
                    uri += "Rotten.png"; break;
                case FieldItemState.Harvested:
                    uri += "Harvested.png"; break;
            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(uri, UriKind.Absolute);
            bi.EndInit();

            l.Background = new ImageBrush()
            {
                ImageSource = bi,
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
            tractor = new Image()
            {
                Width = 75,
                Height = 50,
            };
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(
                "pack://application:,,,/InteligentnyTraktor;component/traktor.png", UriKind.Absolute
            );
            bi.EndInit();

            tractor.Source = bi;

            fieldCanvas.Children.Add(tractor);
        }

        private void InitializeBackgroundImage()
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(
                "pack://application:,,,/InteligentnyTraktor;component/background.png", UriKind.Absolute
            );
            bi.EndInit();
            mainWindow.Background = new ImageBrush()
            {
                ImageSource = bi,
            };
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
            AddImagesForEachField();
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

        private void AddImagesForEachField()
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(
                "pack://application:,,,/InteligentnyTraktor;component/Bare.png", UriKind.Absolute
            );
            bi.EndInit();
            ImageBrush brush = new ImageBrush()
            {
                ImageSource = bi,
            };

            var ch = gridField.Children;
            foreach(Label l in ch) 
            {
                l.Background = brush;
            }
        }

        private int _r;
        private int _c;
        /*private bool is_good()
        {
            int r;
            int c;

            //bool firstParse = int.TryParse(textBoxEnterRow.Text, out r);
            //bool secondParse = int.TryParse(textBoxEnterColumn.Text, out c);
            //bool result = firstParse && secondParse;

            //textBoxEnterRow.Text = "";
            //textBoxEnterColumn.Text = "";

            if (!result)
            {
                return false;
            }

            if ((r > fieldItems.Length - 1) || (c > fieldItems[0].Length - 1))
            {
                return false;
            }
            this._r = r;
            this._c = c;
            return true;
        }*/
        
        /*private void buttonMoveTractor_Click(object sender, RoutedEventArgs e)
        {
          
            if(is_good())
                stateManager.MoveTractorTo(_r, _c);
            return;
        }*/

        /*private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            labelCommunication.Content = "";
            stateManager.StopTractor();
        }

        private void buttonPlow_Click(object sender, RoutedEventArgs e)
        {
            if (is_good())
                stateManager.PlowAt(_r, _c);
            return;
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
        }*/

        private void ButtonDo_Click(object sender, RoutedEventArgs e)
        {
            string commend = textBoxEnterCommend.Text;
            Comp.RunCompiler(commend);
            if (commend != "")
            {
                commendLabel.Content += commend + "\n"; //nie za dużo razy wywoływane? może stworzyć w klasie stringbuildera
            }
            LPDict.CheckActionTypeAndRunIt(commend);
        }
    }
}
