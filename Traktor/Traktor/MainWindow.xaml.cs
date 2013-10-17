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

namespace Traktor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < 4; i++)
            {
                gridField.RowDefinitions.Add(
                    new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });

                gridField.ColumnDefinitions.Add(
                    new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
            }

            var c = gridField.Children;

            for (int i = 0; i < 16; i++)
            {
                c.Add(
                new Button()
                {
                    Name = "button" + i.ToString(),
                    Content = "klikaj",
                });
            }
            
            for (int i = 0; i < 16; i++)
            {
                Grid.SetRow(c[i], i % 4);                 
                Grid.SetColumn(c[i], i/4);
            }
        }
    }
}
