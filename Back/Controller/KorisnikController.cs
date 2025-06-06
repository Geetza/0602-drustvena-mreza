﻿using System.Globalization;
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
        private KorisnikDbRepo korisnikDbRepo;

        public KorisnikController(IConfiguration configuration)
        {
            korisnikDbRepo = new KorisnikDbRepo(configuration);
        }

        // GET api/korisnici?page={page}&pageSize={pageSize}

        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page i pagesize moraju biti veći od nule.");
            }

            List<Korisnik> korisnici = korisnikDbRepo.GetPaged(page, pageSize);
            int totalCount = korisnikDbRepo.CountAll();
            Object result = new
            {
                Data = korisnici,
                TotalCount = totalCount
            };
            try
            {
                if (korisnici == null || korisnici.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
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
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            try
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
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }



        [HttpPut("{korisnikId}")]
        public ActionResult<Korisnik> Update(int korisnikId, [FromBody] Korisnik noviKorisnik)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(noviKorisnik.KorIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                        string.IsNullOrWhiteSpace(noviKorisnik.Prezime) || noviKorisnik.DatumRodjenja == DateTime.Now)
                {
                    return BadRequest();
                }



                int rowsAffected = korisnikDbRepo.UpdateUser(korisnikId, noviKorisnik);

                if (korisnikId <= 0)
                {
                    return NotFound();
                }

                if (rowsAffected == 0)
                {
                    return BadRequest();
                }

                return Ok(noviKorisnik);
            }
            catch (Exception ex)
            {
                return Problem($"Doslo je do greske: {ex.Message}");
            }
        }

        [HttpDelete("{korisnikId}")]
        public ActionResult Delete(int korisnikId)
        {

            try
            {
                int rowsAffected = korisnikDbRepo.DeleteById(korisnikId);
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
