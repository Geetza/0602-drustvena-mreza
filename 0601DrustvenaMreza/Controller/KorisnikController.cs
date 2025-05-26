using System.Globalization;
using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;


namespace _0601DrustvenaMreza.Controller
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        private KorisnikDbRepo korisnikDbRepo = new KorisnikDbRepo();

        
        [HttpGet]
        public ActionResult<List<Korisnik>> GetUsers()
        {
            List<Korisnik> korisnici = korisnikDbRepo.GetAll();
            try
            {
                if (korisnici == null || korisnici.Count == 0)
                {
                    return NotFound();
                }
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                return StatusCode(500);
            }

        }


        
        




        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            

            try
            {
                Korisnik korisnik = korisnikDbRepo.GetById(korisnikId);
                if (korisnik == null)
                {
                    return NotFound();
                }
                return Ok(korisnik);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) || noviKorisnik.DatumRodjenja == DateTime.MinValue)
            {
                return BadRequest();
            }

            noviKorisnik.Id = korisnikDbRepo.InsertNewUser(noviKorisnik);
            if (noviKorisnik.Id == 0)
            {
                return BadRequest();
            }

            return Ok(noviKorisnik);
        }

        //private int IzracunajNoviId(List<int> identifikatori)
        //{
        //    int maxId = 0;
        //    foreach (int id in identifikatori)
        //    {
        //        if (id > maxId)
        //        {
        //            maxId = id;
        //        }
        //    }

        //    return maxId + 1;
        //}

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
