using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_SI
{
    class Plany
    {
        public Zajecie[,] plan = new Zajecie[5, 40];//struktura do v2
        public List<Zajecie> plann = new List<Zajecie>();
        public List<Sala> sale = new List<Sala>();
        public List<Wykladowca> wykladowcy = new List<Wykladowca>();
        public int semestr;
        public int[,] plan_i = new int[5, 40];//struktura do v2
        public int ocena;
        public Plany()//v2
        {

        }
        public void dodaj_plan(Zajecie[,] input_plan)//v2
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 40; j++)
                    plan_i[i, j] = 0;
            this.plan = (Zajecie[,])input_plan.Clone();
        }
        public void kompatobilnosc()//przejscie na plan_i
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 40; j++)
                    plan_i[i, j] = 0;
            foreach(Zajecie z in plann)
            {
                for (int j = 0; j < z.dlugosc; j++)
                    plan_i[z.i, z.j+j]++;
            }
        }
        public void odejmij_z_plan_i(Zajecie z)
        {
            for (int j = 0; j < z.dlugosc; j++)
                plan_i[z.i, z.j + j]--;
        }
        public int max_plan_i(Zajecie z)
        {
            int x = 0;
            for (int j = 0; j < z.dlugosc; j++)
                if (x < plan_i[z.i, z.j + j])
                    x = plan_i[z.i, z.j + j];
            return x;
        }
    }
    //public class Plany
    //{
    //    Zajecie[,] planx = new Zajecie[5, 40];
    //    List<Sala> salex = new List<Sala>();
    //    List<Wykladowca> wykladowcyx = new List<Wykladowca>();
    //    int semestrx;
    //    int[,] plan_ix = new int[5, 50];
    //    int ocenax;

    //    internal Zajecie[,] plan
    //    {
    //        get
    //        {
    //            return planx;
    //        }

    //        set
    //        {
    //            planx = value;
    //        }
    //    }

    //    internal List<Sala> sale
    //    {
    //        get
    //        {
    //            return salex;
    //        }

    //        set
    //        {
    //            salex = value;
    //        }
    //    }

    //    internal List<Wykladowca> wykladowcy
    //    {
    //        get
    //        {
    //            return wykladowcyx;
    //        }

    //        set
    //        {
    //            wykladowcyx = value;
    //        }
    //    }

    //    public int semestr
    //    {
    //        get
    //        {
    //            return semestrx;
    //        }

    //        set
    //        {
    //            semestrx = value;
    //        }
    //    }

    //    public int[,] plan_i
    //    {
    //        get
    //        {
    //            return plan_ix;
    //        }

    //        set
    //        {
    //            plan_ix = value;
    //        }
    //    }

    //    public int ocena
    //    {
    //        get
    //        {
    //            return ocenax;
    //        }

    //        set
    //        {
    //            ocenax = value;
    //        }
    //    }

    //    public Plany()
    //    {

    //    }
    //    void dodaj_plan(Zajecie[,] input_plan)
    //    {
    //        for (int i = 0; i < 5; i++)
    //            for (int j = 0; j < 40; j++)
    //                plan_ix[i, j] = 0;
    //        this.plan = (Zajecie[,])input_plan.Clone();
    //    }
    //}
}
