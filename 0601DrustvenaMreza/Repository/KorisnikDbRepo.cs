using _0601DrustvenaMreza.Model;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace _0601DrustvenaMreza.Repository
{
    public class KorisnikDbRepo
    {

        public KorisnikDbRepo() { }

        public List<Korisnik> GetAll()
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
                    }
                    else
                    {
                        return korisnici;
                    }

                }

                return korisnici;
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

        public Korisnik GetById(int idKorisnika)


        {


            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using var connection = new SqliteConnection($"Data Source={dbPath}");

                if (!System.IO.File.Exists(dbPath))
                {
                    throw new FileNotFoundException($"Baza podataka ne postoji na putanji: {dbPath}");
                }

                connection.Open();

                string query = "SELECT * FROM Korisnici WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", idKorisnika);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())

                {
                    string korIme = reader["KorIme"]?.ToString();
                    string ime = reader["Ime"]?.ToString();
                    string prezime = reader["Prezime"]?.ToString();
                    string datumRodjenjaString = reader["DatumRodjenja"].ToString();
                    DateTime datumRodjenja = DateTime.ParseExact(datumRodjenjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    return new Korisnik(idKorisnika, korIme, ime, prezime, datumRodjenja);
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


        public int InsertNewUser(Korisnik korisnik)
        {
            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using var connection = new SqliteConnection($"Data Source={dbPath}");

                if (!System.IO.File.Exists(dbPath))
                {
                    throw new FileNotFoundException($"Baza podataka ne postoji na putanji: {dbPath}");
                }

                connection.Open();

                
                string query = "INSERT INTO Korisnici (KorIme,Ime, Prezime,DatumRodjenja) VALUES (@KorIme,@Ime,@Prezime,@DatumRodjenja); SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);

                
                command.Parameters.AddWithValue("@KorIme", korisnik.KorIme);
                command.Parameters.AddWithValue("@Ime", korisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", korisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                
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

        public int UpdateUser(int id,Korisnik noviKorisnik)
        {
            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using var connection = new SqliteConnection($"Data Source={dbPath}");

                if (!System.IO.File.Exists(dbPath))
                {
                    throw new FileNotFoundException($"Baza podataka ne postoji na putanji: {dbPath}");
                }

                connection.Open();


                string query = "Update Korisnici SET KorIme=@KorIme,Ime=@Ime,Prezime=@Prezime,DatumRodjenja=@DatumRodjenja  WHERE id = @Id; SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);

                
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@KorIme", noviKorisnik.KorIme);
                command.Parameters.AddWithValue("@Ime", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));


                int affectedRows = command.ExecuteNonQuery(); 
                return affectedRows;

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

        public int DeleteById(int id)
        {
            try
            {
                string dbPath = Path.Combine("database", "mydatabase.db");
                using var connection = new SqliteConnection($"Data Source={dbPath}");

                if (!System.IO.File.Exists(dbPath))
                {
                    throw new FileNotFoundException($"Baza podataka ne postoji na putanji: {dbPath}");
                }

                connection.Open();


                string query = "DELETE FROM Korisnici WHERE id=@Id; SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);


                command.Parameters.AddWithValue("@Id", id);
               


                int affectedRows = command.ExecuteNonQuery();
                return affectedRows;

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
