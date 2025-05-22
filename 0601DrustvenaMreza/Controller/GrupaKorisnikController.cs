using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _0601DrustvenaMreza.Controller
{
    [Route("api/grupe/{grupaId}/korisnici")]
    [ApiController]
    public class GrupaKorisnikController : ControllerBase
    {
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        [HttpGet]
        public ActionResult<List<Korisnik>> Get(int grupaId)
        {
            if (!GrupaRepo.Data.ContainsKey(grupaId))
            {
                return NotFound();
            }

            List<Korisnik> sviKorisnici = KorisnikRepo.Data.Values.ToList();
            List<Korisnik> korisniciGrupe = new List<Korisnik>();
            Grupa grupa = GrupaRepo.Data[grupaId];
            foreach (Korisnik korisnik in sviKorisnici)
            {
                if (grupa != null && grupa.korisnici.ContainsKey(korisnik.Id))
                {
                    korisniciGrupe.Add(korisnik);
                }
            }

            return Ok(korisniciGrupe);
        }

        // Ubaciti korisnika u grupu
        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik> Put(int grupaId,int korisnikId)
        {
            if (!GrupaRepo.Data.ContainsKey(grupaId))
            {
                return NotFound("Grupa nije pronađena");
            }

            if (!KorisnikRepo.Data.ContainsKey(korisnikId))
            {
                return NotFound("Korisnik nije pronađen");
            }

            Korisnik korisnik = KorisnikRepo.Data[korisnikId];
            
            Grupa grupa = GrupaRepo.Data[grupaId];

            if (grupa != null && grupa.korisnici.ContainsKey(korisnik.Id))
            {
                return BadRequest("Korisnik je već član grupe");
            }

            grupa.korisnici[korisnik.Id] = korisnik;
            korisnikRepo.SaveInCommonFile();
            return Ok(korisnik);
        }

        // Izbaciti korisnika iz grupe
        [HttpDelete("{korisnikId}")]
        public ActionResult Delete(int grupaId, int korisnikId)
        {
            if (!GrupaRepo.Data.ContainsKey(grupaId))
            {
                return NotFound("Grupa nije pronađena");
            }

            if (!KorisnikRepo.Data.ContainsKey(korisnikId))
            {
                return NotFound("Korisnik nije pronađen");
            }

            Korisnik korisnik = KorisnikRepo.Data[korisnikId];
            Grupa grupa = GrupaRepo.Data[grupaId];
            if (grupa == null || !grupa.korisnici.ContainsKey(korisnik.Id))
            {
                return BadRequest("Korisnik nije član grupe");
            }

            grupa.korisnici.Remove(korisnik.Id);
            korisnikRepo.SaveInCommonFile();
            return NoContent();
        }
    }
}
