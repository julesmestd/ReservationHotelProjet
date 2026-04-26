namespace ReservationHotelProjet.Models;

public class Client
{
    public int IdClient { get; set; }

    public string Nom { get; set; } = "";

    public string Prenom { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";
    
    public string Role { get; set; } = "";
}