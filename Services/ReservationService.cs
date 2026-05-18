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
        cmd.CommandText = @"INSERT INTO Reservation (dateDebut, dateFin, id_chambre, id_client)
                            VALUES (@dateDebut, @dateFin, @idChambre, @idClient)";

        cmd.AddParameter("@dateDebut", reservation.DateDebut);
        cmd.AddParameter("@dateFin", reservation.DateFin);
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
                        AND @dateFin > dateDebut";

        cmd.AddParameter("@idChambre", idChambre);
        cmd.AddParameter("@dateDebut", dateDebut);
        cmd.AddParameter("@dateFin", dateFin);

        var count = (long)cmd.ExecuteScalar()!;
        return count > 0;
    }
    
    public List<Reservation> GetByClient(int idClient)
    {
        var reservations = new List<Reservation>();
    
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Reservation
                            WHERE id_client = @idClient 
                            AND dateFin >= @today
                            ORDER BY dateDebut";
    
        cmd.AddParameter("@idClient", idClient);
        cmd.AddParameter("@today", DateTime.Today);
    
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            reservations.Add(new Reservation
            {
                IdReservation = reader.GetInt32(0),
                DateDebut     = reader.GetDateTime(1),
                DateFin       = reader.GetDateTime(2),
                IdChambre     = reader.GetInt32(3),
                IdClient      = reader.GetInt32(4)
            });
        }
    
        return reservations;
    }
    
    public List<Reservation> GetAll()
    {
        var reservations = new List<Reservation>();
    
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Reservation ORDER BY dateDebut";
    
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            reservations.Add(new Reservation
            {
                IdReservation = reader.GetInt32(0),
                DateDebut     = reader.GetDateTime(1),
                DateFin       = reader.GetDateTime(2),
                IdChambre     = reader.GetInt32(3),
                IdClient      = reader.GetInt32(4)
            });
        }
    
        return reservations;
    }
    
    public void Update(Reservation reservation)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"UPDATE Reservation
                            SET dateDebut = @dateDebut, dateFin = @dateFin
                            WHERE id_reservation = @id";
    
        cmd.AddParameter("@dateDebut", reservation.DateDebut);
        cmd.AddParameter("@dateFin", reservation.DateFin);
        cmd.AddParameter("@id", reservation.IdReservation);
    
        cmd.ExecuteNonQuery();
    }
    
    public void Delete(int id)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM Reservation WHERE id_reservation = @id";
        cmd.AddParameter("@id", id);
        cmd.ExecuteNonQuery();
    }
    
    public Reservation? GetById(int id)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Reservation WHERE id_reservation = @id";
        cmd.AddParameter("@id", id);
    
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Reservation
            {
                IdReservation = reader.GetInt32(0),
                DateDebut     = reader.GetDateTime(1),
                DateFin       = reader.GetDateTime(2),
                IdChambre     = reader.GetInt32(3),
                IdClient      = reader.GetInt32(4)
            };
        }
    
        return null;
    }
    
    public bool ClientDejaReserve(int idClient, DateTime dateDebut, DateTime dateFin)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM reservation
                        WHERE id_client = @idClient
                        AND @dateDebut < datefin
                        AND @dateFin > datedebut";

        cmd.AddParameter("@idClient", idClient);
        cmd.AddParameter("@dateDebut", dateDebut);
        cmd.AddParameter("@dateFin", dateFin);

        var count = (long)cmd.ExecuteScalar()!;
        return count > 0;
    }
}