using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteligentnyTraktor
{
    class Traktor
    {
        string Imie = "Traktor Tymoteusz"; //zeby bylo zabawnie :P
        int Przebieg = 0; //km
        int Waga = 2500; //kg
        int Pojemnosc = 3000; //l
        int Paliwo = 200; //l
        int Predkosc_Max = 60; //km/h
        int Predkosc_Srednia = 37; //km/h

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

        public int Jedz(string Gdzie2) 
        {
            return 0; /* do sklepu
	                     na stacje benzynową itd.
                      */
        }
    }
}
