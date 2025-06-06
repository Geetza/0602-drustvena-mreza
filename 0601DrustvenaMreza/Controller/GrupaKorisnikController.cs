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

        [HttpPut]
        public ActionResult<int> PutUserIntoGroup(int grupaId, [FromBody] int korisnikId)
        {
            try
            {
                if (korisnikId <= 0)
                {
                    return BadRequest("Nevalidan ID korisnika");
                }

                int lastRowInsertedId = grupaKorisnikRepo.InsertKorisnikInGroup(grupaId,korisnikId);

                if (lastRowInsertedId == 0)
                {
                    return Ok("Korisnik je vec u grupi");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult<int> RemoveUserFromGroup(int grupaId, [FromBody] int korisnikId)
        {
            try
            {
                int rowsAffected = grupaKorisnikRepo.RemoveKorisnikFromGroup(grupaId, korisnikId);
                if (rowsAffected == 0)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }
    }
}
