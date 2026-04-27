using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Chambres;

public class IndexModel : PageModel
{
    
    private readonly ChambreService _chambreService;

    [BindProperty(SupportsGet = true)]
    public decimal? PrixMax { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? DateDebut { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? DateFin { get; set; }

    public List<Chambre> Chambres { get; set; } = new();

    public IndexModel(ChambreService chambreService)
    {
        _chambreService = chambreService;
    }
    
    public void OnGet()
    {
        Chambres = _chambreService.GetFiltrees(PrixMax, DateDebut, DateFin);
    }
}