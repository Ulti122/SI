using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_SI
{
    class Sala
    {
        public int numer;
        public String typ;
        public int miejsc;
        public int[,] dostepnosc_sali = new int[5, 40];
        public Sala(int input_numer, int input_miejsc, String input_typ)
        {
            this.numer = input_numer;
            this.miejsc = input_miejsc;
            this.typ = input_typ;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 40; j++)
                    dostepnosc_sali[i, j] = 0;
        }
    }
    
}
