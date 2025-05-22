using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _0601DrustvenaMreza.Controller
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> korisnici = KorisnikRepo.Data.Values.ToList();
            return Ok(korisnici);
        }

        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            if (!KorisnikRepo.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            return Ok(KorisnikRepo.Data[korisnikId]);
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) || noviKorisnik.DatumRodjenja == DateTime.Now)
            {
                return BadRequest();
            }

            noviKorisnik.Id = IzracunajNoviId(KorisnikRepo.Data.Keys.ToList());
            KorisnikRepo.Data[noviKorisnik.Id] = noviKorisnik;
            korisnikRepo.Save();

            return Ok(noviKorisnik);
        }

        private int IzracunajNoviId(List<int> identifikatori)
        {
            int maxId = 0;
            foreach (int id in identifikatori)
            {
                if (id > maxId)
                {
                    maxId = id;
                }
            }

            return maxId + 1;
        }

        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik> Update(int korisnikId, [FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) || noviKorisnik.DatumRodjenja == DateTime.Now)
            {
                return BadRequest();
            }

            if (!KorisnikRepo.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepo.Data[korisnikId];
            korisnik.KorIme = noviKorisnik.KorIme;
            korisnik.Ime = noviKorisnik.Ime;
            korisnik.Prezime = noviKorisnik.Prezime;
            korisnik.DatumRodjenja = noviKorisnik.DatumRodjenja;
            korisnikRepo.Save();

            return Ok(noviKorisnik);
        }
        [HttpDelete("{korisnikId}")]
        public ActionResult Delete(int korisnikId)
        {
            if (!KorisnikRepo.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }
            KorisnikRepo.Data.Remove(korisnikId);
            korisnikRepo.Save();
            korisnikRepo.SaveInCommonFile();

            return NoContent();
        }
    }
}
