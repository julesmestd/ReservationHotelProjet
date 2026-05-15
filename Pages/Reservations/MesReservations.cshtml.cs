using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Reservations;

[Authorize]
public class MesReservationsModel : PageModel
{
    private readonly ReservationService _reservationService;
    private readonly ChambreService _chambreService;

    public List<Reservation> Reservations { get; set; } = new();
    public Dictionary<int, Chambre> Chambres { get; set; } = new();

    public MesReservationsModel(ReservationService reservationService, ChambreService chambreService)
    {
        _reservationService = reservationService;
        _chambreService = chambreService;
    }

    public void OnGet()
    {
        var idClient = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        Reservations = _reservationService.GetByClient(idClient);
        
        foreach (var r in Reservations)
        {
            if (!Chambres.ContainsKey(r.IdChambre))
                Chambres[r.IdChambre] = _chambreService.GetById(r.IdChambre)!;
        }
    }

    public IActionResult OnPostDelete(int id)
    {
        var reservation = _reservationService.GetById(id);

        if (reservation == null)
            return NotFound();
        
        if (reservation.DateDebut <= DateTime.Today.AddDays(2))
        {
            TempData["Erreur"] = "Impossible d'annuler une réservation moins de 2 jours avant l'arrivée";
            return RedirectToPage();
        }

        _reservationService.Delete(id);
        TempData["Message"] = "Réservation annulée avec succès";
        return RedirectToPage();
    }
}