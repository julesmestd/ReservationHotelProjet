namespace ReservationHotelProjet.Models;

public class Reservation
{
    public int IdReservation { get; set; }
    
    public DateTime DateDebut { get; set; }
    
    public DateTime DateFin { get; set; }
    
    public string Statut { get; set; } = "";
    
    public int IdClient { get; set; }
    
    public int IdChambre { get; set; }
}