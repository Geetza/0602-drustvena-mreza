using System.Globalization;
using _0601DrustvenaMreza.Model;
using _0601DrustvenaMreza.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using static System.Reflection.Metadata.BlobBuilder;

namespace _0601DrustvenaMreza.Controller
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        private readonly GrupaDbRepository grupaRepo;

        public GrupaController(IConfiguration configuration)
        {
            grupaRepo = new GrupaDbRepository(configuration);
        }

        // GET WITH USERS
        [HttpGet("sa-korisnicima")]
        public ActionResult<List<Grupa>> GetAll()
        {
            try
            {
                List<Grupa> grupe = grupaRepo.GetWithUsers();
                return Ok(grupe);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }

        }

        // GET
        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest();
            }
            try
            {
                List<Grupa> grupeIzDB = grupaRepo.GetPaged(page, pageSize);
                int totalCount = grupaRepo.CountAll();
                Object result = new
                {
                    Data = grupeIzDB,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Grupa> GetGroupById(int id)
        {
            try
            {
                Grupa grupaIzDB = grupaRepo.GetById(id);
                if (grupaIzDB == null)
                {
                    return NotFound();
                }
                return Ok(grupaIzDB);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
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

                novaGrupa.Id = grupaRepo.Create(novaGrupa);
                if (novaGrupa.Id == 0)
                {
                    return BadRequest();
                }

                return Ok(novaGrupa);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
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

                int rowsAffected = grupaRepo.Update(id, novaGrupa);
                if (rowsAffected == 0)
                {
                    return BadRequest();
                }

                return Ok(novaGrupa);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteGroup(int id)
        {
            try
            {
                int rowsAffected = grupaRepo.Delete(id);
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
