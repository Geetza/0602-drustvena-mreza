namespace _0601DrustvenaMreza.Model
{
    public class Korisnik
    {
        private int id;

        private string korIme;

        private string ime;

        private string prezime;

        private DateTime datumRodjenja;

        public int Id { get => id; set => id = value; }
        public string KorIme { get => korIme; set => korIme = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public DateTime DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }

        public Korisnik() { }

        public Korisnik(int id, string korIme, string ime, string prezime, DateTime datumRodjenja)
        {
            Id = id;
            KorIme = korIme;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
        }
    }
}
