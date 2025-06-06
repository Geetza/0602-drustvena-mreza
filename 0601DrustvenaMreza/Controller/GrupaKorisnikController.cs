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
        private readonly GrupaKorisniciRepo grupaKorisnikRepo;

        public GrupaKorisnikController(IConfiguration configuration)
        {
            grupaKorisnikRepo = new GrupaKorisniciRepo(configuration);
        }

        [HttpGet]
        public ActionResult<Grupa> GetAll(int grupaId)
        {
            try
            {
                Grupa grupa = grupaKorisnikRepo.GetAllGrpUsers(grupaId);
                if (grupa == null)
                {
                    return NotFound();
                }
                return Ok(grupa);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }

        }

        
    }
}
