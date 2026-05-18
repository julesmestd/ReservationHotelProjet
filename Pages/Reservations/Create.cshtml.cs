using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Reservations;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ReservationService _reservationService;
    private readonly ChambreService _chambreService;

    [BindProperty]
    public Reservation Reservation { get; set; } = new();

    public Chambre Chambre { get; set; } = new();

    public CreateModel(ReservationService reservationService, ChambreService chambreService)
    {
        _reservationService = reservationService;
        _chambreService = chambreService;
    }

    public void OnGet(int idChambre)
    {
        Chambre = _chambreService.GetById(idChambre)!;
        Reservation.IdChambre = idChambre;
    }

    public IActionResult OnPost(int idChambre)
    {
        var idClient = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (_reservationService.ClientDejaReserve(idClient, Reservation.DateDebut, Reservation.DateFin))
            ModelState.AddModelError("", "Vous avez déjà une réservation sur ces dates");
        
        if (Reservation.DateDebut < DateTime.Today)
            ModelState.AddModelError("Reservation.DateDebut", "La date d'arrivée doit être dans le futur");

        if (Reservation.DateFin <= Reservation.DateDebut)
            ModelState.AddModelError("Reservation.DateFin", "La date de départ doit être après la date d'arrivée");

        if (_reservationService.ChambreDejaReservee(idChambre, Reservation.DateDebut, Reservation.DateFin))
            ModelState.AddModelError("", "Cette chambre n'est pas disponible sur ces dates");
        if (!ModelState.IsValid)
        {
            Chambre = _chambreService.GetById(idChambre)!;
            return Page();
        }

        try
        {
            Reservation.IdChambre = idChambre;
            Reservation.IdClient = idClient;

            _reservationService.Create(Reservation);

            TempData["Message"] = "Réservation confirmée avec succès !";
            return RedirectToPage("/Index");
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Impossible de créer la réservation");
            Chambre = _chambreService.GetById(idChambre)!;
            return Page();
        }
    }
}