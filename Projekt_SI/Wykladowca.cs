using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_SI
{
    class Wykladowca 
    {
        public String imie;
        public int czas;
        public Zajecie[,] plan = new Zajecie[5, 40];
        public int[,] plan_i = new int[5, 50];
        public Boolean[] dostep_dzien = new Boolean[5];
        public int dni=0;
        public int[,] dostepnosc;
        public int[] od_ktorej = new int[5];
        public Wykladowca(String input_imie, int[,] input_dostepnosc )
        {
            this.imie = input_imie;
            this.dostepnosc = (int[,])input_dostepnosc.Clone();
            policz_dni();
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 40; j++)
                    plan_i[i, j] = 0;
        }
        public void policz_dni()
        {
            int licznik = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 40; j++)
                    if(this.dostepnosc[i,j]==1)
                        licznik++;
                if (licznik > 0)
                {
                    this.dostep_dzien[i] = true;
                    this.dni++;
                }   
                licznik = 0;
            }
        }
    }
}
