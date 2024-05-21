using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<Plyta> bazaDanych = new List<Plyta>();

    static void Main()
    {
        
        DodajPrzykladoweDane();

        
        WyswietlWszystkiePlyty();

       
        WyswietlSzczegolyPlyty(1);

        
        WyswietlSzczegolyUtworu(1, 1);

        
        ZapiszBazeDoPliku("baza.txt");

        
        OdczytajBazeZPliku("baza.txt");
    }

    static void DodajPrzykladoweDane()
    {
        var utwory1 = new List<Utwor>
        {
            new Utwor { Tytul = "Utwor 1", CzasTrwania = new TimeSpan(0, 3, 45), Wykonawcy = new List<string> { "Wykonawca 1" }, Kompozytor = "Kompozytor 1", NumerNaPlycie = 1 },
            new Utwor { Tytul = "Utwor 2", CzasTrwania = new TimeSpan(0, 4, 20), Wykonawcy = new List<string> { "Wykonawca 2" }, Kompozytor = "Kompozytor 2", NumerNaPlycie = 2 }
        };
        var plyta1 = new Plyta { Tytul = "Plyta 1", Typ = "CD", CzasTrwania = new TimeSpan(0, 50, 0), Utwory = utwory1, Numer = 1 };

        bazaDanych.Add(plyta1);
    }

    static void WyswietlWszystkiePlyty()
    {
        Console.WriteLine("Wszystkie plyty w bazie:");
        foreach (var plyta in bazaDanych)
        {
            Console.WriteLine(plyta.Tytul);
        }
    }

    static void WyswietlSzczegolyPlyty(int numerPlyty)
    {
        var plyta = bazaDanych.Find(p => p.Numer == numerPlyty);
        if (plyta != null)
        {
            Console.WriteLine($"Szczegoly plyty: {plyta.Tytul}");
            Console.WriteLine($"Typ: {plyta.Typ}");
            Console.WriteLine($"Czas trwania: {plyta.CzasTrwania}");
            Console.WriteLine("Utwory:");
            foreach (var utwor in plyta.Utwory)
            {
                Console.WriteLine($"  {utwor.NumerNaPlycie}. {utwor.Tytul}");
            }
        }
    }

    static void WyswietlSzczegolyUtworu(int numerPlyty, int numerUtworu)
    {
        var plyta = bazaDanych.Find(p => p.Numer == numerPlyty);
        if (plyta != null)
        {
            var utwor = plyta.Utwory.Find(u => u.NumerNaPlycie == numerUtworu);
            if (utwor != null)
            {
                Console.WriteLine($"Szczegoly utworu: {utwor.Tytul}");
                Console.WriteLine($"Czas trwania: {utwor.CzasTrwania}");
                Console.WriteLine("Wykonawcy: " + string.Join(", ", utwor.Wykonawcy));
                Console.WriteLine($"Kompozytor: {utwor.Kompozytor}");
            }
        }
    }

    static void ZapiszBazeDoPliku(string sciezka)
    {
        using (var writer = new StreamWriter(sciezka))
        {
            foreach (var plyta in bazaDanych)
            {
                writer.WriteLine($"{plyta.Numer}|{plyta.Tytul}|{plyta.Typ}|{plyta.CzasTrwania}");
                foreach (var utwor in plyta.Utwory)
                {
                    writer.WriteLine($"  {utwor.NumerNaPlycie}|{utwor.Tytul}|{utwor.CzasTrwania}|{string.Join(", ", utwor.Wykonawcy)}|{utwor.Kompozytor}");
                }
            }
        }
    }

    static void OdczytajBazeZPliku(string sciezka)
    {
        if (File.Exists(sciezka))
        {
            var lines = File.ReadAllLines(sciezka);
            Plyta currentPlyta = null;
            foreach (var line in lines)
            {
                if (!line.StartsWith("  "))
                {
                    var plytaData = line.Split('|');
                    currentPlyta = new Plyta
                    {
                        Numer = int.Parse(plytaData[0]),
                        Tytul = plytaData[1],
                        Typ = plytaData[2],
                        CzasTrwania = TimeSpan.Parse(plytaData[3]),
                        Utwory = new List<Utwor>()
                    };
                    bazaDanych.Add(currentPlyta);
                }
                else
                {
                    var utworData = line.Trim().Split('|');
                    var utwor = new Utwor
                    {
                        NumerNaPlycie = int.Parse(utworData[0]),
                        Tytul = utworData[1],
                        CzasTrwania = TimeSpan.Parse(utworData[2]),
                        Wykonawcy = new List<string>(utworData[3].Split(',')),
                        Kompozytor = utworData[4]
                    };
                    currentPlyta.Utwory.Add(utwor);
                }
            }
        }
    }
}

class Plyta
{
    public string Tytul { get; set; }
    public string Typ { get; set; }
    public TimeSpan CzasTrwania { get; set; }
    public List<Utwor> Utwory { get; set; }
    public int Numer { get; set; }
}

class Utwor
{
    public string Tytul { get; set; }
    public TimeSpan CzasTrwania { get; set; }
    public List<string> Wykonawcy { get; set; }
    public string Kompozytor { get; set; }
    public int NumerNaPlycie { get; set; }
}

