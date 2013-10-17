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

        public MainWindow()
        {
            InitializeComponent();
            InitializeFieldGrid(6);
        }
<<<<<<< HEAD
        private void InitializeFieldGrid()
=======

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
>>>>>>> origin/buttony-blad
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
                ch.Add(new Button()
                    {
                        Content = r.ToString() + " " + c.ToString(),
                    });
                Grid.SetRow(ch[i], r);
                Grid.SetColumn(ch[i], c);
                this.fieldItems[r][c] = ch[i];
            }                        
        }
    }
}
