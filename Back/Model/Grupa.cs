namespace _0601DrustvenaMreza.Model
{
    public class Grupa
    {
        private int id;

        private string ime;

        private DateTime datumOsnivanja;

        public List <Korisnik> korisnici = new List<Korisnik>();

        public int Id { get => id; set => id = value; }
        public string Ime { get => ime; set => ime = value; }
        public DateTime DatumOsnivanja { get => datumOsnivanja; set => datumOsnivanja = value; }
        public List<Korisnik> Korisnici { get; set; } = new List<Korisnik>();

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            Id = id;
            Ime = ime;
            DatumOsnivanja = datumOsnivanja;
        }
    }
}
