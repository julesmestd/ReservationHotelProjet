using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Chambres;

public class DetailsModel : PageModel
{
    private readonly ChambreService _chambreService;

    public Chambre Chambre { get; set; }

    public DetailsModel(ChambreService chambreService)
    {
        _chambreService = chambreService;
    }

    public void OnGet(int id)
    {
        Chambre = _chambreService.GetById(id);
    }
}