using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _0601DrustvenaMreza.Controller
{
    [Route("api")]
    [ApiController]
    public class ObjavaController : ControllerBase
    {
        private readonly KorisnikDbRepo korisnikDbRepo;
        private readonly ObjavaDBRepo objavaDBRepo;

        public ObjavaController(IConfiguration configuration)
        {
            objavaDBRepo = new ObjavaDBRepo(configuration);
            korisnikDbRepo = new KorisnikDbRepo(configuration);
        }

        [HttpGet("objave")]
        public ActionResult<List<Objava>> GetAll()
        {
            try
            {
                List<Objava> objave = objavaDBRepo.GetAll();
                return Ok(objave);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }

        [HttpPost("korisnici/{korisnikId}/objave")]
        public ActionResult<Objava> Create(int korisnikId, [FromBody] Objava objava)
        {
            Korisnik korisnik = korisnikDbRepo.GetById(korisnikId);
            if (korisnik == null)
            {
                return NotFound($"Korisnik sa ID-em : {korisnikId} nije pronađen.");
            }
            objava.Korisnik = korisnik;
            try
            {
                Objava kreiranaObjava = objavaDBRepo.Create(objava);
                return Ok(kreiranaObjava);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }
    }
}
