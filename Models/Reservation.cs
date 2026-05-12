using System.ComponentModel.DataAnnotations;

namespace ReservationHotelProjet.Models;

public class Reservation
{
    public int IdReservation { get; set; }

    [Required(ErrorMessage = "La date d'arrivée est obligatoire")]
    public DateTime DateDebut { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La date de départ est obligatoire")]
    public DateTime DateFin { get; set; }= DateTime.Today.AddDays(1);
    
    public string Statut { get; set; } = "";
    
    public int IdClient { get; set; }
    
    public int IdChambre { get; set; }
}