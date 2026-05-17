using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ReservationsModel : PageModel
{
    private readonly ReservationService _reservationService;
    private readonly ChambreService _chambreService;
    private readonly ClientService _clientService;

    public List<Reservation> EnCours { get; set; } = new();
    public List<Reservation> Terminees { get; set; } = new();
    public Dictionary<int, Chambre> Chambres { get; set; } = new();
    public Dictionary<int, Client> Clients { get; set; } = new();

    public ReservationsModel(ReservationService reservationService, ChambreService chambreService, ClientService clientService)
    {
        _reservationService = reservationService;
        _chambreService = chambreService;
        _clientService = clientService;
    }

    public void OnGet()
    {
        var toutes = _reservationService.GetAll();

        EnCours = toutes.Where(r => r.DateFin >= DateTime.Today).ToList();
        Terminees = toutes.Where(r => r.DateFin < DateTime.Today).ToList();
        
        foreach (var r in toutes)
        {
            if (!Chambres.ContainsKey(r.IdChambre))
                Chambres[r.IdChambre] = _chambreService.GetById(r.IdChambre)!;

            if (!Clients.ContainsKey(r.IdClient))
                Clients[r.IdClient] = _clientService.GetById(r.IdClient)!;
        }
    }

    public IActionResult OnPostDelete(int id)
    {
        _reservationService.Delete(id);
        TempData["Message"] = "Réservation supprimée avec succès";
        return RedirectToPage();
    }
}