namespace _0601DrustvenaMreza.Model
{
    public class Objava
    {
        private int id;

        private string sadrzaj;

        private DateTime datum;

        private Korisnik? korisnik;


        public int Id { get => id; set => id = value; }
        public string Sadrzaj { get => sadrzaj; set => sadrzaj = value; }
        public DateTime Datum { get => datum; set => datum = value; }
        public Korisnik? Korisnik { get => korisnik; set => korisnik = value; }
    }
}
