using System;
using System.Globalization;
using _0601DrustvenaMreza.Model;

namespace _0601DrustvenaMreza.Repository
{
    public class GrupaRepo
    {
        private const string filePath = "data/grupe.csv"; 
        public static Dictionary<int, Grupa> Data;

        public GrupaRepo()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Grupa>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string ime = attributes[1];
                string datumString = attributes[2];
                DateTime datumOsnivanja = DateTime.ParseExact(datumString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Grupa grupa = new Grupa(id, ime,datumOsnivanja);
                Data[id] = grupa;
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Grupa g in Data.Values)
            {
                lines.Add($"{g.Id},{g.Ime},{g.DatumOsnivanja.ToString("yyyy-MM-dd")}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
