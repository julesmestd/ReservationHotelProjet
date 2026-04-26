namespace ReservationHotelProjet.Models;

public class Chambre
{
    public int IdChambre { get; set; }
    
    public int Numero { get; set; }
    
    public string Type { get; set; } = "";
    
    public string Description { get; set; } = "";
    
    public decimal Prix { get; set; }
}