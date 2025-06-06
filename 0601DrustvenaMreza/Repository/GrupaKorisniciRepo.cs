using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using _0601DrustvenaMreza.Model;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.Sqlite;

namespace _0601DrustvenaMreza.Repository
{
    public class GrupaKorisniciRepo
    {
        private readonly string connectionString;

        public GrupaKorisniciRepo(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        // GET ALL
        public Grupa GetAllGrpUsers(int id)
        {
            Grupa currentGrupa = null;
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
                                WHERE g.Id=@Id
                                ORDER BY g.Id ASC;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int groupId = Convert.ToInt32(reader["IdGrupe"]);

                    if (currentGrupa == null || currentGrupa.Id != groupId)
                    {
                        string ime = reader["ImeGrupe"].ToString();
                        string datumOsnivanjaString = reader["DatumOsnivanja"].ToString();
                        DateTime datumOsnivanja = DateTime.ParseExact(datumOsnivanjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        currentGrupa = new Grupa(id, ime, datumOsnivanja);
                    }

                    if (reader["IdKorisnika"] != DBNull.Value)
                    {
                        int idKorisnika = Convert.ToInt32(reader["IdKorisnika"]);
                        string korIme = reader["KorisnickoIme"].ToString();
                        string ime = reader["ImeKorisnika"].ToString();
                        string prezime = reader["PrezimeKorisnika"].ToString();
                        string datumRodjenjaString = reader["DatumRodjenja"].ToString();
                        DateTime datumRodjenja = DateTime.ParseExact(datumRodjenjaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        Korisnik korisnik = new Korisnik(idKorisnika, korIme, ime, prezime, datumRodjenja);
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

            return currentGrupa;
        }
    }
}
