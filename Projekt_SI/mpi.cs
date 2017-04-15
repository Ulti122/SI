using System;
using System.Drawing;
using System.Windows.Forms;

namespace Projekt_SI
{
    public partial class mpi : Form
    {
        public mpi()
        {
            InitializeComponent();
            //rysowanie obramowania
            int godzina = 8;//poczatkowa godzina do labeli
            ////
            int temp_godzina = godzina;
            Label ll = new Label();
            ll.TextAlign = ContentAlignment.MiddleCenter;
            ll.Width = 100;
            ll.Height = 40;
            ll.Text = ((Form1)Application.OpenForms[0]).kto_selected;
            ll.Location = new Point(0,0);
            this.Controls.Add(ll);
            for (int i = 0; i< 5;i++)
            {
                Label l = new Label();
                l.TextAlign = ContentAlignment.MiddleCenter;
                if (i == 0)
                    l.Text = "Poniedziałek";
                else if(i==1)
                    l.Text = "Wtorek";
                else if (i == 2)
                    l.Text = "Środa";
                else if (i == 3)
                    l.Text = "Czwartek";
                else if (i == 4)
                    l.Text = "Piątek";
                l.Height = 40;
                l.Width = 200;
                l.Location = new Point(100+i*200);
                this.Controls.Add(l);
            }
            for (int i = 0; i < 40+1; i++)
            {
                Label l = new Label();
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Location = new Point(0, 40 + i * 20);
                int tempo = (i % 4) * 15;
                if (tempo == 0 && i > 0)
                    temp_godzina++;
                if(tempo==0)
                    l.Text = temp_godzina + ":" + "00";
                else
                    l.Text = temp_godzina + ":" + tempo;
                this.Controls.Add(l);
                this.Height = (60+(40 + i * 20));
            }
            //real shit


            int kto;
            if(((Form1)Application.OpenForms[0]).klasa_selected_i == 0 && ((Form1)Application.OpenForms[0]).ewe_selected_i == 1)
            {
                kto = ((Form1)Application.OpenForms[0]).kto_selected_i;
                int temp = 0;
                Boolean old_i = false;
                for (int i = 0; i < 5; i++)
                {
                    if (old_i)
                    {
                        Label l = new Label();
                        l.Location = new Point(((i - 1) * 200) + 100, (((40 - temp) * 20) + 40));
                        l.Height = 20 * temp;
                        l.Width = 200;
                        l.Text = ((Form1)Application.OpenForms[0]).Wykladowcy[kto].imie;
                        if ((temp % 4) == 0)
                            l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":00";
                        else
                            l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":" + Math.Abs((temp % 4) - 4) * 15;

                        l.Text += "\n" + "18:00";
                        l.TextAlign = ContentAlignment.MiddleCenter;
                        this.Controls.Add(l);
                        old_i = false;
                        temp = 0;
                    }
                    for (int j = 0; j < 40; j++)
                    {
                        if (((Form1)Application.OpenForms[0]).Wykladowcy[kto].dostepnosc[i, j] == 1)
                        {
                            if (j == 39)
                                old_i = true;
                            temp++;
                        }
                        else if (temp > 0 && old_i == false)
                        {
                            Label l = new Label();
                            l.Location = new Point((i * 200) + 100, ((j * 20) + 40) - 20 * temp);
                            l.Height = 20 * temp;
                            l.Width = 200;
                            l.Text = ((Form1)Application.OpenForms[0]).Wykladowcy[kto].imie;
                            //start
                            if (((j - temp) % 4) == 0)
                                l.Text += "\n" + (Math.Floor((double)(j - temp) / 4) + godzina) + ":00";
                            else
                                l.Text += "\n" + (Math.Floor((double)(j - temp) / 4) + godzina) + ":" + ((j - temp) % 4) * 15;
                            //koniec
                            if ((temp % 4) == 0)
                                l.Text += "\n" + (Math.Floor((double)(j) / 4) + godzina) + ":00";
                            else
                                l.Text += "\n" + (Math.Floor((double)(j) / 4) + godzina) + ":" + (temp % 4) * 15;
                            l.TextAlign = ContentAlignment.MiddleCenter;
                            this.Controls.Add(l);
                            old_i = false;
                            temp = 0;
                        }
                    }
                }
                if (old_i)
                {
                    Label l = new Label();
                    l.Location = new Point(((5 - 1) * 200) + 100, (((40 - temp) * 20) + 40));
                    l.Height = 20 * temp;
                    l.Width = 200;
                    l.Text = ((Form1)Application.OpenForms[0]).Wykladowcy[kto].imie;
                    if ((temp % 4) == 0)
                        l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":00";
                    else
                        l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":" + Math.Abs((temp % 4) - 4) * 15;

                    l.Text += "\n" + "18:00";
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    this.Controls.Add(l);
                    old_i = false;
                    temp = 0;
                }
            }
            else if (((Form1)Application.OpenForms[0]).klasa_selected_i == 1)
            {
                kto = ((Form1)Application.OpenForms[0]).kto_selected_i;
                if (((Form1)Application.OpenForms[0]).Studenty[kto].specjalizacja != null)
                    ll.Text += "\n Specjalizacja : " + (((Form1)Application.OpenForms[0]).Studenty[kto].specjalizacja);
                int temp = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 40; j++)
                    {
                        if (((Form1)Application.OpenForms[0]).Studenty[kto].plan[i, j] != null)
                            temp = ((Form1)Application.OpenForms[0]).Studenty[kto].plan[i, j].dlugosc;
                        if (temp > 0)
                        {
                            Label l = new Label();
                            l.Location = new Point((i * 200) + 100, ((j * 20) + 40));
                            l.Height = 20 * temp;
                            l.Width = 200;
                            l.Text = ((Form1)Application.OpenForms[0]).Studenty[kto].plan[i, j].przedmiot.typ + "  " + ((Form1)Application.OpenForms[0]).Studenty[kto].plan[i, j].przedmiot.nazwa;
                            l.Text += "\n Sala : " + ((Form1)Application.OpenForms[0]).Studenty[kto].plan[i, j].przedmiot.sala.numer.ToString();
                            if (((Form1)Application.OpenForms[0]).Studenty[kto].plan[i,j].przedmiot.specjalizacja != null)
                                l.Text += "\n Specjalizacja : " + ((Form1)Application.OpenForms[0]).Studenty[kto].plan[i, j].przedmiot.specjalizacja;
                            //start
                            Double zegar;
                            zegar = (godzina * 60) + j * 15;
                            if (zegar % 60 == 0)
                                l.Text += "\n" + Math.Floor(zegar / 60) + ":00";
                            else
                                l.Text += "\n" + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                            zegar += temp * 15;
                            if (zegar % 60 == 0)
                                l.Text += " - " + Math.Floor(zegar / 60) + ":00";
                            else
                                l.Text += " - " + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                            //koniec
                            l.TextAlign = ContentAlignment.MiddleCenter;
                            this.Controls.Add(l);
                            temp = 0;
                        }
                    }
                }

            }
            else if (((Form1)Application.OpenForms[0]).klasa_selected_i == 2)
            {
                kto = ((Form1)Application.OpenForms[0]).kto_selected_i;
                int temp = 0;
                Boolean old_i = false;
                for (int i = 0; i < 5; i++)
                {
                    if (old_i)
                    {
                        Label l = new Label();
                        l.Location = new Point(((i - 1) * 200) + 100, (((40 - temp) * 20) + 40));
                        l.Height = 20 * temp;
                        l.Width = 200;
                        l.Text = ((Form1)Application.OpenForms[0]).Sale[kto].numer.ToString();
                        if ((temp % 4) == 0)
                            l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":00";
                        else
                            l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":" + Math.Abs((temp % 4) - 4) * 15;

                        l.Text += "\n" + "18:00";
                        l.TextAlign = ContentAlignment.MiddleCenter;
                        this.Controls.Add(l);
                        old_i = false;
                        temp = 0;
                    }
                    for (int j = 0; j < 40; j++)
                    {
                        if (((Form1)Application.OpenForms[0]).Sale[kto].dostepnosc_sali[i, j] == 1)
                        {
                            if (j == 39)
                                old_i = true;
                            temp++;
                        }
                        else if (temp > 0 && old_i == false)
                        {
                            Label l = new Label();
                            l.Location = new Point((i * 200) + 100, ((j * 20) + 40) - 20 * temp);
                            l.Height = 20 * temp;
                            l.Width = 200;
                            l.Text = ((Form1)Application.OpenForms[0]).Sale[kto].numer.ToString();
                            //start
                            if (((j - temp) % 4) == 0)
                                l.Text += "\n" + (Math.Floor((double)(j - temp) / 4) + godzina) + ":00";
                            else
                                l.Text += "\n" + (Math.Floor((double)(j - temp) / 4) + godzina) + ":" + ((j - temp) % 4) * 15;
                            //koniec
                            if ((temp % 4) == 0)
                                l.Text += "\n" + (Math.Floor((double)(j) / 4) + godzina) + ":00";
                            else
                                l.Text += "\n" + (Math.Floor((double)(j) / 4) + godzina) + ":" + (temp % 4) * 15;
                            l.TextAlign = ContentAlignment.MiddleCenter;
                            this.Controls.Add(l);
                            old_i = false;
                            temp = 0;
                        }
                    }
                }
                if (old_i)
                {
                    Label l = new Label();
                    l.Location = new Point(((5 - 1) * 200) + 100, (((40 - temp) * 20) + 40));
                    l.Height = 20 * temp;
                    l.Width = 200;
                    l.Text = ((Form1)Application.OpenForms[0]).Sale[kto].numer.ToString();
                    if ((temp % 4) == 0)
                        l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":00";
                    else
                        l.Text += "\n" + (Math.Floor((double)(40 - temp) / 4) + godzina) + ":" + Math.Abs((temp % 4) - 4) * 15;

                    l.Text += "\n" + "18:00";
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    this.Controls.Add(l);
                    old_i = false;
                    temp = 0;
                }
            }
            else if (((Form1)Application.OpenForms[0]).klasa_selected_i == 0 && ((Form1)Application.OpenForms[0]).ewe_selected_i == 0)
            {
                kto = ((Form1)Application.OpenForms[0]).kto_selected_i;
                int temp = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 40; j++)
                    {
                        if (((Form1)Application.OpenForms[0]).Wykladowcy[kto].plan[i, j] != null)
                            temp = ((Form1)Application.OpenForms[0]).Wykladowcy[kto].plan[i, j].dlugosc;
                        if (temp > 0)
                        {
                            Label l = new Label();
                            l.Location = new Point((i * 200) + 100, ((j * 20) + 40));
                            l.Height = 20 * temp;
                            l.Width = 200;
                            l.Text = ((Form1)Application.OpenForms[0]).Wykladowcy[kto].plan[i, j].przedmiot.typ + "  "+((Form1)Application.OpenForms[0]).Wykladowcy[kto].plan[i, j].przedmiot.nazwa;
                            //start
                            Double zegar;
                            zegar = (godzina * 60) + j*15;
                            if(zegar%60==0)
                                l.Text += "\n" + Math.Floor(zegar / 60) + ":00";
                            else
                                l.Text += "\n" + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                            zegar += temp*15;
                            if (zegar % 60 == 0)
                                l.Text += " - " + Math.Floor(zegar / 60) + ":00";
                            else
                                l.Text += " - " + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                            //koniec
                            l.TextAlign = ContentAlignment.MiddleCenter;
                            this.Controls.Add(l);
                            temp = 0;
                        }
                    }
                }
            }
        }
        public mpi(int plan_id)
        {
            InitializeComponent();
            int godzina = 8;//poczatkowa godzina do labeli
            ////
            int temp_godzina = godzina;
            Label ll = new Label();
            ll.TextAlign = ContentAlignment.MiddleCenter;
            ll.Width = 100;
            ll.Height = 40;
            ll.Text = ((Form1)Application.OpenForms[0]).kto_selected;
            ll.Location = new Point(0, 0);
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
                l.Width = 200;
                l.Location = new Point(100 + i * 200);
                this.Controls.Add(l);
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
            }
            //real shit 
            int kto = plan_id;
            ll.Text = "\n Semestr : " + ((Form1)Application.OpenForms[0]).Planki[kto].semestr;
            int temp = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    if (((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j] != null)
                        temp = ((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].dlugosc;
                    if (temp > 0)
                    {
                        Label l = new Label();
                        l.Location = new Point((i * 200) + 100, ((j * 20) + 40));
                        l.Height = 20 * temp;
                        l.Width = 200;
                        l.Text = ((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].przedmiot.typ + "  " + ((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].przedmiot.nazwa;
                        l.Text += "\n" + ((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].przedmiot.nauczyciel.imie;
                        l.Text += "\nSala : " + ((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].przedmiot.sala.numer.ToString();
                        if (((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].przedmiot.specjalizacja != null)
                            l.Text += "\nSpecjalizacja : " + ((Form1)Application.OpenForms[0]).Planki[kto].plan[i, j].przedmiot.specjalizacja;
                        //start
                        Double zegar;
                        zegar = (godzina * 60) + j * 15;
                        if (zegar % 60 == 0)
                            l.Text += "\n" + Math.Floor(zegar / 60) + ":00";
                        else
                            l.Text += "\n" + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                        zegar += temp * 15;
                        if (zegar % 60 == 0)
                            l.Text += " - " + Math.Floor(zegar / 60) + ":00";
                        else
                            l.Text += " - " + Math.Floor(zegar / 60) + ":" + (zegar % 60);
                        //koniec
                        l.TextAlign = ContentAlignment.MiddleCenter;
                        this.Controls.Add(l);
                        temp = 0;
                    }
                }
            }
        }
    }
}
