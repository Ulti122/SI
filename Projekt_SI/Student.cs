using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_SI
{
    class Student
    {
        public String imie;
        public int semestr;
        public String specjalizacja;
        public Zajecie[,] plan = new Zajecie[5, 40];
        public Student(String input_imie, int input_semestr, String input_specka)
        {
            this.semestr = input_semestr;
            this.imie = input_imie;
            this.specjalizacja = input_specka;
        }
        public Boolean Czy_ma_przedmiot(Przedmiot p)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    if(this.plan[i, j]!=null)
                        if (this.plan[i, j].przedmiot.nazwa == p.nazwa)
                            if (this.plan[i, j].przedmiot.typ == p.typ)
                                return true;
                }
            }
            return false;
        }
    }
}
