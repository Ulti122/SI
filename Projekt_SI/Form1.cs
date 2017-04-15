using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Projekt_SI
{
    public partial class Form1 : Form
    {
        private List<Wykladowca> wykladowcy = new List<Wykladowca>();
        private List<Sala> sala_wykladowa = new List<Sala>();
        private List<Przedmiot> przedmioty = new List<Przedmiot>();
        private List<Student> studenty = new List<Student>();
        private List<Zajecie> zajecia = new List<Zajecie>();
        private List<Plany> planki = new List<Plany>();

        public String kto_selected, ewe_selected;
        public int kto_selected_i,klasa_selected_i, ewe_selected_i;
        Random random = new Random();
        int tries=0;
        public Form1()
        {
            InitializeComponent();
            //wczytywanka
            wczytaj_dostepnosc_wykladowcow();
            wczytaj_sale();
            wczytaj_przedmioty();
            wczytaj_studentow();
            //generowanie zajec
            uloz_przedmioty();
            generuj_zajecia();
            //test label
            button1.Enabled = false;
            label1.Text = "Wykładowcy\n";
            for (int i = 0; i < Wykladowcy.Count; i++)
            {
                label1.Text += Wykladowcy[i].imie;
                label1.Text += "\n";
            }
            label3.Text = "Sale\n";
            for (int i = 0; i < Sale.Count; i++)
            {
                label3.Text += "Numer : "+Sale[i].numer+" typ : "+Sale[i].typ+" miejsc : "+Sale[i].miejsc;
                label3.Text += "\n";
            }
            label4.Text = "Przedmioty\n";
            for (int i = 0; i < Przedmioty.Count; i++)
            {
                label4.Text += "Nazwa : " + Przedmioty[i].nazwa + " typ : " + Przedmioty[i].typ + " prowadzący : " + Przedmioty[i].nauczyciel.imie + " spec : " + Przedmioty[i].specjalizacja;
                label4.Text += "\n";
            }
            label5.Text = "Studentów : "+Studenty.Count+"\n";
            for (int i = 0; i < Studenty.Count; i++)
            {
                label5.Text += "Imie : " + Studenty[i].imie + " spe : " + Studenty[i].specjalizacja;
                label5.Text += "\n";
            }
            label6.Text = "Zajęć : " + Zajecia.Count + "\n";
            for (int i = 0; i < Zajecia.Count; i++)
            {
                label6.Text += "["+i+"]" +"Nazwa : " + Zajecia[i].przedmiot.nazwa + " typ : " + Zajecia[i].przedmiot.typ + " spec : " + Zajecia[i].przedmiot.specjalizacja + " miejsc : " + Zajecia[i].miejsca+ " czas : " + Zajecia[i].dlugosc;
                label6.Text += "\n";
            }

        }

        internal List<Wykladowca> Wykladowcy
        {
            get
            {
                return wykladowcy;
            }

            set
            {
                wykladowcy = value;
            }
        }
        internal List<Sala> Sale
        {
            get
            {
                return sala_wykladowa;
            }

            set
            {
                sala_wykladowa = value;
            }
        }
        internal List<Przedmiot> Przedmioty
        {
            get
            {
                return przedmioty;
            }

            set
            {
                przedmioty = value;
            }
        }
        internal List<Student> Studenty
        {
            get
            {
                return studenty;
            }

            set
            {
                studenty = value;
            }
        }
        internal List<Zajecie> Zajecia
        {
            get
            {
                return zajecia;
            }

            set
            {
                zajecia = value;
            }
        }
        internal List<Plany> Planki
        {
            get
            {
                return planki;
            }

            set
            {
                planki = value;
            }
        }

        public void wczytaj_dostepnosc_wykladowcow()
        {
            var fs = File.OpenRead(@"dostepnosc_plebsu.csv");
            var reader = new StreamReader(fs);
            String temp_name = null;
            int[,] temp = new int[5,40];
            int i = 0;
            int ii = 0;
            int ilosc = 0;
            while (!reader.EndOfStream)
            {
                if (i == 0)
                    temp_name = reader.ReadLine();
                else if(i<201)
                {
                   // temp_name = reader.ReadLine();
                    temp[ii,ilosc++] = Convert.ToInt16(reader.ReadLine());
                }
                if (i % 40 == 0 && i!=0)
                {
                    ilosc = 0;
                    ii++;
                }
                if (i == 201)
                {
                    ilosc = 0;
                    ii = 0;
                    i = -1;
                    Wykladowca wykl = new Wykladowca(temp_name, temp);
                    Wykladowcy.Add(wykl);
                    wykl = null;
                }
                i++;
            }
            fs.Close();
        }
        public void wczytaj_sale()
        {
            var fs = File.OpenRead(@"sale.csv");
            var reader = new StreamReader(fs);
            int temp_numer, temp_miejsc;
            String temp_typ = null;
            while (!reader.EndOfStream)
            {
                temp_typ = reader.ReadLine();
                temp_numer = Convert.ToInt16(reader.ReadLine());
                temp_miejsc = Convert.ToInt16(reader.ReadLine());
                Sala s = new Sala(temp_numer, temp_miejsc,temp_typ);
                Sale.Add(s);
                s = null;
            }
            fs.Close();
        }
        public void wczytaj_przedmioty()
        {
            var fs = File.OpenRead(@"przedmiot.csv");
            var reader = new StreamReader(fs);
            int temp_semestr, temp_miejsc , temp_sala;
            String temp_nazwa = null;
            String temp_wykladowca = null;
            String temp_spec = null;
            String[] temp_specs = null;
            String temp_typ = null;
            Double temp_ilosc_h = 0.0;
            Boolean multi_spec = false;
            while (!reader.EndOfStream)
            {
                temp_nazwa = reader.ReadLine();
                temp_nazwa = reader.ReadLine();
                temp_typ = reader.ReadLine();
                temp_wykladowca = reader.ReadLine();
                temp_ilosc_h = Convert.ToDouble(reader.ReadLine());
                temp_semestr = Convert.ToInt16(reader.ReadLine());
                temp_spec = reader.ReadLine();
                if (temp_spec == "null")
                    temp_spec = null;
                else if (temp_spec.Contains(','))
                    multi_spec = true;
                else
                    temp_specs = temp_spec.Split(';');
                temp_sala = Convert.ToInt16(reader.ReadLine());
                temp_miejsc = Convert.ToInt16(reader.ReadLine());
                if(temp_specs!=null&&multi_spec==false)
                {
                    foreach(String ss in temp_specs)
                    {
                        Przedmiot p = new Przedmiot(temp_nazwa, temp_typ, temp_ilosc_h, temp_semestr, ss, temp_miejsc);
                        foreach (Sala s in Sale)
                            if (temp_sala == s.numer)
                                p.sala = s;
                        foreach (Wykladowca w in Wykladowcy)
                            if (w.imie == temp_wykladowca)
                                p.nauczyciel = w;
                        p.multi_spec = false;
                        Przedmioty.Add(p);
                        p = null;
                    }
                }
                else if(multi_spec)
                {
                    Przedmiot p = new Przedmiot(temp_nazwa, temp_typ, temp_ilosc_h, temp_semestr, temp_spec, temp_miejsc);
                    foreach (Sala s in Sale)
                        if (temp_sala == s.numer)
                            p.sala = s;
                    foreach (Wykladowca w in Wykladowcy)
                        if (w.imie == temp_wykladowca)
                            p.nauczyciel = w;
                    p.multi_spec = true;
                    Przedmioty.Add(p);
                    p = null;
                }
                else
                {
                    Przedmiot p = new Przedmiot(temp_nazwa, temp_typ, temp_ilosc_h, temp_semestr, temp_spec, temp_miejsc);
                    foreach (Sala s in Sale)
                        if (temp_sala == s.numer)
                            p.sala = s;
                    foreach (Wykladowca w in Wykladowcy)
                        if (w.imie == temp_wykladowca)
                            p.nauczyciel = w;
                    p.multi_spec = false;
                    Przedmioty.Add(p);
                    p = null;
                }
                multi_spec = false;
                temp_specs = null;
            }
            fs.Close();
        }
        public void wczytaj_studentow()
        {
            var fs = File.OpenRead(@"studenci.csv");
            var reader = new StreamReader(fs);
            int temp_semestr;
            String temp_nazwa = null;
            String temp_spec = null;
            while (!reader.EndOfStream)
            {
                temp_nazwa = reader.ReadLine();
                temp_semestr = Convert.ToInt16(reader.ReadLine());
                temp_spec = reader.ReadLine();
                Student s = new Student(temp_nazwa, temp_semestr, temp_spec);
                Studenty.Add(s);
                s = null;
            }
            fs.Close();
        }
        public void generuj_zajecia()
        {
            for(int i=0;i<Przedmioty.Count;i++)
            {
                int ilosc_spelniajaca_kryterium = 0;
                foreach (Student s in Studenty)
                    if (Przedmioty[i].semestr == s.semestr)
                        if (Przedmioty[i].specjalizacja == null)
                            ilosc_spelniajaca_kryterium++;
                        else if(Przedmioty[i].specjalizacja == s.specjalizacja && Przedmioty[i].multi_spec==false)
                            ilosc_spelniajaca_kryterium++;
                        else if (Przedmioty[i].multi_spec == true)
                        {
                            String[] tempo = Przedmioty[i].specjalizacja.Split(',');
                            foreach (String ss in tempo)
                                if(s.specjalizacja==ss)
                                    ilosc_spelniajaca_kryterium++;
                        }
                            ilosc_spelniajaca_kryterium++;
                Double temp = ilosc_spelniajaca_kryterium;
                int ilosc_grup = (int)Math.Ceiling(temp / Przedmioty[i].ilosc_w_grupie);
                for(int j = 0; j<ilosc_grup;j++)
                {
                    int miejsca;
                    if (j == ilosc_grup - 1)
                        miejsca = ilosc_spelniajaca_kryterium % Przedmioty[i].ilosc_w_grupie;
                    else
                        miejsca = Przedmioty[i].ilosc_w_grupie;

                    Zajecie z = new Zajecie(Przedmioty[i],miejsca);
                    z.grupa = j + 1;
                    Zajecia.Add(z);
                    z = null;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new mpi();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tries = 0;
           // int semestr = 6;
            to_nie_zadziala_v5();
            label2.Text = "Porzuconych iteracji : " + tries.ToString();
        }

        private void klasacomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Convert.ToString(klasacomboBox.SelectedItem)=="Wykładowca")
            {
                ktocomboBox.Items.Clear();
                for(int i=0;i<Wykladowcy.Count;i++)
                    ktocomboBox.Items.Add("[" + i + "] " + Wykladowcy[i].imie);
                ewentualnosccomboBox.Items.Clear();
                ewentualnosccomboBox.Items.Add("Plan");
                ewentualnosccomboBox.Items.Add("Dostępność");
            }
            if (Convert.ToString(klasacomboBox.SelectedItem) == "Student")
            {
                ktocomboBox.Items.Clear();
                for (int i = 0; i < Studenty.Count; i++)
                    ktocomboBox.Items.Add("[" + i + "] " + Studenty[i].imie);
                ewentualnosccomboBox.Items.Clear();
                ewentualnosccomboBox.Items.Add("Plan");
            }
            if (Convert.ToString(klasacomboBox.SelectedItem) == "Klasa")
            {
                ktocomboBox.Items.Clear();
                for (int i = 0; i < Sale.Count; i++)
                    ktocomboBox.Items.Add("[" + i + "] " + Sale[i].numer);
                ewentualnosccomboBox.Items.Clear();
                ewentualnosccomboBox.Items.Add("Dostępność");
            }
            klasa_selected_i = klasacomboBox.SelectedIndex;
            ewentualnosccomboBox.ResetText();
            ktocomboBox.ResetText();
        }

        private void ktocomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            kto_selected = ktocomboBox.SelectedItem.ToString();
            kto_selected_i = ktocomboBox.SelectedIndex;
            button1.Enabled = true;
        }
        private void ewentualnosccomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ewe_selected = ewentualnosccomboBox.SelectedItem.ToString();
            ewe_selected_i = ewentualnosccomboBox.SelectedIndex;
            button1.Enabled = true;
        }
        //Algorytm v1
        // losowania-- sal - wpisane na sztywno w pliku przedmioty ( odnosnik do sali Sale ofc)
        //w porownaniu do v2 to jakis zart
        //NIE RADZI SOBIE Z MALA DOSTEPNOSCIA -> najmniej dropi
        public void to_nie_zadziala()
        {
            int semestr = 6;//TODO ZROBIC PARAMETR
            int limit = Convert.ToInt32(textBox.Text);
            int z = 0;
            int najdluzsze_zajecia=-1;
            foreach (Zajecie zz in zajecia)
                if (zz.dlugosc > najdluzsze_zajecia)
                    najdluzsze_zajecia = zz.dlugosc;
            for(int counter=0;counter<limit;counter++)
            {
                //List<Wykladowca> temp_wykladowcy = new List<Wykladowca>(Wykladowcy);
                //List<Sala> temp_sala_wykladowa = new List<Sala>(Sale);
               // List<Zajecie> temp_zajecia = new List<Zajecie>(Zajecia);

                Plany p = new Plany();//TODO normalne wywolanie metody z parametrami
                //p.sale = Sale.ToList();
                foreach(Sala s in Sale)
                {
                    Sala ss = new Sala(s.numer, s.miejsc, s.typ);
                    p.sale.Add(ss);
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 40; j++)
                            p.sale[p.sale.IndexOf(ss)].dostepnosc_sali[i,j] = s.dostepnosc_sali[i, j];
                }
                p.semestr = 6;
                //p.specjalnosc = null;

                int proby = 100000;
                for (int i = 0; i < Zajecia.Count; i++)
                {
                    proby--;
                    if (proby < 0)
                    {
                        i = Zajecia.Count;
                    }
                    else
                    {
                        int rand_czas_i = random.Next(0, 5);
                        int rand_czas_j = random.Next(0, 40);
                        Boolean wykladowca_dostepny = false;
                        Boolean wykladowca_zajety = false;
                        Boolean inne_zajecia = false;

                        int wykladowca_id = -1;//wrazie czego sypniemy erora
                                               ///
                        if (Zajecia[i].przedmiot.semestr == semestr)
                        {
                            Zajecie temp_zajecie = new Zajecie(Zajecia[i].przedmiot, Zajecia[i].miejsca);
                            if (39 - rand_czas_j < Zajecia[i].dlugosc)
                            {
                                p.wykladowcy.RemoveRange(0, p.wykladowcy.Count);
                                i--;
                            }
                            else
                            {
                                foreach (Wykladowca w in Wykladowcy)//szukamy zipka co nam zajecia prowadzi
                                {
                                    if (Zajecia[i].przedmiot.nauczyciel == w)//      VV czy jest i czy jeszcze bedzie
                                        if (w.dostepnosc[rand_czas_i, rand_czas_j] == 1 && w.dostepnosc[rand_czas_i, rand_czas_j + Zajecia[i].dlugosc] == 1)
                                        {
                                            wykladowca_dostepny = true;
                                            Wykladowca ww = new Wykladowca(w.imie, w.dostepnosc);
                                            p.wykladowcy.Add(ww);
                                            wykladowca_id = p.wykladowcy.IndexOf(ww);
                                        }
                                }
                                if (wykladowca_id < 0 && wykladowca_dostepny == false)
                                {
                                    p.wykladowcy.RemoveRange(0, p.wykladowcy.Count);
                                    i--;
                                }
                                else
                                {
                                    if (wykladowca_dostepny)//taki myk zeby uniknac jechania dalej w foreachu // optymalizacja FTW(j/k)
                                        if (rand_czas_j <= najdluzsze_zajecia)
                                        {
                                            for (int j = -rand_czas_j; j < Zajecia[i].dlugosc+1; j++)//jeden przed bo przerwa i dlugosc po bo czy nie wchodzi mu w inne zajecia 
                                            {
                                                if ((wykladowca_zajety != true || inne_zajecia != true))
                                                {
                                                    if (p.wykladowcy[wykladowca_id].plan[rand_czas_i, rand_czas_j + j] != null)
                                                        wykladowca_zajety = true;
                                                    if (p.plan[rand_czas_i, rand_czas_j + j] != null)
                                                        inne_zajecia = true;
                                                }
                                                else
                                                    j = Zajecia[i].dlugosc;
                                            }
                                        }
                                        else
                                        {
                                            for (int j = -(najdluzsze_zajecia + 1); j < Zajecia[i].dlugosc+1; j++)//jeden przed bo przerwa i dlugosc po bo czy nie wchodzi mu w inne zajecia 
                                            {
                                                if ((wykladowca_zajety != true || inne_zajecia != true))
                                                {
                                                    if (p.wykladowcy[wykladowca_id].plan[rand_czas_i, rand_czas_j + j] != null)
                                                        wykladowca_zajety = true;
                                                    if (p.plan[rand_czas_i, rand_czas_j + j] != null)
                                                        inne_zajecia = true;
                                                }
                                                else
                                                    j = Zajecia[i].dlugosc;
                                            }
                                        }
                                    if (wykladowca_zajety == true || inne_zajecia == true)//CO TO KURWA JEST - OUT - cofamy petle niech zlosuje ten przedmiot jeszcze raz w normalym okienku :)
                                    {
                                        p.wykladowcy.RemoveRange(0, p.wykladowcy.Count);
                                        i--;
                                    }
                                    else
                                    {
                                        //int rand_sali;
                                        Boolean zajeta = false;
                                        //do
                                        //{
                                        //    rand_sali = random.Next(0, Sale.Count);
                                        //    if (Sale[rand_sali].typ == Zajecia[i].przedmiot.typ)//losowanie wolnej sali o typie zajec, sala nie musi miec przerwy XD
                                        //        if (p.sale[rand_sali].dostepnosc_sali[rand_czas_i, rand_czas_j] == 0 && p.sale[rand_sali].dostepnosc_sali[rand_czas_i, rand_czas_j + Zajecia[i].dlugosc] == 0)
                                        //            zajeta = false;
                                        //} while (zajeta);

                                        for(int j = 0;j<Zajecia[i].dlugosc;j++)
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta = true;
                                        if(zajeta==false)
                                        {
                                            p.plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                            for (int j = 0; j < Zajecia[i].dlugosc; j++)
                                            {
                                                p.sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] = 1;//ustawianie ze sala jest zajeta 
                                                p.plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                            }

                                            p.wykladowcy[wykladowca_id].plan[rand_czas_i, rand_czas_j] = temp_zajecie;//ustawiamy zipkowi w planie ze cos robi
                                        }
                                        else
                                        {
                                            p.wykladowcy.RemoveRange(0, p.wykladowcy.Count);
                                            i--;
                                        }
                                       // temp_zajecie.sala = p.sale[rand_sali];
                                        
                                    }
                                }
                            }

                        }
                    }
                }
                    
                if (proby > 0)
                {
                    //label2.Text = proby.ToString();
                    proby = 100000;
                    Planki.Add(p);
                    ocen_to(Planki.IndexOf(p));
                    p = null;
                }
                else
                {
                    z++;
                    label2.Text = "Porzuconych iteracji : "+z.ToString();
                    p = null;
                }
                    
            }
            wgraj_ten_szajs(semestr);
        }
        //Algorytm v2 aka wcale nie taka losowa losowosc Kappa
        //Teoretycznie im mniejsza dostepnosc wykladowcy tym szybszy
        public void to_nie_zadziala_v2()
        {
            int semestr = 6;//TODO SELECT
            int limit = Convert.ToInt32(textBox.Text);
            int rand_czas_i;
            int rand_czas_j;
            int rand_czas_i_licznik_p = 0;
            int rand_czas_i_licznik_f = 0;
            int proby = 1000;
            int numer_wykladowcy;
            Boolean wykladowca_dostepny = false;
            Boolean wykladowca_ma_inne_zajecia = false;
            Boolean inne_zajecia = false;
            Boolean zajeta_sala = false;
            for (int qwert=0;qwert<limit;qwert++)
            {
                Plany p = new Plany();
                foreach (Sala s in Sale)
                {
                    Sala ss = new Sala(s.numer, s.miejsc, s.typ);
                    p.sale.Add(ss);
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 40; j++)
                            p.sale[p.sale.IndexOf(ss)].dostepnosc_sali[i, j] = s.dostepnosc_sali[i, j];
                }
                p.semestr = semestr;
                foreach (Wykladowca w in Wykladowcy)
                {
                    Wykladowca ww = new Wykladowca(w.imie, w.dostepnosc);
                    p.wykladowcy.Add(ww);
                }
                for(int i = 0; i<Zajecia.Count;i++)
                {
                    if (--proby > 0)
                    {
                        if (Zajecia[i].przedmiot.semestr == p.semestr)
                        {
                            rand_czas_i = random.Next(0, Zajecia[i].przedmiot.nauczyciel.dni);
                            rand_czas_i_licznik_p = 0;
                            rand_czas_i_licznik_f = 0;
                            foreach(Boolean b in Zajecia[i].przedmiot.nauczyciel.dostep_dzien)
                                if(b==true)
                                {
                                    if (rand_czas_i_licznik_p == rand_czas_i)
                                        rand_czas_i = rand_czas_i_licznik_p + rand_czas_i_licznik_f;
                                    else
                                        rand_czas_i_licznik_p++;
                                }
                                    else
                                        rand_czas_i_licznik_f++;
                            rand_czas_j = random.Next(0, 40 - Zajecia[i].dlugosc);

                            if (Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j] == 1 && Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j + Zajecia[i].dlugosc] == 1)
                                wykladowca_dostepny = true;
                            if (wykladowca_dostepny)
                            {
                                Zajecie temp_zajecie = new Zajecie(Zajecia[i].przedmiot, Zajecia[i].miejsca);
                                numer_wykladowcy = Wykladowcy.IndexOf(Zajecia[i].przedmiot.nauczyciel);
                                wykladowca_ma_inne_zajecia = false;
                                inne_zajecia = false;
                                zajeta_sala = false;
                                if (rand_czas_j == 0)
                                {
                                    for (int j = 0; j < Zajecia[i].dlugosc+1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (p.plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                else
                                {
                                    for (int j = -1; j < Zajecia[i].dlugosc+1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (p.plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                {
                                    p.plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                    p.wykladowcy[numer_wykladowcy].plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                    for (int j = 0; j < Zajecia[i].dlugosc; j++)
                                    {
                                        p.sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] = 1;
                                        p.plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                        p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                    }
                                }
                                else
                                    i--;
                            }
                            else
                                i--;
                        }
                    }
                    else
                        i = Zajecia.Count;
                }
                if (proby > 0)
                {
                    proby = 1000;
                    Planki.Add(p);
                    //ocen_je(p);//Nowa metoda dla multi
                    ocen_to(Planki.IndexOf(p));
                    p = null;
                }
                else
                {
                    proby = 1000;
                    tries++;
                    p = null;
                }
            }
            wgraj_ten_szajs(semestr);
        }
        //Algorytm v3 coraz mniej losowej losowsci - paczki zajec inkoming !
        //DUZE TORBULE DROPI ITERACJE
        public void to_nie_zadziala_v3()
        {
            int semestr = 6;//TODO SELECT
            int limit = Convert.ToInt32(textBox.Text);
            int rand_czas_i;
            int rand_czas_j;
            int rand_czas_i_licznik_p = 0;
            int rand_czas_i_licznik_f = 0;
            int proby = 1000;
            int numer_wykladowcy;
            int ostatnie_dzien = 0;
            int ostatnie_godzina = 0;
            Boolean ostatnia_proba = true;
            Boolean wykladowca_dostepny = false;
            Boolean wykladowca_ma_inne_zajecia = false;
            Boolean inne_zajecia = false;
            Boolean zajeta_sala = false;
            Boolean sky_is_the_limit = false;
            for (int qwert = 0; qwert < limit; qwert++)
            {
                ostatnie_dzien = 0;
                ostatnie_godzina = 0;
                Plany p = new Plany();
                foreach (Sala s in Sale)
                {
                    Sala ss = new Sala(s.numer, s.miejsc, s.typ);
                    p.sale.Add(ss);
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 40; j++)
                            p.sale[p.sale.IndexOf(ss)].dostepnosc_sali[i, j] = s.dostepnosc_sali[i, j];
                }
                p.semestr = semestr;
                foreach (Wykladowca w in Wykladowcy)
                {
                    Wykladowca ww = new Wykladowca(w.imie, w.dostepnosc);
                    p.wykladowcy.Add(ww);
                }
                for (int i = 0; i < Zajecia.Count; i++)
                {
                    if (--proby > 0)
                    {
                        if (Zajecia[i].przedmiot.semestr == p.semestr)
                        {
                            Zajecie poprzednie_zajecie = null;
                            if (i != 0)
                                poprzednie_zajecie = Zajecia[i - 1];
                            rand_czas_i = random.Next(0, Zajecia[i].przedmiot.nauczyciel.dni);
                            rand_czas_i_licznik_p = 0;
                            rand_czas_i_licznik_f = 0;
                            if (poprzednie_zajecie == null || poprzednie_zajecie.przedmiot.nazwa != Zajecia[i].przedmiot.nazwa ||ostatnia_proba==false)
                            {
                                foreach (Boolean b in Zajecia[i].przedmiot.nauczyciel.dostep_dzien)
                                    if (b == true)
                                    {
                                        if (rand_czas_i_licznik_p == rand_czas_i)
                                            rand_czas_i = rand_czas_i_licznik_p + rand_czas_i_licznik_f;
                                        else
                                            rand_czas_i_licznik_p++;
                                    }
                                    else
                                        rand_czas_i_licznik_f++;
                            }
                            else
                            {
                                rand_czas_i = ostatnie_dzien;
                            }
                            if (ostatnie_godzina + Zajecia[i].dlugosc + 1 > 39 - Zajecia[i].dlugosc)
                                sky_is_the_limit = true;
                            if(poprzednie_zajecie==null || poprzednie_zajecie.przedmiot.nazwa!=Zajecia[i].przedmiot.nazwa || sky_is_the_limit==true)
                                rand_czas_j = random.Next(0, 40 - Zajecia[i].dlugosc);
                            else
                            {
                                rand_czas_j = ostatnie_godzina + Zajecia[i].dlugosc + 1;
                            }

                            if (Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j] == 1 && Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j + Zajecia[i].dlugosc] == 1)
                                wykladowca_dostepny = true;
                            if (wykladowca_dostepny)
                            {
                                Zajecie temp_zajecie = new Zajecie(Zajecia[i].przedmiot, Zajecia[i].miejsca);
                                numer_wykladowcy = Wykladowcy.IndexOf(Zajecia[i].przedmiot.nauczyciel);
                                wykladowca_ma_inne_zajecia = false;
                                inne_zajecia = false;
                                zajeta_sala = false;
                                if (rand_czas_j == 0)
                                {
                                    for (int j = 0; j < Zajecia[i].dlugosc + 1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (p.plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                else
                                {
                                    for (int j = -1; j < Zajecia[i].dlugosc + 1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (p.plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                {
                                    p.plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                    p.wykladowcy[numer_wykladowcy].plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                    for (int j = 0; j < Zajecia[i].dlugosc; j++)
                                    {
                                        p.sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] = 1;
                                        p.plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                        p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                    }
                                    ostatnie_dzien = rand_czas_i;
                                    ostatnie_godzina = rand_czas_j;
                                    ostatnia_proba=true;
                                }
                                else
                                {
                                    i--;
                                    ostatnia_proba = false;
                                }   
                            }
                            else
                            {
                                i--;
                                ostatnia_proba = false;
                            }     
                        }
                    }
                    else
                        i = Zajecia.Count;
                }
                if (proby > 0)
                {
                    proby = 1000;
                    Planki.Add(p);
                    ocen_to(Planki.IndexOf(p));
                    p = null;
                }
                else
                {
                    proby = 1000;
                    tries++;
                    p = null;
                }
            }
            //label2.Text = "Porzuconych iteracji : " + z.ToString();
            wgraj_ten_szajs(semestr);
        }
        //Algorytm v2 + mt test
        //DUZE TORBULE DROPI ITERACJE
        public void to_nie_zadziala_v4()
        {
            int semestr = 6;//TODO SELECT
            int limit = Convert.ToInt32(textBox.Text);
            int rand_czas_i;
            int rand_czas_j;
            int rand_czas_i_licznik_p = 0;
            int rand_czas_i_licznik_f = 0;
            int proby = 1000;
            int numer_wykladowcy;
            int watkow = Convert.ToInt32(watki_textBox.Text);
            Boolean wykladowca_dostepny = false;
            Boolean wykladowca_ma_inne_zajecia = false;
            Boolean inne_zajecia = false;
            Boolean zajeta_sala = false;
            for (int qwert = 0; qwert < (limit / watkow); qwert++)
            {
                Plany p = new Plany();
                foreach (Sala s in Sale)
                {
                    Sala ss = new Sala(s.numer, s.miejsc, s.typ);
                    p.sale.Add(ss);
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 40; j++)
                            p.sale[p.sale.IndexOf(ss)].dostepnosc_sali[i, j] = s.dostepnosc_sali[i, j];
                }
                p.semestr = semestr;
                foreach (Wykladowca w in Wykladowcy)
                {
                    Wykladowca ww = new Wykladowca(w.imie, w.dostepnosc);
                    p.wykladowcy.Add(ww);
                }
                for (int i = 0; i < Zajecia.Count; i++)
                {
                    if (--proby > 0)
                    {
                        if (Zajecia[i].przedmiot.semestr == p.semestr)
                        {
                            rand_czas_i = random.Next(0, Zajecia[i].przedmiot.nauczyciel.dni);
                            rand_czas_i_licznik_p = 0;
                            rand_czas_i_licznik_f = 0;
                            foreach (Boolean b in Zajecia[i].przedmiot.nauczyciel.dostep_dzien)
                                if (b == true)
                                {
                                    if (rand_czas_i_licznik_p == rand_czas_i)
                                        rand_czas_i = rand_czas_i_licznik_p + rand_czas_i_licznik_f;
                                    else
                                        rand_czas_i_licznik_p++;
                                }
                                else
                                    rand_czas_i_licznik_f++;
                            rand_czas_j = random.Next(0, 40 - Zajecia[i].dlugosc);

                            if (Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j] == 1 && Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j + Zajecia[i].dlugosc] == 1)
                                wykladowca_dostepny = true;
                            if (wykladowca_dostepny)
                            {
                                Zajecie temp_zajecie = new Zajecie(Zajecia[i].przedmiot, Zajecia[i].miejsca);
                                numer_wykladowcy = Wykladowcy.IndexOf(Zajecia[i].przedmiot.nauczyciel);
                                wykladowca_ma_inne_zajecia = false;
                                inne_zajecia = false;
                                zajeta_sala = false;
                                if (rand_czas_j == 0)
                                {
                                    for (int j = 0; j < Zajecia[i].dlugosc + 1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (p.plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                else
                                {
                                    for (int j = -1; j < Zajecia[i].dlugosc + 1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (p.plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                if (wykladowca_ma_inne_zajecia == false && inne_zajecia == false && zajeta_sala == false)
                                {
                                    p.plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                    p.wykladowcy[numer_wykladowcy].plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                    for (int j = 0; j < Zajecia[i].dlugosc; j++)
                                    {
                                        p.sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] = 1;
                                        p.plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                        p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                    }
                                }
                                else
                                    i--;
                            }
                            else
                                i--;
                        }
                    }
                    else
                        i = Zajecia.Count;
                }
                if (proby > 0)
                {
                    proby = 1000;
                    Planki.Add(p);
                    lock(this)
                        ocen_je();//Nowa metoda dla multi
                    //ocen_to(Planki.IndexOf(p));
                    p = null;
                }
                else
                {
                    //Thread.Sleep(1);
                    proby = 1000;
                    tries++;
                    p = null;
                }
            }
        }
        //To juz nie moze zadzialac, nawet trojka nie dziala jeszcze Keppo
        //DROPI ITERACJE -> memory leak
        //Aktualny algorytm w srodku : v4 (aka v2v2)
        public void to_nie_zadziala_wiele()
        {
            int watkow = Convert.ToInt32(watki_textBox.Text);
            Thread[] watki = new Thread[watkow];
            for(int i=0; i< watkow; i++)
                watki[i] = new Thread(new ThreadStart(to_nie_zadziala_v4));
            foreach (Thread t in watki)
                t.Start();
            foreach (Thread t in watki)
                t.Join();
            wgraj_ten_szajs(6);
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        //V5 losowanie z v2, losowanie v3 wedlug mnie podbawi mozliwych dobrych rozwiazan
        //z uwagi na losowanie kolejnoscia przedmiotow z danej specki - pelna losowosc da wiekszy zakres wynikow
        //nowa struktura planu - umozliwenie wystepowania kilku zajec w jednym okienku czasowym
        //5;40 planu daje uchwyt tylko do jednego zajecia -> to wymaga innego podejscia czym jest v5
        //modyfikacji pewnie trzeba bedzie poddac mpi i ocenianie
        public void to_nie_zadziala_v5()
        {
            int semestr = 6;//TODO SELECT
            int limit = Convert.ToInt32(textBox.Text);
            int rand_czas_i;
            int rand_czas_j;
            int rand_czas_i_licznik_p = 0;
            int rand_czas_i_licznik_f = 0;
            int proby = 1000;
            int numer_wykladowcy;
            Boolean wykladowca_dostepny = false;
            Boolean wykladowca_ma_inne_zajecia = false;
            Boolean shall_pass = false;
            Boolean zajeta_sala = false;
            for (int qwert = 0; qwert < limit; qwert++)
            {
                Plany p = new Plany();
                foreach (Sala s in Sale)
                {
                    Sala ss = new Sala(s.numer, s.miejsc, s.typ);
                    p.sale.Add(ss);
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 40; j++)
                            p.sale[p.sale.IndexOf(ss)].dostepnosc_sali[i, j] = s.dostepnosc_sali[i, j];
                }
                p.semestr = semestr;
                foreach (Wykladowca w in Wykladowcy)
                {
                    Wykladowca ww = new Wykladowca(w.imie, w.dostepnosc);
                    p.wykladowcy.Add(ww);
                }
                for (int i = 0; i < Zajecia.Count; i++)
                {
                    if (--proby > 0)
                    {
                        if (Zajecia[i].przedmiot.semestr == p.semestr)
                        {
                            rand_czas_i = random.Next(0, Zajecia[i].przedmiot.nauczyciel.dni);
                            rand_czas_i_licznik_p = 0;
                            rand_czas_i_licznik_f = 0;
                            foreach (Boolean b in Zajecia[i].przedmiot.nauczyciel.dostep_dzien)
                                if (b == true)
                                {
                                    if (rand_czas_i_licznik_p == rand_czas_i)
                                        rand_czas_i = rand_czas_i_licznik_p + rand_czas_i_licznik_f;
                                    else
                                        rand_czas_i_licznik_p++;
                                }
                                else
                                    rand_czas_i_licznik_f++;
                            rand_czas_j = random.Next(0, 40 - Zajecia[i].dlugosc);

                            if (Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j] == 1 && Zajecia[i].przedmiot.nauczyciel.dostepnosc[rand_czas_i, rand_czas_j + Zajecia[i].dlugosc] == 1)
                                wykladowca_dostepny = true;
                            if (wykladowca_dostepny)
                            {
                                Zajecie temp_zajecie = new Zajecie(Zajecia[i].przedmiot, Zajecia[i].miejsca);
                                temp_zajecie.grupa = Zajecia[i].grupa;
                                temp_zajecie.i = rand_czas_i;
                                temp_zajecie.j = rand_czas_j;
                                numer_wykladowcy = Wykladowcy.IndexOf(Zajecia[i].przedmiot.nauczyciel);
                                wykladowca_ma_inne_zajecia = false;
                                zajeta_sala = false;
                                if (rand_czas_j == 0)
                                {
                                    for (int j = 0; j < Zajecia[i].dlugosc + 1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                else
                                {
                                    for (int j = -1; j < Zajecia[i].dlugosc + 1; j++)
                                    {
                                        if (wykladowca_ma_inne_zajecia == false && zajeta_sala == false)
                                        {
                                            if (p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] == 1)
                                                wykladowca_ma_inne_zajecia = true;
                                            if (Sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] != 0)
                                                zajeta_sala = true;
                                        }
                                        else
                                            j = Zajecia[i].dlugosc;
                                    }
                                }
                                shall_pass = true;
                                if (zajeta_sala == false && wykladowca_ma_inne_zajecia == false)
                                {
                                    foreach (Zajecie z in p.plann)
                                    {
                                        if (z.i == rand_czas_i)
                                        {
                                            if (Enumerable.Range(z.j - 1, z.dlugosc + 1).Contains(rand_czas_j) || Enumerable.Range(z.j, z.dlugosc).Contains(rand_czas_j + z.dlugosc))
                                            {
                                                if (z.przedmiot.specjalizacja == null || temp_zajecie.przedmiot.specjalizacja == null) 
                                                    shall_pass = false;
                                                else if (z.przedmiot.multi_spec == false && temp_zajecie.przedmiot.multi_spec == false)
                                                {
                                                    if (z.przedmiot.specjalizacja == temp_zajecie.przedmiot.specjalizacja)
                                                        shall_pass = false;     
                                                }
                                                else if (z.przedmiot.multi_spec == false && temp_zajecie.przedmiot.multi_spec == true)
                                                {
                                                    String[] temp = temp_zajecie.przedmiot.specjalizacja.Split(',');
                                                    foreach (String ss in temp)
                                                    {
                                                        if (z.przedmiot.specjalizacja == ss)
                                                        {
                                                            shall_pass = false;
                                                        }
                                                    }
                                                }
                                                else if (z.przedmiot.multi_spec == true && temp_zajecie.przedmiot.multi_spec == false)
                                                {
                                                    String[] temp = z.przedmiot.specjalizacja.Split(',');
                                                    foreach (String ss in temp)
                                                    {
                                                        if (temp_zajecie.przedmiot.specjalizacja == ss)
                                                        {
                                                            shall_pass = false;
                                                        }
                                                    }
                                                }
                                                else if (z.przedmiot.multi_spec == true && temp_zajecie.przedmiot.multi_spec == true)
                                                {
                                                    String[] temp = z.przedmiot.specjalizacja.Split(',');
                                                    String[] tempo = temp_zajecie.przedmiot.specjalizacja.Split(',');
                                                    foreach (String ss in temp)
                                                        foreach(String sss in tempo)
                                                            if (sss == ss)
                                                                shall_pass = false;
                                                }
                                            }
                                        }
                                    }
                                    if (shall_pass)
                                    {
                                        p.plann.Add(temp_zajecie);
                                        p.wykladowcy[numer_wykladowcy].plan[rand_czas_i, rand_czas_j] = temp_zajecie;
                                        for (int j = 0; j < Zajecia[i].dlugosc; j++)
                                        {
                                            p.sale[Sale.IndexOf(Zajecia[i].przedmiot.sala)].dostepnosc_sali[rand_czas_i, rand_czas_j + j] = 1;
                                            p.wykladowcy[numer_wykladowcy].plan_i[rand_czas_i, rand_czas_j + j] = 1;
                                        }
                                    }
                                    else
                                        i--;
                                }
                                else
                                    i--;
                            }
                            else
                                i--;
                        }//
                    }
                    else
                        i = Zajecia.Count;
                }
                if (proby > 0)
                {
                    proby = 1000;
                    Planki.Add(p);
                    //ocen_je(p);//Nowa metoda dla multi
                   ocen_to_v5(Planki.IndexOf(p));
                    p = null;
                }
                else
                {
                    proby = 1000;
                    tries++;
                    p = null;
                }
            }
            wgraj_ten_szajs_v5(semestr);
        }
        //Ukladanie zajec wedlug wykladowcy o najkrotszym czasie na uczelni ( im krotej tym zajecia sa szybciej losowane)
        public void uloz_przedmioty()
        {
            Boolean wyklad_first = false;
            int licznik_h,licznik_p;
            foreach (Wykladowca w in Wykladowcy)
            {
                licznik_h = 0;
                licznik_p = 0;
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 40; j++)
                        if (w.dostepnosc[i, j] == 1)
                            licznik_h++;
                foreach (Przedmiot p in Przedmioty)
                    if (p.nauczyciel == w)
                        licznik_p++;
                w.czas = licznik_h - (5*licznik_p);
            }
            Przedmiot[] tempowo = new Przedmiot[Przedmioty.Count];
            tempowo = Przedmioty.ToArray();
            int n = Przedmioty.Count;
            do
            {
                for (int i = 0; i < n - 1; i++)
                {
                    if (tempowo[i].nauczyciel.czas > tempowo[i + 1].nauczyciel.czas)
                    {
                        Przedmiot tmp = tempowo[i];
                        tempowo[i] = tempowo[i + 1];
                        tempowo[i + 1] = tmp;
                    }
                }
                n--;
            }
            while (n > 1);
            if(wyklad_first)//TODO WYKLADY_FERST
            {
                n = Przedmioty.Count;
                do
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        if (tempowo[i].nauczyciel == tempowo[i + 1].nauczyciel)
                        {
                            //if(tempowo[i])
                            {

                            }
                            Przedmiot tmp = tempowo[i];
                            tempowo[i] = tempowo[i + 1];
                            tempowo[i + 1] = tmp;
                        }
                    }
                    n--;
                }
                while (n > 1);
            }
            Przedmioty.RemoveRange(0, Przedmioty.Count);
            Przedmioty.AddRange(tempowo.ToList());
        }
        public void ocen_to(int p)
        {
            int semestr = 6;
           // String spec = null;
            int mini = 1000;
            int mini_id=-1;
            int id_planu = p;
            int czas = 0;
            for(int i=0;i<5;i++)
            {
                for(int j=0;j<40;j++)
                {
                    if (Planki[id_planu].plan[i, j] != null)
                        czas += j + Planki[id_planu].plan[i, j].dlugosc;
                }
            }
            Planki[id_planu].ocena = czas;
            int licz = 0;
            foreach (Plany pp in Planki)
            {
                if(pp.semestr==semestr)
                {
                    if (mini > pp.ocena)
                    {
                        mini = pp.ocena;
                        mini_id = Planki.IndexOf(pp);
                    } 
                    licz++;
                }
            }
            if(licz>10)
            {
                for(int i=0;i<Planki.Count;i++)
                {
                if (Planki[i].semestr == semestr)
                    if (Planki[i].ocena != mini)
                        {
                            Planki.Remove(Planki[i]);
                            i--;
                        }
                }    
            }
        }
        public void ocen_to_v5(int p)
        {
            int semestr = 6;
            int mini = 1000;
            int mini_id = -1;
            int id_planu = p;
            int czas = 0;
            foreach(Zajecie z in Planki[id_planu].plann)
            {
                czas += z.j + z.dlugosc;
            }
            Planki[id_planu].ocena = czas;
            int licz = 0;
            foreach (Plany pp in Planki)
            {
                if (pp.semestr == semestr)
                {
                    if (mini > pp.ocena)
                    {
                        mini = pp.ocena;
                        mini_id = Planki.IndexOf(pp);
                    }
                    licz++;
                }
            }
            if (licz > 10)
            {
                for (int i = 0; i < Planki.Count; i++)
                {
                    if (Planki[i].semestr == semestr)
                        if (Planki[i].ocena != mini)
                        {
                            Planki.Remove(Planki[i]);
                            i--;
                        }
                }
            }
        }
        public void ocen_je()
        {
            int semestr = 6;
            int mini = 100000;
            //int id_planu = Planki.IndexOf(p);
            int czas = 0;
            int ilosc = Planki.Count;
            for(int k=0;k<ilosc;k++)
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 40; j++)
                    {
                        if (Planki[k].plan[i, j] != null)
                            czas += j + Planki[k].plan[i, j].dlugosc;
                    }
                }
            Planki[Planki.Count-1].ocena = czas;
            int licz = 0;
            for (int k = 0; k < ilosc; k++)
            {
                if (Planki[k].semestr == semestr)
                {
                    if (mini > Planki[k].ocena)
                    {
                        mini = Planki[k].ocena;
                    }
                    licz++;
                }
            }
            if (licz > 10)
            {
                for (int i = 0; i < Planki.Count; i++)
                {
                    if (Planki[i].semestr == semestr)
                        if (Planki[i].ocena != mini)
                        {
                            Planki.Remove(Planki[i]);
                            i--;
                        }
                }
            }
            //Planki.Sort();
        }
        public void wgraj_ten_szajs(int semestr)
        {
            if (Planki.Count == 0)
            {
                label2.Text = "TROBUL";
                return;
            }
                
            int najlepszy=Planki[Planki.Count-1].ocena;
            int najlepszy_id=0;
            foreach(Plany p in Planki)
            {
                if (p.semestr == semestr)
                    if (p.ocena < najlepszy)
                    {
                        najlepszy = p.ocena;
                        najlepszy_id = Planki.IndexOf(p);
                    }   
            }
            for(int i=0;i<5;i++)
            {
                for(int j=0;j<40;j++)
                {
                    for (int k = 0; k < Wykladowcy.Count; k++)
                    {
                        if(Planki[najlepszy_id].plan[i, j]!=null)
                            if (Planki[najlepszy_id].plan[i,j].przedmiot.nauczyciel.imie == Wykladowcy[k].imie)
                                Wykladowcy[k].plan[i, j] = Planki[najlepszy_id].plan[i, j];
                    }
                        
                    for(int k=0;k<Sale.Count;k++)
                        Sale[k].dostepnosc_sali[i, j] = Planki[najlepszy_id].sale[k].dostepnosc_sali[i, j];
                }
            }
            Plany najlepszy_plan = Planki[najlepszy_id];
            for (int i = 0; i < Planki.Count; i++)
            {
                if (Planki[i].semestr == semestr)
                    if (Planki[i]!=najlepszy_plan)
                    {
                        Planki.Remove(Planki[i]);
                        i--;
                    }
            }
            for (int i = 0; i < Planki.Count; i++)
                if (Planki[i].semestr == semestr)
                    najlepszy_id = i;
            dopisz_studenta(Planki[najlepszy_id].semestr,najlepszy_id);
            poka_plana_v5(najlepszy_id);
            Planki.Sort();
        }
        public void wgraj_ten_szajs_v5(int semestr)
        {
            if (Planki.Count == 0)
            {
                label2.Text = "TROBUL";
                return;
            }

            int najlepszy = Planki[Planki.Count - 1].ocena;
            int najlepszy_id = 0;
            foreach (Plany p in Planki)
            {
                if (p.semestr == semestr)
                    if (p.ocena < najlepszy)
                    {
                        najlepszy = p.ocena;
                        najlepszy_id = Planki.IndexOf(p);
                    }
            }
            foreach(Zajecie z in Planki[najlepszy_id].plann)
            {
                foreach(Wykladowca w in Wykladowcy)
                    if (z.przedmiot.nauczyciel.imie == w.imie)
                        w.plan[z.i, z.j] = z;
                foreach(Sala s in Sale)
                {
                    for(int i=0;i<z.dlugosc;i++)
                        s.dostepnosc_sali[z.i, z.j] = Planki[najlepszy_id].sale[Sale.IndexOf(s)].dostepnosc_sali[z.i, z.j+i];
                }     
            }
            Plany najlepszy_plan = Planki[najlepszy_id];
            for (int i = 0; i < Planki.Count; i++)
            {
                if (Planki[i].semestr == semestr)
                    if (Planki[i] != najlepszy_plan)
                    {
                        Planki.Remove(Planki[i]);
                        i--;
                    }
            }
            for (int i = 0; i < Planki.Count; i++)
                if (Planki[i].semestr == semestr)
                    najlepszy_id = i;
            dopisz_studenta_v5(Planki[najlepszy_id].semestr, najlepszy_id);
            Planki[najlepszy_id].kompatobilnosc();
            poka_plana_v5(najlepszy_id);
            Planki.Sort();
        }
        public void dopisz_studenta(int input_semestr, int id_planu)
        {
            int counter = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    if (Planki[id_planu].plan[i, j] != null)
                        counter = Planki[id_planu].plan[i, j].miejsca;
                    foreach (Student s in Studenty)
                    {
                        if (s.semestr == input_semestr)
                        {
                            if (Planki[id_planu].plan[i, j] != null && counter != 0)
                            {
                                if (s.czy_ma_przedmiot(Planki[id_planu].plan[i, j].przedmiot) == false)
                                {
                                    if (Planki[id_planu].plan[i, j].przedmiot.specjalizacja == null)
                                    {
                                        s.plan[i, j] = Planki[id_planu].plan[i, j];
                                        counter--;
                                    }
                                    else if (Planki[id_planu].plan[i, j].przedmiot.specjalizacja != null&& Planki[id_planu].plan[i, j].przedmiot.multi_spec==false)
                                    {
                                        if (s.specjalizacja == Planki[id_planu].plan[i, j].przedmiot.specjalizacja)
                                        {
                                            s.plan[i, j] = Planki[id_planu].plan[i, j];
                                            counter--;
                                        }
                                    }
                                    else if (Planki[id_planu].plan[i, j].przedmiot.specjalizacja != null && Planki[id_planu].plan[i, j].przedmiot.multi_spec == true)
                                    {
                                        String[] temp = Planki[id_planu].plan[i, j].przedmiot.specjalizacja.Split(','); 
                                        foreach(String ss in temp)
                                        {
                                            if (s.specjalizacja == ss)
                                            {
                                                s.plan[i, j] = Planki[id_planu].plan[i, j];
                                                counter--;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            label2.Text = "hover";
        }

        public void dopisz_studenta_v5(int input_semestr, int id_planu)
        {
            int counter = 0;
            foreach(Zajecie z in Planki[id_planu].plann)
            {
                counter = z.miejsca;
                foreach(Student s in Studenty)
                {
                    if(s.semestr==input_semestr)
                    {
                        if(counter!=0)
                        {
                            if(s.czy_ma_przedmiot(z.przedmiot)==false)
                            {
                                if(z.przedmiot.specjalizacja==null)
                                {
                                    s.plan[z.i, z.j] = z;
                                    counter--;
                                }
                                else if(z.przedmiot.specjalizacja !=null && z.przedmiot.multi_spec==false)
                                {
                                    if(s.specjalizacja==z.przedmiot.specjalizacja)
                                    {
                                        s.plan[z.i, z.j] = z;
                                        counter--;
                                    }
                                }
                                else if (z.przedmiot.specjalizacja != null && z.przedmiot.multi_spec == true)
                                {
                                    String[] temp = z.przedmiot.specjalizacja.Split(',');
                                    foreach (String ss in temp)
                                    {
                                        if (s.specjalizacja == ss)
                                        {
                                            s.plan[z.i, z.j] = z;
                                            counter--;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void poka_plana(int id_planu)
        {
            Form f = new mpi(id_planu);
            f.Show();
        }
        public void poka_plana_v5(int id_planu)
        {
            Form f = new f_plan(id_planu);
            f.Show();
        }
    }
}
