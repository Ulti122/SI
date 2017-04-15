using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_SI
{
    public partial class f_plan : Form
    {
        public f_plan()
        {
            InitializeComponent();
        }
        public f_plan(int plan_id)
        {
            InitializeComponent();
            int[,] temp_ilosc = new int[5, 40];
            foreach(Zajecie z in ((Form1)Application.OpenForms[0]).Planki[plan_id].plann)
            {
                for (int i = 0; i < z.dlugosc; i++)
                    temp_ilosc[z.i, z.j + i] = ((Form1)Application.OpenForms[0]).Planki[plan_id].max_plan_i(z);
            }
            int[,] pozycja = new int[5, 40];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 40; j++)
                    pozycja[i, j] = temp_ilosc[i,j]-1;
            int szerokosc_butona = 300;
            this.BackColor= Color.White;
            Color kolor = Color.LightGray;
            int godzina = 8;//poczatkowa godzina do labeli
            ////
            int temp_godzina = godzina;
            Label ll = new Label();
            ll.TextAlign = ContentAlignment.MiddleCenter;
            ll.Width = 100;
            ll.Height = 40;
            ll.Text = ((Form1)Application.OpenForms[0]).kto_selected;
            ll.Location = new Point(0, 0);
            ll.BackColor = kolor;
            this.Controls.Add(ll);
            for (int i = 0; i < 5; i++)
            {
                Label l = new Label();
                l.TextAlign = ContentAlignment.MiddleCenter;
                if (i == 0)
                    l.Text = "Poniedziałek";
                else if (i == 1)
                    l.Text = "Wtorek";
                else if (i == 2)
                    l.Text = "Środa";
                else if (i == 3)
                    l.Text = "Czwartek";
                else if (i == 4)
                    l.Text = "Piątek";
                l.Height = 40;
                l.Width = szerokosc_butona;
                l.Location = new Point(100 + i * szerokosc_butona);
                l.BackColor = kolor;
                this.Controls.Add(l);
                Label lll = new Label();
                lll.Width = 2;
                lll.Height = 860;
                lll.BackColor = Color.Black;
                lll.Location = new Point(100 + i * szerokosc_butona);
                this.Controls.Add(lll);
            }
            for (int i = 0; i < 40 + 1; i++)
            {
                Label l = new Label();
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Location = new Point(0, 40 + i * 20);
                int tempo = (i % 4) * 15;
                if (tempo == 0 && i > 0)
                    temp_godzina++;
                if (tempo == 0)
                    l.Text = temp_godzina + ":" + "00";
                else
                    l.Text = temp_godzina + ":" + tempo;
                this.Controls.Add(l);
                this.Height = (60 + (40 + i * 20));
                l.BackColor = kolor;
                this.Size = new Size(szerokosc_butona * 5 + 100, (60 + (40 + i * 20)));
            }
            //real shit 
            int kto = plan_id;
            ll.Text = "\n Semestr : " + ((Form1)Application.OpenForms[0]).Planki[kto].semestr;

            foreach (Zajecie z in ((Form1)Application.OpenForms[0]).Planki[kto].plann)
            {
                Label l = new Label();
                l.Location = new Point(((z.i * szerokosc_butona) + 100), ((z.j * 20) + 40));
                //((Form1)Application.OpenForms[0]).Planki[kto].odejmij_z_plan_i(z);
                l.Height = 20 * z.dlugosc;
                l.Width = szerokosc_butona;
                if (temp_ilosc[z.i, z.j]  > 1) 
                {
                    int temp_poz = pozycja[z.i, z.j];
                    int k = 1;
                    l.Width = szerokosc_butona / temp_ilosc[z.i,z.j];
                    if (pozycja[z.i, z.j] < 0)
                        pozycja[z.i, z.j] = 0;
                    l.Location = new Point((((z.i * szerokosc_butona) + 100)+(( szerokosc_butona / temp_ilosc[z.i,z.j])*pozycja[z.i,z.j])), ((z.j * 20) + 40));
                    for (int j = 0; j < z.dlugosc; j++)
                    {
                        pozycja[z.i, z.j + j]--;
                    }  
                    if(z.j>0)
                        while(pozycja[z.i, z.j-k] == temp_poz)
                        {
                            if (pozycja[z.i, z.j - k] != temp_poz)
                                break;
                            pozycja[z.i, z.j - k]--;
                            if (z.j - k > 0) 
                                k++;
                            else
                                break;
                        }
                }
                l.Text = z.przedmiot.typ + " " + z.przedmiot.nazwa + " " + z.grupa.ToString();
                //l.Text += "\n" + z.przedmiot.nauczyciel.imie;
                l.Text += "\nSala : " + z.przedmiot.sala.numer.ToString();
                if (z.przedmiot.specjalizacja != null)
                    l.Text += "\nSpec : " + z.przedmiot.specjalizacja;
                //start
                Double zegar;
                zegar = (godzina * 60) + z.j * 15;
                if (zegar % 60 == 0)
                    l.Text += "\n" + Math.Floor(zegar / 60) + ":00";
                else
                    l.Text += "\n" + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                zegar += z.dlugosc * 15;
                if (zegar % 60 == 0)
                    l.Text += " - " + Math.Floor(zegar / 60) + ":00";
                else
                    l.Text += " - " + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                //koniec
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Visible = true;
                l.BackColor = kolor;
                this.Controls.Add(l);
            }
        }
    }
}
