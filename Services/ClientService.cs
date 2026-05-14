using Npgsql;
using ReservationHotelProjet.Extensions;
using ReservationHotelProjet.Models;

namespace ReservationHotelProjet.Services;

public class ClientService
{
    private readonly NpgsqlConnection _connection;

    public ClientService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public Client? GetByEmail(string email)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Client WHERE email = @email";
        cmd.AddParameter("@email", email);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Client
            {
                IdClient     = reader.GetInt32(0),
                Nom          = reader.GetString(1),
                Prenom       = reader.GetString(2),
                Email        = reader.GetString(3),
                PasswordHash = reader.GetString(4),
                Role         = reader.GetString(5)
            };
        }

        return null;
    }

    public void Create(string nom, string prenom, string email, string passwordHash, string role = "Client")
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO Client (nom, prenom, email, passwordHash, role)
                            VALUES (@nom, @prenom, @email, @passwordHash, @role)";

        cmd.AddParameter("@nom", nom);
        cmd.AddParameter("@prenom", prenom);
        cmd.AddParameter("@email", email);
        cmd.AddParameter("@passwordHash", passwordHash);
        cmd.AddParameter("@role", role);

        cmd.ExecuteNonQuery();
    }
    
    public Client? GetById(int id)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Client WHERE id_client = @id";
        cmd.AddParameter("@id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Client
            {
                IdClient     = reader.GetInt32(0),
                Nom          = reader.GetString(1),
                Prenom       = reader.GetString(2),
                Email        = reader.GetString(3),
                PasswordHash = reader.GetString(4),
                Role         = reader.GetString(5)
            };
        }

        return null;
    }
}