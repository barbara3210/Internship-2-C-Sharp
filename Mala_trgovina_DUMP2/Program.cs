using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

var artikli=new List<Tuple<string,decimal,DateTime,int>>();
var prodani= new List<Tuple<string, int, decimal>>();
var racuni = new List<Tuple<int, DateTime, List<Tuple<string, int, decimal>>>>();
var radnici = new Dictionary<string, DateTime>();
const string password = "barbara3210";

while (true)
{
    Console.Write(
       "--------------------------\n" +
      "1- Artikli\n" +
      "2 - Radnici\n" +
      "3- Racuni\n" +
      "4 - Statistika\n" +
      "0- Izlaz iz aplikacije\n"+
      "--------------------------\n" 
      );

    var unos = Console.ReadLine();
    //Artikli
    while (int.Parse(unos) == 1)
    {
        Console.Write(
       "--------------------------\n" +
        "1 - Unos artikla\n" +
        "2 - Brisanje artikla\n" +
        "3 - Uređivanje artikla\n" +
        "4 - Ispis\n" +
        "0 - Nazad\n" +
        "--------------------------\n"
        );
   
        var odabir = Console.ReadLine();
        string ime;
        decimal cijena;
        DateTime datum;
        int kolicina;

        if(int.TryParse(odabir,out int odabranBroj))
        {
            //Unos artikla
            if (odabranBroj == 1)
            {
                Console.WriteLine("UNOS ARTIKLA");
                Console.WriteLine("Naziv artikla: ");
                ime = Console.ReadLine();
                do
                {
                    Console.WriteLine("Cijena artikla: ");
                    if (decimal.TryParse(Console.ReadLine(), out cijena))
                        break;
                    else
                        Console.WriteLine("Ponovno unesite cijenu");
                } while (true);

                do
                {
                    Console.WriteLine("Rok trajanja: ");
                    if (DateTime.TryParse(Console.ReadLine(), out datum))
                        break;
                    else
                        Console.WriteLine("Ponovno unesite rok");
                } while (true);
                do
                {
                    Console.WriteLine("Kolicina: ");
                    if (int.TryParse(Console.ReadLine(), out kolicina))
                        break;
                    else
                        Console.WriteLine("Ponovno unesite kolicinu");
                } while (true);

                var novi_artikl = new Tuple<string, decimal, DateTime, int>(ime, cijena, datum, kolicina);
                artikli.Add(novi_artikl);
                IspisiArtikle(artikli);

            }
            //Brisanje artikla
            else if (odabranBroj == 2)
            {
                Console.Write(
               "--------------------------\n" +
                "1- Izbriši artikl po imenu\n" +
                "2 - Izbriši s istekom roka\n" +
                "0- Nazad\n" +
                "--------------------------\n"
                );
                var opcija = Console.ReadLine();

                if (int.Parse(opcija) == 1)
                {
                    IspisiArtikle(artikli);
                    Console.WriteLine("Unesite naziv:");
                    ime = Console.ReadLine();
                    BrisanjeArtPoImenu(ime, artikli);
                }
                else if (int.Parse(opcija) == 2)
                {
                    IstekRoka(artikli);
                    IspisiArtikle(artikli);
                }
                else if (int.Parse(opcija) == 0)
                {
                    break;
                }
                else
                    IspisiArtikle(artikli);


            }
            //Uređivanje artikla
            else if (odabranBroj == 3)
            {
                Console.Write(
               "--------------------------\n" +
                "1- Uredi proizvod\n" +
                "2 - Promjena cijene\n" +
                "0- Nazad\n" +
                "--------------------------\n"
                );
                var opcijaUnos = Console.ReadLine();
                if(int.TryParse(opcijaUnos,out int opcija))
                {
                    //uredi proizvod
                    if (opcija == 1)
                    {
                        string art;

                        do
                        {
                            Console.WriteLine("Unesite naziv artikla: ");
                            art = Console.ReadLine();
                        } while (string.IsNullOrEmpty(art));


                        Console.Write(
                          "--------------------------\n" +
                           "1 - Promijeni ime\n" +
                           "2 - Promijeni cijenu\n" +
                           "3 - Promijeni rok\n" +
                           "4 - Promijeni kolicinu\n" +
                           "0 - Nazad\n" +
                           "--------------------------\n"
                        );
                        var pr = Console.ReadLine();

                        if (int.TryParse(pr, out int promjena))
                        {
                            switch (promjena)
                            {
                                case 0:
                                    break;
                                case 1:
                                    UpdateIme(artikli, art);
                                    IspisiArtikle(artikli);
                                    break;
                                case 2:
                                    UpdateCijena(artikli, art);
                                    IspisiArtikle(artikli);
                                    break;
                                case 3:
                                    UpdateDatum(artikli, art);
                                    IspisiArtikle(artikli);
                                    break;
                                case 4:
                                    UpdateKolicina(artikli, art);
                                    IspisiArtikle(artikli);
                                    break;
                                default: break;
                            }
                        }
                        else { Console.WriteLine("Pogrešan format unosa!"); }
                    }
                    //promjena cijene
                    else if (opcija == 2)
                    {
                        Console.WriteLine("PROMJENA SVIH CIJENA U TRGOVINI");
                        Console.Write(
                          "--------------------------\n" +
                           "1 - Popust\n" +
                           "2 - Poskupljenje\n" +
                           "0 - Nazad\n" +
                           "--------------------------\n"
                        );
                        var p = Console.ReadLine();

                        if (int.TryParse(p, out int promjenaOdabir))
                        {
                            //smanjenje cijene
                            if (promjenaOdabir == 1)
                            {
                                Console.WriteLine("Unesite postotak smanjenja cijene: ");
                                var popustUnos = Console.ReadLine();

                                if (decimal.TryParse(popustUnos, out decimal popust))
                                {
                                    if (popust >= 0 && popust <= 100)
                                    {
                                        List<Tuple<string, decimal, DateTime, int>> updatedLista = new List<Tuple<string, decimal, DateTime, int>>();


                                        foreach (var a in artikli)
                                        {
                                            decimal upCijena = a.Item2 - (a.Item2 * popust / 100);

                                            var updatedTuple = new Tuple<string, decimal, DateTime, int>(a.Item1, upCijena, a.Item3, a.Item4);
                                            updatedLista.Add(updatedTuple);
                                        }
                                        artikli.Clear();
                                        artikli = updatedLista;
                                    }
                                    else { Console.WriteLine("Pogresno ste unijeli postotak"); }
                                }
                                else { Console.WriteLine("Pogresan unos"); }

                            }
                            //poskupljenje
                            else if (promjenaOdabir == 2)
                            {
                                Console.WriteLine("Unesite postotak povecanja cijene: ");
                                var povecanjeUnos = Console.ReadLine();

                                if (decimal.TryParse(povecanjeUnos, out decimal postotak))
                                {
                                    if (postotak >= 0 && postotak <= 100)
                                    {
                                        List<Tuple<string, decimal, DateTime, int>> updatedLista = new List<Tuple<string, decimal, DateTime, int>>();


                                        foreach (var a in artikli)
                                        {
                                            decimal upCijena = a.Item2 + (a.Item2 * postotak / 100);

                                            var updatedTuple = new Tuple<string, decimal, DateTime, int>(a.Item1, upCijena, a.Item3, a.Item4);
                                            updatedLista.Add(updatedTuple);
                                        }
                                        artikli.Clear();
                                        artikli = updatedLista;
                                    }
                                    else { Console.WriteLine("Pogresno ste unijeli postotak"); }
                                }
                                else { Console.WriteLine("Pogresan unos"); }

                            }
                            else if (int.Parse(p) == 0) { break; }
                            Console.WriteLine();
                        }
                    }
                    //Izlaz
                    else if (opcija == 0) { break; }
                    else { Console.WriteLine(); }
                }
                
            }
            //Ispis po..
            else if (odabranBroj == 4)
            {
                Console.Write(
              "--------------------------\n" +
               "1- Ispis svih artikala\n" +
               "2 - Svih artikala sortirano po imenu\n" +
               "3 - Svih artikala sortirano po datumu silazno\n" +
               "4 - Svih artikala sortirano po datumu uzlazno\n" +
               "5 - Svih artikala sortirano po količini\n" +
               "6 - Najprodavaniji artikl\n" +
               "7 - Najmanje prodavan artikl\n" +
               "--------------------------\n"
               );
                var broj = int.Parse(Console.ReadLine());

                switch (broj)
                {
                    case 1:
                        IspisiArtikle(artikli);
                        break;
                    case 2:
                        IspisAbecedno(artikli);
                        break;
                    case 3:
                        IspisDatumSilazno(artikli);
                        break;
                    case 4:
                        IspisDatumUzlazno(artikli);
                        break;
                    case 5:
                        IspisKol(artikli);
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    default: break;

                }
            }
            //Izlaz
            else if (odabranBroj == 0) { break; }
            else { Console.WriteLine(); }
        }
        else { Console.WriteLine("Pogresan unos"); }
        
    }
    //Radnici
    while (int.Parse(unos) == 2)
    {
        Console.Write(
          "--------------------------\n" +
           "1 - Unos radnika\n" +
           "2 - Brisanje radnika\n" +
           "3 - Uređivanje radnika\n" +
           "4 - Ispis\n" +
           "0 - Nazad\n" +
           "--------------------------\n"
           );
        
        string ime;
        DateTime rodenje;
        
        var odabir = Console.ReadLine();

        if (int.TryParse(odabir, out int odabranBroj))
        {
            //unos radnika
            if (odabranBroj == 1)
            {
                Console.WriteLine("UNOS RADNIKA");
                Console.WriteLine("Ime: ");
                ime = Console.ReadLine();
                do
                {
                    Console.WriteLine("Datum rod: ");
                    if (DateTime.TryParse(Console.ReadLine(), out rodenje))
                        break;
                    else
                        Console.WriteLine("Ponovno unesite datum");
                } while (true);


                radnici.Add(ime, rodenje);

            }
            //brisanje radnika
            else if (odabranBroj == 2)
            {
                Console.Write(
              "--------------------------\n" +
               "1- Izbriši radnika po imenu\n" +
               "2 - Izbriši radnika starijeg od 65\n" +
               "0- Nazad\n" +
               "--------------------------\n"
               );
                var broj = Console.ReadLine();
                if (int.Parse(broj) == 1)
                {
                    IspisRadnika(radnici);
                    Console.WriteLine("Unesite ime koje zelite izbrisati: ");
                    string radnik = Console.ReadLine();
                    BrisiRadnika(radnici, radnik);
                    IspisRadnika(radnici);
                }
                else if (int.Parse(broj) == 2)
                {
                    Console.WriteLine("Brisanje svih starijih od 65");
                    BrisiRadnika65(radnici);
                    IspisRadnika(radnici);
                }
                else if (int.Parse(broj) == 0) { break; }
                else { Console.WriteLine(); }
            }
            //uredivanje radnika
            else if (odabranBroj == 3)
            {
                string r;

                do
                {
                    Console.WriteLine("Unesite ime radnika kojeg uredujete: ");
                    r = Console.ReadLine();
                } while (string.IsNullOrEmpty(r));

                foreach (var radnik in radnici)
                {
                    if (radnik.Key == r)
                    {
                        Console.Write(
                      "--------------------------\n" +
                       "1 - Promijeni ime\n" +
                       "2 - Promijeni datum rod\n" +
                       "0 - Nazad\n" +
                       "--------------------------\n"
                        );
                        var br = Console.ReadLine();

                        if (int.Parse(br) == 1)
                        {
                            Console.WriteLine("Unesite novo ime: ");
                            string novoIme = Console.ReadLine();

                            var updatedRadnici = new Dictionary<string, DateTime>(radnici);
                            updatedRadnici[novoIme] = radnik.Value;

                            radnici = updatedRadnici;

                        }
                        else if ((int.Parse(br) == 2))
                        {
                            Console.WriteLine("Unesite novi datum: ");
                            DateTime d = DateTime.Parse(Console.ReadLine());

                            var updatedRadnici = new Dictionary<string, DateTime>(radnici);
                            updatedRadnici[radnik.Key] = d;

                            radnici = updatedRadnici;
                        }
                        else if (int.Parse(br) == 0) { break; }
                        else { Console.WriteLine(); }

                    }
                }


            }
            //ispis
            else if (odabranBroj == 4)
            {
                Console.Write(

              "--------------------------\n" +
               "1 - Ispis svih radnika\n" +
               "2 - Ispis radnika kojima je ubrzo rodendan\n" +
               "0 - Nazad\n" +
               "--------------------------\n"
               );
                var br = Console.ReadLine();
                if (int.Parse(br) == 1) { IspisRadnika(radnici); }
                else if (int.Parse(br) == 2) { Rodendan(radnici); }
                else if ((int.Parse(br) == 0)) { break; }
                else { Console.WriteLine(); }

            }
            //Izlaz
            else if (odabranBroj == 0) { break; }
            else { Console.WriteLine(); }
        }
        else { Console.WriteLine("Pogresan unos"); }


    }
    //Racuni
    while (int.Parse(unos) == 3)
    {
        Console.Write(
        "--------------------------\n" +
         "1 - Unos novog racuna\n" +
         "2 - Ispis racuna\n" +
         "0 - Nazad\n" +
         "--------------------------\n"
         );

        var odabir= Console.ReadLine();
        if (int.TryParse(odabir, out int odabranBroj))
        {
            //novi racun
            if (odabranBroj == 1)
            {
                List<Tuple<string, int, decimal>> stavkeRacuna = new List<Tuple<string, int, decimal>>();
                int i=0;
                while (true)
                {
                    IspisiArtikle(artikli);
                    Console.WriteLine("Unesite ime proizvoda ili x za prekid:");
                    string imeProizvoda = Console.ReadLine();

                    if (imeProizvoda.ToLower() == "x")
                        break;
                    
                    if (TrazenProizvod(artikli,imeProizvoda)==null)
                    {
                        Console.WriteLine("Navedeni unos ne postoji");
                    }
                    else
                    {
                        var odabraniProizvod = artikli.FirstOrDefault(a => a.Item1 == imeProizvoda);

                        do
                        {
                            Console.WriteLine("Unesite količinu:");
                            var unosKol=Console.ReadLine();
                            if(int.TryParse(unosKol, out int kolicina))
                            {
                                if (odabraniProizvod.Item4 < kolicina)
                                {
                                    Console.WriteLine("Nedovoljno resursa");
                                }
                                else
                                {
                                    var proizvodTuple = new Tuple<string, int, decimal>(imeProizvoda, kolicina, odabraniProizvod.Item2 * kolicina);
                                    stavkeRacuna.Add(proizvodTuple);
                                    break;
                                }
                            }

                        } while (true);

                    }

                }

                Console.Write(
                    "--------------------------\n" +
                     "1 - Zavrsi kupnju\n" +
                     "2 - Odustani\n" +
                     "--------------------------\n"
                     );

                var n = Console.ReadLine();
                

                if (int.TryParse(n, out int nastavak))
                {
                    if (nastavak == 1)
                    {
                        i++;
                        IspisStavkiRc(stavkeRacuna);
                        Console.WriteLine("=======================================");
                        Console.WriteLine("RACUN:");
                        Console.WriteLine($"ID: {i}  ");
                        Console.WriteLine($"Datum: {DateTime.Now}  ");

                        foreach (var stavka in stavkeRacuna)
                        {
                            Console.WriteLine($"Proizvod: {stavka.Item1} Kolicina: {stavka.Item2} Ukupna cijena: {stavka.Item3}");
                        }
                        Console.WriteLine("--------------------------------------");
                        Console.WriteLine($"UKUPNO: {IzracunajUkupnuCijenu(stavkeRacuna)} EUR");
                        Console.WriteLine("=======================================");

                        var noviRacun = new Tuple<int, DateTime, List<Tuple<string, int, decimal>>>(i, DateTime.Now, new List<Tuple<string, int, decimal>>(stavkeRacuna));
                        racuni.Add(noviRacun);
                        ZavrsiKupnju(artikli, stavkeRacuna,prodani);
                    }
                    else if (nastavak == 2)
                    {
                        break;
                    }
                }
            }
            //ispis racuna
            else if(odabranBroj == 2)
            {
                Console.Write(
            "--------------------------\n" +
             "1 - Ispisi sve\n" +
             "0 - Nazad\n" +
             "--------------------------\n"
             );


            }
            else if (odabranBroj == 0) { break; }
            else { Console.WriteLine(); }

        }
        else { Console.WriteLine("Pogresan unos"); }


    }
    //Statistika
    while (int.Parse(unos) == 4)
    {
        
        Console.WriteLine("Unesite sifru: ");
        string sifra = Console.ReadLine();

        if (password == sifra)
        {
            Console.Write(
            "--------------------------\n" +
             "1 - Ukupan broj artikala u trgovini\n" +
             "2 - Vrijednost artikala koji još nisu prodani\n" +
             "3 - Vrijednost svih artikala koji su prodani (nalaze se na računima)\n" +
             "4 - Stanje po mjesecima\n" +
             "0 - Nazad\n" +
             "--------------------------\n"
             );
            var statUnos= Console.ReadLine();

            if(int.TryParse(statUnos,out int br))
            {
                switch(br)
                {
                    case 0: break;
                    case 1:
                        Console.WriteLine("Ukupan broj artikala u trgovini: "+ artikli.Count());
                        break;
                    case 2:
                        IznosArtikala(artikli);
                        break;
                    case 3:
                        Console.WriteLine($"Iznos svih prodanih proizvoda: {IznosProdanih(prodani)}");
                        break;
                    case 4:
                        Console.WriteLine($"Mjesec:");
                        break;
                    default:break;
                }
            }
        }
        else { Console.WriteLine("Pogresna sifra"); }

    }
    //Izlaz
    if (int.Parse(unos) == 0)
        break;
    else { Console.WriteLine(); }
}



static void BrisanjeArtPoImenu(string ime, List<Tuple<string, decimal, DateTime,int>> listaAr)
{
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        List<Tuple<string, decimal, DateTime, int>> upList = new List<Tuple<string, decimal, DateTime, int>>();

        foreach (var a in listaAr)
        {

            if (a.Item1 == ime) { Console.WriteLine($"Izbrisano: {a.Item1}"); }
            else { upList.Add(a); }
        }
        listaAr.Clear();
        listaAr.AddRange(upList);
    }    
}
static void IstekRoka( List<Tuple<string, decimal, DateTime, int>> listaAr)
{
    DateTime d= DateTime.Now;
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        List<Tuple<string, decimal, DateTime, int>> upList = new List<Tuple<string, decimal, DateTime, int>>();

        foreach (var a in listaAr)
        {
            
            if (a.Item3 < d) { Console.WriteLine($"Izbrisano: {a.Item1}"); }
            else { upList.Add(a); }
        }
        listaAr.Clear();
        listaAr.AddRange(upList);
    }
    

}
static void IspisiArtikle(List<Tuple<string,decimal,DateTime,int>> listaAr)
{
    Console.WriteLine("Svi artikli:");
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        foreach (var a in listaAr)
        {
            Console.WriteLine($"Naziv: {a.Item1.ToUpper()} Cijena: {a.Item2} Rok trajanja: {a.Item3.ToShortDateString()} Kolicina: {a.Item4}");
        }
    }
    
}
static void IspisAbecedno(List<Tuple<string, decimal, DateTime, int>> listaAr)
{
    Console.WriteLine("Artikli sortirani po imenu:");
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        var sortirano = listaAr.OrderBy(a => a.Item1).ToList();
        foreach (var a in sortirano)
        {
            Console.WriteLine($"Naziv: {a.Item1} Cijena: {a.Item2} Rok trajanja: {a.Item3.ToShortDateString()} Kolicina: {a.Item4}");
            
        }
    }

}
static void IspisDatumUzlazno(List<Tuple<string, decimal, DateTime, int>> listaAr)
{
    Console.WriteLine("Artikli sortirani po datumu uzlazno:");
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        var sortirano = listaAr.OrderBy(a => a.Item3).ToList();
        foreach (var a in sortirano)
        {
            Console.WriteLine($"Naziv: {a.Item1} Cijena: {a.Item2} Rok trajanja: {a.Item3.ToShortDateString()} Kolicina: {a.Item4}");
        }
    }

}
static void IspisDatumSilazno(List<Tuple<string, decimal, DateTime, int>> listaAr)
{
    Console.WriteLine("Artikli sortirani po datumu silazno:");
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        var sortirano = listaAr.OrderBy(a => a.Item3).Reverse().ToList();
        foreach (var a in sortirano)
        {
            Console.WriteLine($"Naziv: {a.Item1} Cijena: {a.Item2} Rok trajanja: {a.Item3.ToShortDateString()} Kolicina: {a.Item4}");
        }
    }

}
static void IspisKol(List<Tuple<string, decimal, DateTime,int>> listaAr)
{
    Console.WriteLine("Artikli sortirani po kolicini:");
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        var sortirano = listaAr.OrderBy(a => a.Item4).ToList();
        foreach (var a in sortirano)
        {
            Console.WriteLine($"Naziv: {a.Item1} Cijena: {a.Item2} Rok trajanja: {a.Item3.ToShortDateString()} Kolicina: {a.Item4}");
        }
    }

}

static void UpdateIme(List<Tuple<string, decimal, DateTime, int>> listaAr,string odabraniArt)
{
    Console.WriteLine("Unesite novi naziv: ");
    string upArtikal = Console.ReadLine();

    var updatedLista = new List<Tuple<string, decimal, DateTime, int>>();

    foreach (var a in listaAr)
    {
        if(a.Item1 == odabraniArt)
        {
            var updatedTuple = new Tuple<string, decimal, DateTime, int>(upArtikal, a.Item2, a.Item3, a.Item4);
            updatedLista.Add(updatedTuple);
        }
        else { updatedLista.Add(a); }
    }

    listaAr.Clear();
    listaAr.AddRange(updatedLista);
}
static void UpdateCijena(List<Tuple<string, decimal, DateTime, int>> listaAr, string odabraniArt)
{
    Console.WriteLine("Unesite novu cijenu: ");
    decimal upArtikal = decimal.Parse(Console.ReadLine());

    var updatedLista = new List<Tuple<string, decimal, DateTime, int>>();

    foreach (var a in listaAr)
    {
        if (a.Item1 == odabraniArt)
        {
            var updatedTuple = new Tuple<string, decimal, DateTime, int>(a.Item1, upArtikal, a.Item3, a.Item4);
            updatedLista.Add(updatedTuple);
        }
        else { updatedLista.Add(a); }
    }

    listaAr.Clear();
    listaAr.AddRange(updatedLista);
}
static void UpdateDatum(List<Tuple<string, decimal, DateTime, int>> listaAr, string odabraniArt)
{
    Console.WriteLine("Unesite novi rok trajanja: ");
    DateTime upArtikal = DateTime.Parse( Console.ReadLine());

    var updatedLista = new List<Tuple<string, decimal, DateTime, int>>();

    foreach (var a in listaAr)
    {
        if (a.Item1 == odabraniArt)
        {
            var updatedTuple = new Tuple<string, decimal, DateTime, int>(a.Item1, a.Item2, upArtikal, a.Item4);
            updatedLista.Add(updatedTuple);
        }
        else { updatedLista.Add(a); }
    }

    listaAr.Clear();
    listaAr.AddRange(updatedLista);
}
static void UpdateKolicina(List<Tuple<string, decimal, DateTime, int>> listaAr, string odabraniArt)
{
    Console.WriteLine("Unesite novu kolicinu: ");
    int upArtikal = int.Parse(Console.ReadLine());

    var updatedLista = new List<Tuple<string, decimal, DateTime, int>>();

    foreach (var a in listaAr)
    {
        if (a.Item1 == odabraniArt)
        {
            var updatedTuple = new Tuple<string, decimal, DateTime, int>(a.Item1, a.Item2, a.Item3, upArtikal);
            updatedLista.Add(updatedTuple);
        }
        else { updatedLista.Add(a); }
    }

    listaAr.Clear();
    listaAr.AddRange(updatedLista);
}

static void BrisiRadnika(Dictionary<string,DateTime> dRadnici,string ime)
{
    foreach (var d in dRadnici)
    {
        if (d.Key == ime)
        {
            dRadnici.Remove(d.Key);
            Console.WriteLine("Izbrisano");
        }
        else { Console.WriteLine("Unos ne postoji"); }
    }
}
static void BrisiRadnika65(Dictionary<string, DateTime> dRadnici)
{
    foreach (var d in dRadnici)
    {
        int god = DateTime.Now.Year - d.Value.Year;
        if (god>=65)
        {
            dRadnici.Remove(d.Key);
            Console.WriteLine("Izbrisano");
        }
        else { Console.WriteLine(); }
    }
}
static void IspisRadnika(Dictionary <string, DateTime> dRadnici)
{
    Console.WriteLine("~RADNICI~");
    foreach (var d in dRadnici)
    {
        Console.WriteLine($"Zaposlenik: {d.Key.ToUpper()} - {d.Value.ToShortDateString()}");
    }
}
static void Rodendan(Dictionary<string, DateTime> dRadnici)
{
    Console.WriteLine("~RADNICI KOJIMA JE RODENDAN OVAJ MJESEC~");
    var mj = DateTime.Now.Month;
    foreach (var d in dRadnici)
    {
        if(d.Value.Month == mj) 
            Console.WriteLine($"Zaposlenik: {d.Key.ToUpper()} - {d.Value.ToShortDateString()}");
    }

}

static void IznosArtikala(List<Tuple<string, decimal, DateTime, int>> listaAr)
{
    if (listaAr.Count == 0)
    {
        Console.WriteLine("Nema unesenih proizvoda!");
    }
    else
    {
        decimal ukCijena=0;
        foreach (var a in listaAr)
        {
            ukCijena += a.Item2;
            
        }
        Console.WriteLine($"Ukupna vrijednost proizvoda u trgovini: "+ukCijena);
    }
}
static decimal IznosProdanih(List<Tuple<string, int, decimal>> listaAr)
{

    if(listaAr.Count == 0)
    {
        Console.WriteLine("Nema prodanih proizvoda");
        return 0;
    }
    else
    {
        decimal ukCijena=0;
        foreach (var a in listaAr)
        {
            ukCijena+=a.Item3;
        }
        return ukCijena;
    }
}
static Tuple<string, decimal, DateTime, int> TrazenProizvod(List<Tuple<string, decimal, DateTime, int>> listaAr,string ime)
{
    if (listaAr.Count == 0)
        return null;
    foreach (var a in listaAr)
    {
        if (a.Item1 == ime)
            return a;
    }
    return null;

}
static void ZavrsiKupnju(List<Tuple<string, decimal, DateTime, int>> listaAr, List<Tuple<string, int, decimal>> listaRacun, List<Tuple<string, int, decimal>> listaProdano)
{
    var updatedLista = new List<Tuple<string, decimal, DateTime, int>>();
    foreach (var a in listaAr)
    {
        foreach (var b in listaRacun)
        {
            if(a.Item1==b.Item1 && a.Item4 >= b.Item2)
            {
                int novaKol = a.Item4 - b.Item2;
                var updatedTuple = new Tuple<string, decimal, DateTime, int>(a.Item1, a.Item2, a.Item3, novaKol);
                updatedLista.Add(updatedTuple);
               
            }
            else if(a.Item1 == b.Item1 && a.Item4 < b.Item2)
            {
                Console.WriteLine("Nema dovoljno resursa");
            }
            else { updatedLista.Add(a); }
        }
    }
    listaAr.Clear();
    listaProdano.AddRange(listaRacun);
    listaRacun.Clear();
    listaAr.AddRange(updatedLista);
}
static void IspisStavkiRc(List<Tuple<string, int, decimal>> listaRacun)
{
    
    foreach (var a in listaRacun)
    {
        Console.WriteLine($" Kupljeno: {a.Item1.ToUpper()} Kolicina: {a.Item2} Cijena: {a.Item3} ");

    }
}
static decimal IzracunajUkupnuCijenu(List<Tuple<string, int, decimal>> listaRacun)
{
    decimal ukupno = 0;

    foreach (var a in listaRacun)
    {
        ukupno += a.Item3;
    }

    return ukupno;
}
