using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using _0601DrustvenaMreza.Model;
using Microsoft.Data.Sqlite;

namespace _0601DrustvenaMreza.Repository
{
    public class GrupaDbRepository
    {
        // GET ALL
        public List<Grupa> GetAll()
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
                string dbPath = Path.Combine("database", "mydatabase.db");
                using SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
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
                string dbPath = Path.Combine("database", "mydatabase.db");
                using SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
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
                string dbPath = Path.Combine("database", "mydatabase.db");
                using SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
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

    }
}
