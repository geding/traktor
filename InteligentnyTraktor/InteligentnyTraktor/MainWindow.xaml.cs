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

namespace InteligentnyTraktor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UIElement[][] fieldItems;
        Rectangle tractor;

        public MainWindow()
        {
            InitializeComponent();
            InitializeFieldGrid(6);
            InitializeTractor();
            InitializeFieldEvents();
        }

        private void InitializeTractor()
        {
            tractor = new Rectangle()
            {
                Fill = Brushes.Red,
            };

            gridField.Children.Add(tractor);
            Grid.SetRow(tractor, 0);
            Grid.SetColumn(tractor, 0);
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
            var el = s as UIElement;
            if (el == null)
	        {
		        throw new NullReferenceException();
	        }

            int r = Grid.GetRow(el);
            int c = Grid.GetColumn(el);

            Grid.SetRow(tractor, r);
            Grid.SetColumn(tractor, c);
        }

        private void InitializeFieldGrid(int size)
        {
            DefineRowsAndColumns(gridField, size);           
            
            fieldItems = new UIElement[size][];
            for (int i = 0; i < size; i++)
            {
                fieldItems[i] = new UIElement[size];
            } 

            AddButtonsForEachField(gridField, 6);
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

        private void AddButtonsForEachField(Grid grid, int size)
        {
            for (int i = 0; i < size * size; i++)
            {
                int r = i % size;
                int c = i / size;

                var ch = grid.Children;
                ch.Add(new Label()
                    {
                        Content = r.ToString() + " " + c.ToString(),
                    });
                Grid.SetRow(ch[i], r);
                Grid.SetColumn(ch[i], c);
                this.fieldItems[r][c] = ch[i];
            }                        
        }

        private void buttonMoveTractor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int r = int.Parse(textBoxEnterRow.Text);
                int c = int.Parse(textBoxEnterColumn.Text);

                if (r > 5 || c >5 )
                {
                    return;
                }

                MoveTractorHere(fieldItems[r][c], null);
            }
            catch (FormatException) { }
            finally 
            {
                textBoxEnterRow.Text = "";
                textBoxEnterColumn.Text = "";
            }           
        }
    }
}
