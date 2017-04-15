using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_SI
{
    class Zajecie
    {
        public Przedmiot przedmiot;
        public int dlugosc;
        public int miejsca=0;
        public int i;
        public int j;
        public int ilosc = 1;
        public int grupa = 0;
        //public Sala sala;
        public Zajecie(Przedmiot p,int miejsc)
        {
            this.miejsca = miejsc;
            this.przedmiot = p;
            Double temp = Math.Floor(((p.przewidziania_ilosc_h / 15) * 60) / 15);
            this.dlugosc = (int)temp;
        }
    }
}
