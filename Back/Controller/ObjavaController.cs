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
    }
}
