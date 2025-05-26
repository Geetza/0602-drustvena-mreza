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

        //[HttpGet]
        //public ActionResult<List<Korisnik>> GetAll()
        //{
        //    List<Korisnik> korisnici = KorisnikRepo.Data.Values.ToList();
        //    return Ok(korisnici);
        //}

        // GET
        [HttpGet]
        public ActionResult<List<Korisnik>> GetUsers()
        {
            List<Korisnik> korisnici = GetAllFromDatabase();
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


        [HttpGet]
        private List<Korisnik> GetAllFromDatabase()
        {
                List<Korisnik> korisnici = new List<Korisnik>();
            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                Console.WriteLine(dbPath);

                if (!System.IO.File.Exists(dbPath))
                {
                    throw new FileNotFoundException($"Baza podataka ne postoji na putanji: {dbPath}");
                }

                connection.Open();

                string query = "SELECT * FROM Korisnici";
                using var command = new SqliteCommand(query, connection);
                using var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    Korisnik korisnik;
                    int id = Convert.ToInt32(reader["Id"]);
                    string korIme = reader["KorIme"]?.ToString();
                    string ime = reader["Ime"]?.ToString();
                    string prezime = reader["Prezime"]?.ToString();
                    string datumRodjenjaString = reader["DatumRodjenja"].ToString();
                    DateTime datumRodjenja = DateTime.ParseExact(datumRodjenjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (id != -1 && korIme != null && ime != null && prezime != null && datumRodjenja != DateTime.MinValue)
                    {
                         korisnik = new Korisnik(id, korIme, ime, prezime, datumRodjenja);
                        korisnici.Add(korisnik);
                    } else
                    {
                        return korisnici;
                    }
                    
                }

                return korisnici;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQLite error: {ex.Message}");
                return korisnici;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return korisnici;
            }
        }




        [HttpGet("{korisnikId}")]
        public ActionResult<Korisnik> GetById(int korisnikId)
        {
            if (!KorisnikRepo.Data.ContainsKey(korisnikId))
            {
                return NotFound();
            }

            return Ok(KorisnikRepo.Data[korisnikId]);
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) || noviKorisnik.DatumRodjenja == DateTime.Now)
            {
                return BadRequest();
            }

            noviKorisnik.Id = IzracunajNoviId(KorisnikRepo.Data.Keys.ToList());
            KorisnikRepo.Data[noviKorisnik.Id] = noviKorisnik;
            korisnikRepo.Save();

            return Ok(noviKorisnik);
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
