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
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        // GET
        [HttpGet]
        public ActionResult<List<Grupa>> GetGroup()
        {
            List<Grupa> grupeIzDB = GetAllFromDatabase();
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

        // U okviru kontrolera za grupe izmenite metodu za dobavljanje svih grupa.
        // Umesto kontaktiranja repozitorijuma, metoda poziva privatnu metodu kontrolera (npr. GetAllFromDatabase).
        // Data metoda izvlači sve korisnike iz baze podataka i vraća ih.

        private List<Grupa> GetAllFromDatabase()
        {
            List<Grupa> grupe = new List<Grupa>();
            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                string query = "SELECT * FROM Grupe";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string ime = reader["Ime"].ToString();
                    string datumOsnivanjaString = reader["DatumOsnivanja"].ToString();
                    DateTime datumOsnivanja = DateTime.ParseExact(datumOsnivanjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (!string.IsNullOrWhiteSpace(ime) && !string.IsNullOrWhiteSpace(datumOsnivanjaString))
                    {
                        Grupa grupa = new Grupa(id, ime, datumOsnivanja);
                        grupe.Add( grupa );
                    }
                    
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return grupe;

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
