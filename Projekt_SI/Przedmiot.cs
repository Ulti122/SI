using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_SI
{
    class Przedmiot
    {
        public String nazwa;
        public String typ;
        public Wykladowca nauczyciel;
        public Double przewidziania_ilosc_h;
        public int semestr;
        public Sala sala;
        public String specjalizacja;
        public int ilosc_w_grupie;
        public Boolean multi_spec = false;
        public Przedmiot(String input_nazwa,String input_typ, Double input_ilosc_h, int input_semestr, String input_specka,int input_miejsca)
        {
            this.nazwa = input_nazwa;
            this.typ = input_typ;
            this.przewidziania_ilosc_h = input_ilosc_h;
            this.semestr = input_semestr;
            this.specjalizacja = input_specka;
            this.ilosc_w_grupie = input_miejsca;
        }
    }
}
