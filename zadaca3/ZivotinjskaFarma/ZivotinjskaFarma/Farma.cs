﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZivotinjskaFarma
{
    public class Farma
    {
        #region Atributi

        List<Zivotinja> zivotinje;
        List<Lokacija> lokacije;
        List<Proizvod> proizvodi;
        List<Kupovina> kupovine;

        #endregion

        #region Properties

        public List<Zivotinja> Zivotinje { get => zivotinje; }
        public List<Lokacija> Lokacije { get => lokacije; }
        public List<Proizvod> Proizvodi { get => proizvodi; set => proizvodi = value; }
        public List<Kupovina> Kupovine { get => kupovine; }

        #endregion

        #region Konstruktor

        public Farma()
        {
            zivotinje = new List<Zivotinja>();
            lokacije = new List<Lokacija>();
            proizvodi = new List<Proizvod>();
            kupovine = new List<Kupovina>();
        }

        #endregion

        #region Metode

        public void RadSaZivotinjama(string opcija, Zivotinja zivotinja, int maxStarost)
        {
            if (zivotinje.Count > 0)
            {
                Zivotinja postojeca = null;
                foreach (Zivotinja z in zivotinje)
                {
                    Zivotinja z2 = null;
                    if (z.ID1 == zivotinja.ID1)
                        z2 = z;
                    if (z2 != null)
                    {
                        if (maxStarost * 365 > z2.Starost.Year)
                            postojeca = z2;
                    }    
                }

                if (opcija == "Dodavanje")
                    if (postojeca == null)
                        zivotinje.Add(zivotinja);
                    else
                        throw new ArgumentException("Životinja je već registrovana u bazi!");

                else if (opcija == "Izmjena")
                    if (postojeca != null)
                    {
                        var index = zivotinje.IndexOf(postojeca);
                        zivotinje.RemoveAt(index);
                        zivotinje.Add(zivotinja);
                    }
                    else
                        throw new ArgumentException("Životinja nije registrovana u bazi!");

                else if (opcija == "Brisanje")
                    if (postojeca != null)
                        zivotinje.Remove(postojeca);
                    else
                        throw new ArgumentException("Životinja nije registrovana u bazi!");

                else if (postojeca == null)
                    throw new ArgumentException("Životinja nije registrovana u bazi!");
                
                else
                    throw new ArgumentException("Životinja je već registrovana u bazi!");

            }

            else
                return;
        }

        public void RadSaZivotinjamaCodeTunning(string opcija, Zivotinja zivotinja, int maxStarost)
        {
            if (zivotinje.Count > 0)
            {
                Zivotinja postojeca = null;
                
                foreach (Zivotinja z in zivotinje)
                {
                    if (z.ID1 == zivotinja.ID1 && maxStarost * 365 > z.Starost.Year)
                    {
                        postojeca = z;
                        break;
                    }
                        
                }

                if (opcija == "Dodavanje")
                    if (postojeca == null)
                        zivotinje.Add(zivotinja);
                    else
                        throw new ArgumentException("Životinja je već registrovana u bazi!");

                else if (opcija == "Izmjena")
                    if (postojeca != null)
                    {
                        var index = zivotinje.IndexOf(postojeca);
                        zivotinje.RemoveAt(index);
                        zivotinje.Add(zivotinja);
                    }
                    else
                        throw new ArgumentException("Životinja nije registrovana u bazi!");

                else if (opcija == "Brisanje")
                    if (postojeca != null)
                        zivotinje.Remove(postojeca);
                    else
                        throw new ArgumentException("Životinja nije registrovana u bazi!");

                else if (postojeca == null)
                    throw new ArgumentException("Životinja nije registrovana u bazi!");

                else
                    throw new ArgumentException("Životinja je već registrovana u bazi!");

            }

            else
                return;
        }

        public void DodavanjeNoveLokacije(Lokacija lokacija)
        {
            if (lokacije.Any(l => l.Grad == lokacija.Grad && l.Adresa == lokacija.Adresa
                        && l.BrojUlice == lokacija.BrojUlice))
                throw new InvalidOperationException("Ista lokacija je već zabilježena!");
            lokacije.Add(lokacija);
        }

        public bool BrisanjeLokacije(Lokacija lokacija)
        {
            return lokacije.Remove(lokacija);
        }

        public bool KupovinaProizvoda(Proizvod p, DateTime rok, int količina)
        {
            bool popust = Praznik(DateTime.Now);
            int id = Kupovina.DajSljedeciBroj();
            Kupovina kupovina = new Kupovina(id.ToString(), DateTime.Now, rok, p, količina, popust);
            if (!kupovina.VerificirajKupovinu())
                return false;
            else
            {
                Kupovina postojecaKupovina = null;
                for (int i = 0; i < kupovine.Count; i++)
                {
                    if (kupovine[i].DatumKupovine == kupovina.DatumKupovine)
                    {
                        if (kupovine[i].IDKupca1 == kupovina.IDKupca1)
                        {
                            if (kupovine[i].KupljeniProizvod == kupovina.KupljeniProizvod)
                                postojecaKupovina = kupovine[i];
                            else
                                continue;
                        }
                    }
                }

                if (postojecaKupovina == null)
                    kupovine.Add(kupovina);
                else
                    return false;

                return true;
            }
        }

        public bool KupovinaProizvodaRefactoring(Proizvod p, DateTime rok, int količina)
        {
            DateTime danas = DateTime.Now;
            bool popust = Praznik(danas);
            int id = Kupovina.DajSljedeciBroj();
            Kupovina kupovina = new Kupovina(id.ToString(), danas, rok, p, količina, popust);
            if (!kupovina.VerificirajKupovinu())
                return false;

            Kupovina postojecaKupovina = null;
            for (int i = 0; i < kupovine.Count; i++)
            {
               if (provjeraKupovine(kupovine[i], kupovina)){

                   postojecaKupovina = kupovine[i];
                   break;
               }     
            }

            return validnaKupovina(postojecaKupovina, kupovina);
        }

        private bool provjeraKupovine(Kupovina kupovina1, Kupovina kupovina2)
        {
            return kupovina1.DatumKupovine == kupovina2.DatumKupovine && kupovina1.IDKupca1 == kupovina2.IDKupca1
                         && kupovina1.KupljeniProizvod == kupovina2.KupljeniProizvod;
        }

        private bool validnaKupovina(Kupovina postojecaKupovina, Kupovina kupovina)
        {
            if (postojecaKupovina == null) {
                kupovine.Add(kupovina);
                return true;
            }

            return false;
        }

        public void BrisanjeKupovine(Kupovina kupovina)
        {
            if (kupovine.Contains(kupovina))
                kupovine.Remove(kupovina);
        }

        public void ObaviSistematskiPregled(List<List<string>> informacije)
        {
            int i = 0;
            foreach (var zivotinja in zivotinje)
            {
                zivotinja.PregledajZivotinju(informacije[i].ElementAt(0), informacije[i].ElementAt(1), informacije[i].ElementAt(2));
                i++;
            }
        }

        public static bool Praznik(DateTime datum)
        {
            List<List<int>> praznici = new List<List<int>>()
            {
                new List<int>() { 01, 01 },
                new List<int>() { 01, 03 },
                new List<int>() { 01, 05 },
                new List<int>() { 25, 11 },
                new List<int>() { 31, 12 }
            };

            List<int> dan = new List<int>()
            { datum.Day, datum.Month };

            return praznici.Find(datum => datum[0] == dan[0] && datum[1] == dan[1]) != null;
        }

        #endregion
    }
}
