using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TwoRectangles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class Player
    {
        private FrameworkElement shape;
        private Point actualLocation;
        private int dx;
        private int dy;
        private bool isRendering;
        public ControlKeys Klawisze;
        public Player(FrameworkElement frameworkElement, ControlKeys Klawisze)
        {
            shape = frameworkElement;
            actualLocation = new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));
            this.Klawisze = Klawisze;
            isRendering = false;
            //CompositionTarget.Rendering += movingPlayer;
        }

        public Point Location
        {
            get { return actualLocation; }
            set
            {
                actualLocation = value;
                Canvas.SetLeft(shape, actualLocation.X);
                Canvas.SetTop(shape, actualLocation.Y);
            }
        }
        public int X
        {
            get { return (int)actualLocation.X; }
            set
            {
                actualLocation.X = value;
                Canvas.SetLeft(shape, actualLocation.X);
            }
        }
        public int Y
        {
            get { return (int)actualLocation.Y; }
            set
            {
                actualLocation.Y = value;
                Canvas.SetTop(shape, actualLocation.Y);
            }
        }


        public void movingPlayer(object sender, EventArgs e)
        {
            SetLocation(new Point(X + dx, Y + dy));
        }
        public void Move(KeyEventArgs e)
        {
            if (!isRendering)
            {
                CompositionTarget.Rendering += movingPlayer;
                isRendering = true;
            }
            if (e.Key == Klawisze.Up)
            {
                dy = -1;
                return;
            }
            if (e.Key == Klawisze.Down)
            {
                dy = 1;
                return;
            }
            if (e.Key == Klawisze.Right)
            {
                dx = 1;
                return;
            }
            if (e.Key == Klawisze.Left)
            {
                dx = -1;
                return;
            }
        }
        public void Stop(KeyEventArgs e)
        {
            if (isRendering)
            {
                CompositionTarget.Rendering -= movingPlayer;
                isRendering = false;
            }
            if (e.Key == Klawisze.Up || e.Key == Klawisze.Down)
            {
                dy = 0;
                return;
            }
            if (e.Key == Klawisze.Right || e.Key == Klawisze.Left)
            {
                dx = 0;
                return;
            }
        }
        public void SetLocation(Point newLocation)
        {
            actualLocation = newLocation;
            Canvas.SetLeft(shape, actualLocation.X);
            Canvas.SetTop(shape, actualLocation.Y);
        }
    }

    public struct ControlKeys
    {
        private Key up;
        private Key down;
        private Key right;
        private Key left;

        public ControlKeys(Key up, Key down, Key right, Key left)
        {
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
        }

        public Key Up
        {
            get { return up; }
            set { up = value; }
        }
        public Key Down
        {
            get { return down; }
            set { down = value; }
        }
        public Key Right
        {
            get { return right; }
            set { right = value; }
        }
        public Key Left
        {
            get { return left; }
            set { left = value; }
        }
    }


    public partial class MainWindow : Window
    {
        private Player player1;
        private Player player2;
        public MainWindow()
        {
            InitializeComponent();
            player1 = new Player((FrameworkElement)((Canvas)this.Content).Children[0], new ControlKeys(Key.Up, Key.Down, Key.Right, Key.Left));
            player2 = new Player((FrameworkElement)((Canvas)this.Content).Children[1], new ControlKeys(Key.W, Key.S, Key.D, Key.A));
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            player1.Move(e);
            player2.Move(e);
        }

        private void Window_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == player1.Klawisze.Up || e.Key == player1.Klawisze.Down
                || e.Key == player1.Klawisze.Left || e.Key == player1.Klawisze.Right)
            {
                player1.Stop(e);
            }
            if (e.Key == player2.Klawisze.Up || e.Key == player2.Klawisze.Down
                || e.Key == player2.Klawisze.Left || e.Key == player2.Klawisze.Right)
            {
                player2.Stop(e);
            }
        }
    }
}
