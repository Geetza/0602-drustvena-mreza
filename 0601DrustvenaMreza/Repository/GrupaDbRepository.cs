using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using _0601DrustvenaMreza.Model;
using Microsoft.Data.Sqlite;

namespace _0601DrustvenaMreza.Repository
{
    public class GrupaDbRepository
    {
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
        
    }
}
