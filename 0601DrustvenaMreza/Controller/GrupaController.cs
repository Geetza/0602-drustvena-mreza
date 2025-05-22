using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _0601DrustvenaMreza.Controller
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        // GET
        [HttpGet]
        public ActionResult<List<Grupa>> GetGroup()
        {
            List<Grupa> grupe = GrupaRepo.Data.Values.ToList();
            return Ok(grupe);

        }



        // POST
        [HttpPost]
        public ActionResult<Grupa> CreateGroup([FromBody] Grupa novaGrupa)
        {
            if (string.IsNullOrWhiteSpace(novaGrupa.Ime))
            {
                return BadRequest();
            }

            novaGrupa.Id = IzracunajNoviId(GrupaRepo.Data.Keys.ToList());
            novaGrupa.DatumOsnivanja = DateTime.Now;
            GrupaRepo.Data[novaGrupa.Id] = novaGrupa;
            grupaRepo.Save();

            return Ok(novaGrupa);
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

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteGroup(int id)
        {
            if (!GrupaRepo.Data.Keys.Contains(id))
            {
                return NotFound("Grupa pod unetim Id-em ne postoji");
            }

            GrupaRepo.Data.Remove(id);
            grupaRepo.Save();

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
