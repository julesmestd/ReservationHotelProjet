using Npgsql;
using ReservationHotelProjet.Extensions;
using ReservationHotelProjet.Models;

namespace ReservationHotelProjet.Services;

public class ChambreService
{
    private readonly NpgsqlConnection _connection;

    public ChambreService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public Chambre? GetById(int id)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = "Select * from Chambre where id_chambre = @id";

        cmd.AddParameter("@id",id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Chambre
            {
                IdChambre   = reader.GetInt32(0),
                Numero      = reader.GetInt32(1),
                Type        = reader.GetString(2),
                Description = reader.GetString(3),
                Prix        = reader.GetDecimal(4)
            };
        }
        return null;
    }
    
    public List<Chambre> GetFiltrees(decimal? prixMax, DateTime? dateDebut, DateTime? dateFin)
    {
        var chambres = new List<Chambre>();

        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
    SELECT * FROM Chambre
    WHERE (@prixMax IS NULL OR prix <= @prixMax)
    AND (@dateDebut IS NULL OR @dateFin IS NULL OR id_chambre NOT IN (
        SELECT id_chambre FROM Reservation
        WHERE @dateDebut < dateFin
        AND @dateFin > dateDebut
    ))";
        
        cmd.Parameters.Add(new NpgsqlParameter<decimal?>("@prixMax", prixMax));
        cmd.Parameters.Add(new NpgsqlParameter<DateTime?>("@dateDebut", dateDebut));
        cmd.Parameters.Add(new NpgsqlParameter<DateTime?>("@dateFin", dateFin));

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