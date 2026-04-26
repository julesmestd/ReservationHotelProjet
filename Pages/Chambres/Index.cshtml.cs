using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Chambres;

public class IndexModel : PageModel
{
    
    private readonly ChambreService _chambreService;

    public List<Chambre> Chambres { get; set; } = new();

    public IndexModel(ChambreService chambreService)
    {
        _chambreService = chambreService;
    }
    
    public void OnGet()
    {
        Chambres = _chambreService.GetAll();
    }
}