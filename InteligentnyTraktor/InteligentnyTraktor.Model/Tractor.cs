using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteligentnyTraktor.Model
{
    class Tractor
    {

#if(old)
        //string Imie = "Traktor Tymoteusz"; //zeby bylo zabawnie :P
        //int Przebieg = 0; //km
        //int Waga = 2500; //kg
        //int Pojemnosc = 3000; //l
        //int Paliwo = 200; //l
#endif

        public double VMax { get; set; }
        public Point Position { get; set; }
        public Vector Velocity { get; set; }
        public double Acceleration { get; set; }

        public void Move(double ds)
        {
            /*
            var dx = (Velocity.X * ds) / Velocity.LengthSquared;
            var dy = (Velocity.Y * ds) / Velocity.LengthSquared;
            Position = new Point(Position.X + dx, Position.Y + dy);
             */
            Position = new Point(Position.X + Velocity.X, Position.Y + Velocity.Y);
        }

        public void Accelerate()
        {
            Velocity = Velocity * Acceleration;
        }

#if(old)
        public int ZbierzPlony(int IDPola)
        {
            return 0;
        }

        public int Zaoraj(int IDPola) 
        {
            return 0;
        }

        public int Podlej(int IDPola) 
        {
            return 0;
        }

        public int Nawiez(int IDPola, string Czym) 
        {
            //czym -> nawóz sztuczny lub naturalny
        }

        public int Zawiez(string Gdzie1) 
        {
            return 0; /*punktu skupu (kasa)
	                    magazynu (do późniejszego siewu)
	                    kompostownik (na zepsute plony)
                      */
        }
        
         * 
        public int Jedz(string Gdzie2) 
        {
            return 0; /* do sklepu
	                     na stacje benzynową itd.
                      */
        }
#endif      
    }
}
