using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using _0601DrustvenaMreza.Model;
using Microsoft.Data.Sqlite;

namespace _0601DrustvenaMreza.Repository
{
    public class GrupaDbRepository
    {
        private readonly string connectionString;
        public GrupaDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
        // GET WITH USERS
        public List<Grupa> GetWithUsers()
        {
            List<Grupa> grupe = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT 
                                    g.Id AS IdGrupe,
                                    g.Ime AS ImeGrupe,
                                    g.DatumOsnivanja AS DatumOsnivanja,
                                    k.Id AS IdKorisnika,
                                    k.KorIme AS KorisnickoIme,
                                    k.Ime AS ImeKorisnika,
                                    k.Prezime AS PrezimeKorisnika,
                                    k.DatumRodjenja AS DatumRodjenja
                                FROM Grupe g
                                LEFT JOIN GrupaKorisnici gk ON g.Id = gk.GrupaId
                                LEFT JOIN Korisnici k ON gk.KorisnikId = k.Id
                                ORDER BY g.Id ASC;";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                Grupa currentGrupa = null;

                while(reader.Read())
                {
                    int grupaId = Convert.ToInt32(reader["IdGrupe"]);

                    if (currentGrupa == null || currentGrupa.Id != grupaId)
                    {
                        int id = grupaId;
                        string ime = reader["ImeGrupe"].ToString();
                        string datumOsnivanjaString = reader["DatumOsnivanja"].ToString();
                        DateTime datumOsnivanja = DateTime.ParseExact(datumOsnivanjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        currentGrupa = new Grupa(id,ime, datumOsnivanja);   
                        grupe.Add(currentGrupa);
                    }

                    if (reader["IdKorisnika"] != DBNull.Value)
                    {
                        int id = Convert.ToInt32(reader["IdKorisnika"]);
                        string korIme = reader["KorisnickoIme"].ToString();
                        string ime = reader["ImeKorisnika"].ToString();
                        string prezime = reader["PrezimeKorisnika"].ToString();
                        string datumRodjenjaString = reader["DatumRodjenja"].ToString();
                        DateTime datumRodjenja = DateTime.ParseExact(datumRodjenjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        Korisnik korisnik = new Korisnik(id, korIme, ime, prezime, datumRodjenja);
                        currentGrupa.korisnici.Add(korisnik);
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

        // GET ALL --> GETPaged
        public List<Grupa> GetPaged(int page, int pageSize)
        {
            List<Grupa> grupe = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Grupe LIMIT @pageSize OFFSET @offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                command.Parameters.AddWithValue("@offset", pageSize * (page - 1));

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
                        grupe.Add(grupa);
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

        // GET BY ID
        public Grupa GetById(int id)
        {
            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                string query = "SELECT * FROM Grupe WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string ime = reader["Ime"].ToString();
                    string datumOsnivanjaString = reader["DatumOsnivanja"].ToString();
                    DateTime datumOsnivanja = DateTime.ParseExact(datumOsnivanjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (!string.IsNullOrWhiteSpace(ime) && !string.IsNullOrWhiteSpace(datumOsnivanjaString))
                    {
                        Grupa grupa = new Grupa(id, ime, datumOsnivanja);
                        return grupa;
                    }

                }
                return null;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        // CREATE
        public int Create(Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Grupe (Ime,DatumOsnivanja) VALUES (@Ime,@DatumOsnivanja); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Ime", grupa.Ime);
                command.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja.ToString("yyyy-MM-dd"));

                int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                return lastInsertedId;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        // UPDATE
        public int Update(int id,Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Grupe SET Ime=@Ime,DatumOsnivanja=@DatumOsnivanja WHERE Id=@Id;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Ime", grupa.Ime);
                command.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja.ToString("yyyy-MM-dd"));

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        // DELETE
        public int Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Grupe WHERE Id=@Id;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        // COUNT ALL
        public int CountAll()
        {
            List<Group> grupe = new List<Group>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Grupe";
                using SqliteCommand command = new SqliteCommand(query, connection);
                int totalCount = Convert.ToInt32(command.ExecuteScalar());

                return totalCount;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

    }
}
