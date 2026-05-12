using Npgsql;
using ReservationHotelProjet.Extensions;
using ReservationHotelProjet.Models;

namespace ReservationHotelProjet.Services;

public class ReservationService
{
    private readonly NpgsqlConnection _connection;

    public ReservationService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public void Create(Reservation reservation)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO Reservation (dateDebut, dateFin, statut, id_chambre, id_client)
                            VALUES (@dateDebut, @dateFin, @statut, @idChambre, @idClient)";

        cmd.AddParameter("@dateDebut", reservation.DateDebut);
        cmd.AddParameter("@dateFin", reservation.DateFin);
        cmd.AddParameter("@statut", reservation.Statut);
        cmd.AddParameter("@idChambre", reservation.IdChambre);
        cmd.AddParameter("@idClient", reservation.IdClient);

        cmd.ExecuteNonQuery();
    }
    
    public bool ChambreDejaReservee(int idChambre, DateTime dateDebut, DateTime dateFin)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM Reservation
                        WHERE id_chambre = @idChambre
                        AND @dateDebut < dateFin
                        AND @dateFin > dateDebut
                        AND statut = 'Confirmée'";

        cmd.AddParameter("@idChambre", idChambre);
        cmd.AddParameter("@dateDebut", dateDebut);
        cmd.AddParameter("@dateFin", dateFin);

        var count = (long)cmd.ExecuteScalar()!;
        return count > 0;
    }
}