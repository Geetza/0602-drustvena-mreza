using System.Globalization;
using _0601DrustvenaMreza.Model;

namespace _0601DrustvenaMreza.Repository
{
    public class KorisnikRepo
    {
        private const string filePath = "data/korisnici.csv";
        private const string commonFilePath = "data/clanstva.csv";

        public static Dictionary<int, Korisnik> Data;

        public KorisnikRepo()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Korisnik>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string korIme = attributes[1];
                string ime = attributes[2];
                string prezime = attributes[3];
                string datumString = attributes[4];
                DateTime datumRodjenja = DateTime.ParseExact(datumString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Korisnik korisnik = new Korisnik(id, korIme, ime, prezime, datumRodjenja);
                Data[id] = korisnik;
                DodeliKorisnikaGrupama(korisnik);
            }
        }

        private void DodeliKorisnikaGrupama(Korisnik korisnik)
        {
            string[] linije = File.ReadAllLines(commonFilePath);
            foreach (string linija in linije)
            {
                string[] podaci = linija.Split(',');
                int idKorisnika = int.Parse(podaci[0]);
                int idGrupe = int.Parse(podaci[1]);
                foreach (Grupa grupa in GrupaRepo.Data.Values)
                {
                    if (idKorisnika == korisnik.Id && idGrupe == grupa.Id)
                    {
                        grupa.korisnici[idKorisnika] = korisnik;
                    }
                }
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Korisnik k in Data.Values)
            {
                lines.Add($"{k.Id},{k.KorIme},{k.Ime},{k.Prezime},{k.DatumRodjenja.ToString("yyyy-MM-dd")}");
            }
            File.WriteAllLines(filePath, lines);
        }

        public void SaveInCommonFile()
        {
            List<string> lines = new List<string>();
            foreach (Korisnik k in Data.Values)
            {
                foreach (Grupa g in GrupaRepo.Data.Values)
                {
                    if (g.korisnici.ContainsKey(k.Id))
                        lines.Add($"{k.Id},{g.Id}");
                }
            }
            File.WriteAllLines(commonFilePath, lines);
        }
    }
}
