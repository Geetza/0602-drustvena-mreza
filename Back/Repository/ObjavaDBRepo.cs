using System.Globalization;
using System.Text.RegularExpressions;
using _0601DrustvenaMreza.Model;
using Microsoft.Data.Sqlite;

namespace _0601DrustvenaMreza.Repository
{
    public class ObjavaDBRepo
    {
        private readonly string connectionString;

        public ObjavaDBRepo(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Objava> GetAll()
        {
            List<Objava> objave = new List<Objava>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT 
                                    o.Id ,
                                    o.Sadrzaj,
                                    o.Datum AS DatumObjave,
                                    k.Id AS IdKorisnika,
                                    k.KorIme,
                                    k.Ime,
                                    k.Prezime,
                                    k.DatumRodjenja AS DatumRodjenja
                                FROM Objave o
                                LEFT JOIN Korisnici k ON o.KorisnikId = k.Id;
                                ";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                Objava objava = null;

                while (reader.Read())
                {



                    {
                        objava = new Objava
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Sadrzaj = reader["Sadrzaj"].ToString(),
                            Datum = DateTime.ParseExact(reader["DatumObjave"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        };






                        objave.Add(objava);


                        if (reader["IdKorisnika"] != DBNull.Value)
                        {
                            Korisnik korisnik;
                            int id = Convert.ToInt32(reader["IdKorisnika"]);
                            string korIme = reader["KorIme"]?.ToString();
                            string ime = reader["Ime"]?.ToString();
                            string prezime = reader["Prezime"]?.ToString();
                            string datumRodjenjaString = reader["DatumRodjenja"].ToString();
                            DateTime datumRodjenja = DateTime.ParseExact(datumRodjenjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            korisnik = new Korisnik(id, korIme, ime, prezime, datumRodjenja);
                            objava.Korisnik = korisnik;
                        }
                    }
                }
                return objave;
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

        public Objava Create(Objava objava)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Objave (KorisnikId, Sadrzaj, Datum)  VALUES (@KorisnikId, @Sadrzaj, @Datum); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@KorisnikId", objava.Korisnik.Id);
                command.Parameters.AddWithValue("@Sadrzaj", objava.Sadrzaj);
                command.Parameters.AddWithValue("@Datum", objava.Datum.ToString("yyyy-MM-dd"));

                objava.Id = Convert.ToInt32(command.ExecuteScalar());
                return objava;
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
