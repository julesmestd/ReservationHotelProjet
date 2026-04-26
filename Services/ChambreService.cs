using Npgsql;
using ReservationHotelProjet.Models;

namespace ReservationHotelProjet.Services;

public class ChambreService
{
    private readonly NpgsqlConnection _connection;

    public ChambreService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Chambre> GetAll()
    {
        var chambres = new List<Chambre>();

        var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Chambre";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            chambres.Add(new Chambre
            {
                IdChambre   = reader.GetInt32(0),
                Numero      = reader.GetInt32(1),
                Type        = reader.GetString(2),
                Description = reader.GetString(3),
                Prix        = reader.GetDecimal(4)
            });
        }

        return chambres;
    }
}