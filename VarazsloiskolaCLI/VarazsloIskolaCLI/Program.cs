using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VarazsloIskolaCLI
{
        class Varazslo
        {
            public double Atlag { get; private set; }
            public string Tanonc { get; private set; }
            public string Trollkodas { get; private set; }
            public string igyekezettan { get; private set; }
            public int kipurcantan { get; private set; }
            public int logastan { get; private set; }
            public List<int> Naplo = new List<int>();
            public Varazslo(string sor)
            {
                string[] s = sor.Split(';');
                Tanonc = s[0];
                Trollkodas = s[1];
                igyekezettan = s[2];
                kipurcantan = Convert.ToInt32(s[15]);
                logastan = Convert.ToInt32(s[16]);
                int k = 11;
                if (s[7] == "FM")
                {
                    s[7] = "-1";
                    k--;
                }
                else if (s[7] == "")
                {
                    s[7] = "0";
                }
                if (s[8] == "")
                {
                    s[8] = "0";
                }
                for (int i = 3; i < 15; i++)
                {
                    Naplo.Add(Convert.ToInt32(s[i]));
                }
                Atlag = (double)Naplo.Sum() / k;
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
                var lista = new List<Varazslo>();
                var sr = new StreamReader("tanoncok.txt");
                while (!sr.EndOfStream)
                {
                    lista.Add(new Varazslo(sr.ReadLine()));
                }
                sr.Close();
                Console.WriteLine($"4. feladat: {lista.Count()} tanonc jár a varázslóiskolába");
                Console.WriteLine("5. feladat: Trollok:");
                var troll = (from sor in lista where sor.Trollkodas == "troll" select sor);
                foreach (var item in troll)
                {
                    Console.WriteLine($"\t{item.Tanonc}");
                }
                Console.Write("6. feladat: Kérek egy tanonc nevet! ");
                string nev = Console.ReadLine();
                var vane = (from sor in lista where sor.Tanonc == nev select sor);
                if (vane.Any())
                {
                    foreach (var item in vane)
                    {
                        Console.WriteLine($"\t{nev} átlaga {item.Atlag:0.##}");
                    }
                }
                else
                {
                    Console.WriteLine("A megadott tanonc nem varázsolhat!");
                }
                var log = (from sor in lista select sor.logastan).Max();
                var logas = (from sor in lista where sor.logastan == log select sor.Tanonc);
                Console.WriteLine($"7. feladat: A Lógástan bajnoka {logas.First()}");

                var stat = new SortedDictionary<int, int>();
                foreach (var sor in lista)
                {
                    foreach (var jegy in sor.Naplo)
                    {
                        if (stat.Keys.Contains(jegy))
                        {
                            stat[jegy] += 1;
                        }
                        else
                        {
                            stat[jegy] = 1;
                        }
                    }
                }
                foreach (var item in stat.Reverse())
                {
                    if (item.Key > 0)
                    {
                        Console.WriteLine($"\t     {item.Key}  {item.Value}");
                    }
                }
                var nyolc = (from sor in lista where sor.Naplo.Contains(-1) select sor.Tanonc).First();
                Console.WriteLine($"9. feladat: {nyolc} a farmakológus.");

                var gyogyn = (from sor in lista where sor.Naplo[5] > 1 select sor).Count();

                var jos = (from sor in lista where sor.Naplo[4] > 1 select sor).Count();
                Console.WriteLine($"10. feladat: Gyógynövénytanra {jos}-en járnak, Jóslástant {gyogyn}-en tanulnak.");
               //12,11
            var srr = new StreamReader("tantargyak.txt");
            var list2 = srr.ReadLine().Split(';');
            srr.Close();
            var f12 = "";
            double maxi = 0;
            string tant = "";
            for (int i = 0; i < 12; i++)
            {
                var atl = (from sor in lista select sor.Naplo[i]).Average();
                if (atl>maxi)
                {
                    maxi = atl;
                    tant = list2[i + 2];
                }

                f12+=$"\n\t     {list2[i+2]} {atl:0.##}";
            }
            Console.WriteLine($"11. feladat: A tanoncok kedvence a {tant} vagy amit akartok ;)");
            Console.Write("12. feladat: A tantárgyak átlaga:");
            Console.WriteLine($"{f12}");

            

            var atlkip = (from sor in lista select sor.kipurcantan).Average();
            Console.WriteLine($"13. feladat: A Kipurcanástan átlaga: {atlkip:0.##}");            
            var maxikip = (from sor in lista orderby sor.kipurcantan select sor).Last();
            Console.WriteLine($"14. feladat: A legtöbbet kipurcanó tanonc {maxikip.Tanonc}");
            //15
            double bir = (from sor in lista where sor.Tanonc == "Hosszú Platón" select sor.Atlag).First();
            Console.WriteLine($"15. Feladat: Hosszú Platún átéaga: {bir:0.00}");
            Console.WriteLine();
            var nevek = new List<string>();
            var f = (from sor in lista where !sor.Naplo.Contains(1) select sor.Tanonc);
            var sw = new StreamWriter("varazsolhatnak.txt");
            foreach (var item in f)
            {
                sw.WriteLine(item);
            }
            sw.Close();
                Console.ReadKey();
            }
        }
    }
