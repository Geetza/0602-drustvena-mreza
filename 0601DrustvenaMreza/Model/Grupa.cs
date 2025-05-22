namespace _0601DrustvenaMreza.Model
{
    public class Grupa
    {
        private int id;

        private string ime;

        private DateTime datumOsnivanja;

        public Dictionary<int, Korisnik> korisnici = new Dictionary<int, Korisnik>();

        public int Id { get => id; set => id = value; }
        public string Ime { get => ime; set => ime = value; }
        public DateTime DatumOsnivanja { get => datumOsnivanja; set => datumOsnivanja = value; }

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            Id = id;
            Ime = ime;
            DatumOsnivanja = datumOsnivanja;
        }
    }
}
