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

            InitializeBackgroundImage();

            stateManager = new StateManager(fieldCanvas.Width, fieldCanvas.Height, size, size);
            LPDict = new LPDictionary(stateManager, _size);
            Comp = new Compiler(stateManager, _size);

            stateManager.TractorIsBusy += (s, e) => labelCommunication.Content = "traktor jest zajęty";
            
            //adjust window width and height to screen resolution
            mainWindow.Width = SystemParameters.PrimaryScreenWidth;
            mainWindow.Height = SystemParameters.PrimaryScreenHeight;

            AdjustPositionAndSizeOfElements();
            InitializeFieldGrid(size);
            InitializeTractor();
            InitializeFieldEvents();

            //labelCommunication.Content = fieldItems.Length + " " + fieldItems[0].Length;

            //inicjalizacja przezroczystego canvasu; dorobić do niego jakiś slider
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
                        /*(fieldItems[e.row][e.column] as Label).Content =
                            e.row.ToString() + " " + e.column.ToString() + "\n"
                            + ((StateManager)stateManager).fieldItems[e.row][e.column].Type
                            + "\n" + ((StateManager)stateManager).fieldItems[e.row][e.column].State;*/
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
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var t = (this.stateManager as StateManager).Tractor;
                    
                    if (t.Velocity.X != 0 || t.Velocity.Y != 0)
                    {
                        vx = t.Velocity.X;
                        vy = t.Velocity.Y;

                        ChangeTractorImage();
                    }
                }));
            }
            catch (TaskCanceledException tce)
            {

            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
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
            catch (TaskCanceledException tce)
            {

            }
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

        private void ChangeTractorImage()
        {
            string uri = "pack://application:,,,/InteligentnyTraktor;component/traktor.png";
            if (tractor.Source.ToString() == uri)
            {
                uri = "pack://application:,,,/InteligentnyTraktor;component/traktor_1.png";
            }
            else
            {
                uri = "pack://application:,,,/InteligentnyTraktor;component/traktor.png";
            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(
                uri, UriKind.Absolute
            );
            bi.EndInit();

            tractor.Source = bi;
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
            for (int i = 0, k = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++, k++)
                {
                    var ch = grid.Children;
                    ch.Add(new Label()
                    {
                        //Foreground = new SolidColorBrush(Color.FromRgb(26, 0, 0)),
                        Foreground = Brushes.White,
                        FontSize = 16,
                        Content = i * 4 + j + 1 + "\n"
                        //+ ((StateManager)stateManager).fieldItems[r][c].Type
                        //+ "\n" + ((StateManager)stateManager).fieldItems[r][c].State,
                    });
                    Grid.SetRow(ch[k], i);
                    Grid.SetColumn(ch[k], j);
                    this.fieldItems[i][j] = ch[k];
                }
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

        /*private void buttonMoveTractor_Click(object sender, RoutedEventArgs e)
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

        private void AdjustPositionAndSizeOfElements()
        {
            double marginLeft = mainWindow.Width - closeButton.Width;
            double marginTop = 0;
            closeButton.Margin = new Thickness(marginLeft - 5, marginTop + 5, 0, 0);

            //ustawienie canvasu zawierajacego grida (pola + traktory)
            marginLeft = 0.05 * mainWindow.Width;
            marginTop = (mainWindow.Height - fieldCanvas.Height) / 2;
            fieldCanvas.Margin = new Thickness(marginLeft, marginTop, 0, 0);

            //ustawienie canvasu, ktory zawiera kontrolki do komunikacji z traktorem
            textCanvas.Width = 0.42 * mainWindow.Width;
            textCanvas.Height = 0.82 * mainWindow.Height;
            marginLeft = marginLeft + gridField.Width + 50;
            marginTop = (mainWindow.Height - textCanvas.Height) / 2;
            textCanvas.Margin = new Thickness(marginLeft, marginTop, 0, 0);

            //ustawienie labela, ktory wyswietla tekst dialogu
            commendLabel.Width = 0.95 * textCanvas.Width;
            commendLabel.Height = 0.88 * textCanvas.Height;
            //commendTextBox.Opacity = 0.3;
            /*commendTextBox.Background = null;
            commendTextBox.BorderBrush = null;*/
            /*marginLeft = 0.05 * textCanvas.Width;
            marginTop = 0.15 * textCanvas.Height;
            commendLabel.Margin = new Thickness(marginLeft, marginTop, 0, 0);*/

            //ustawienie text boxa, do ktorego wpisuje sie komendy
            textBoxEnterCommend.Width = 0.7 * textCanvas.Width;
            marginLeft = 0.025 * textCanvas.Width;
            marginTop = 0.91 * textCanvas.Height;
            textBoxEnterCommend.Margin = new Thickness(marginLeft, marginTop, 0, 0);

            //ustawienie buttona 'wykonaj'
            marginLeft = (textCanvas.Width - (marginLeft + textBoxEnterCommend.Width) - ButtonDo.Width) / 2;
            marginTop = marginTop + (textBoxEnterCommend.Height - ButtonDo.Height) / 2;
            ButtonDo.Margin = new Thickness(marginLeft + textBoxEnterCommend.Width, marginTop, 0, 0);

            textBoxEnterCommend.Focus();
        }

        private void ButtonDo_Click(object sender, RoutedEventArgs e)
        {
            string commend = textBoxEnterCommend.Text;
            Comp.RunCompiler(commend);
            if (commend != "")
            {
                /*commendLabel.Content += commend + "\n";
                textBoxEnterCommend.Clear();*/
            }
            LPDict.CheckActionTypeAndRunIt(commend);
        }

        private void buttonDo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string commend = textBoxEnterCommend.Text;
                String respond = Comp.RunCompiler(commend);
                if (commend != "")
                {
                    commendLabel.Content += "Ty: ";
                    commendLabel.Content += commend + "\n";
                    commendLabel.Content += "Traktor: ";
                    commendLabel.Content += respond + "\n";
                    textBoxEnterCommend.Clear();
                }
                /*if (commend != "")
                {
                    TextRange tr = new TextRange(commendTextBox.Document.ContentEnd, commendTextBox.Document.ContentEnd);
                    tr.Text = "Ty: ";
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                    /*commendTextBox.AppendText(
                            "Ty: " + commend + "\n"
                            + "Traktor: " + respond + "\n"
                        );*//*

                    commendTextBox.AppendText(commend + "\n");
                    textBoxEnterCommend.Clear();
                }*/
                LPDict.CheckActionTypeAndRunIt(commend);
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
