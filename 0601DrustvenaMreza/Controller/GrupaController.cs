using System.Globalization;
using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace _0601DrustvenaMreza.Controller
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        private GrupaDbRepository grupaDbRepository = new GrupaDbRepository();
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        // GET
        [HttpGet]
        public ActionResult<List<Grupa>> GetGroup()
        {
            List<Grupa> grupeIzDB = grupaDbRepository.GetAll();
            try
            {
                if(grupeIzDB == null || grupeIzDB.Count == 0)
                {
                    return NotFound();
                }
                return Ok(grupeIzDB);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                return StatusCode(500);
            }
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Grupa> GetGroupById(int id)
        {
            try
            {
                Grupa grupaIzDB = grupaDbRepository.GetById(id);
                if (grupaIzDB == null)
                {
                    return NotFound();
                }
                return Ok(grupaIzDB);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                return StatusCode(500);
            }
        }

        // CREATE
        [HttpPost]
        public ActionResult<Grupa> CreateGroup([FromBody] Grupa novaGrupa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(novaGrupa.Ime))
                {
                    return BadRequest();
                }
                if (novaGrupa.DatumOsnivanja > DateTime.Now)
                {
                    return BadRequest();
                }

                novaGrupa.Id = grupaDbRepository.Create(novaGrupa);
                if (novaGrupa.Id == 0)
                {
                    return BadRequest();
                }

                return Ok(novaGrupa);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                return StatusCode(500);
            }
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult<Grupa> UpdateGroup(int id,[FromBody] Grupa novaGrupa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(novaGrupa.Ime))
                {
                    return BadRequest();
                }
                if (novaGrupa.DatumOsnivanja > DateTime.Now)
                {
                    return BadRequest();
                }

                int rowsAffected = grupaDbRepository.Update(id, novaGrupa);
                if (rowsAffected == 0)
                {
                    return BadRequest();
                }

                return Ok(novaGrupa);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                return StatusCode(500);
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteGroup(int id)
        {
            int rowsAffected = grupaDbRepository.Delete(id);
            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/korisnici-van-grupe")]
        public ActionResult<List<Korisnik>> GetKorisnikeVanGrupe(int id)
        {
            List<Korisnik> sviKorisnici = KorisnikRepo.Data.Values.ToList();

            List<Korisnik> korisniciVanGrupe = new List<Korisnik>();

            if (!GrupaRepo.Data.Keys.Contains(id))
            {
                return NotFound("Grupa pod unetim Id-em ne postoji");
            }

            Grupa grupa = GrupaRepo.Data[id];

            
            foreach (Korisnik korisnik in sviKorisnici)
            {
                if (grupa.korisnici.ContainsKey(korisnik.Id))
                {
                    continue;
                } else
                {
                    korisniciVanGrupe.Add(korisnik);
                }
            }

            return Ok(korisniciVanGrupe);
        }

    }
}
