using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Models;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Reservations;

[Authorize]
public class EditModel : PageModel
{
    private readonly ReservationService _reservationService;
    private readonly ChambreService _chambreService;

    [BindProperty]
    public Reservation Reservation { get; set; } = new();

    public Chambre Chambre { get; set; } = new();

    public EditModel(ReservationService reservationService, ChambreService chambreService)
    {
        _reservationService = reservationService;
        _chambreService = chambreService;
    }

    public IActionResult OnGet(int id)
    {
        var reservation = _reservationService.GetById(id);

        if (reservation == null)
            return NotFound();
        
        var idClient = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (reservation.IdClient != idClient)
            return Forbid();

        Reservation = reservation;
        Chambre = _chambreService.GetById(reservation.IdChambre)!;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        var idClient = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (Reservation.DateDebut < DateTime.Today)
            ModelState.AddModelError("Reservation.DateDebut", "La date d'arrivée doit être dans le futur");

        if (Reservation.DateFin <= Reservation.DateDebut)
            ModelState.AddModelError("Reservation.DateFin", "La date de départ doit être après la date d'arrivée");

        if (!ModelState.IsValid)
        {
            Chambre = _chambreService.GetById(Reservation.IdChambre)!;
            return Page();
        }

        Reservation.IdReservation = id;
        Reservation.IdClient = idClient;

        _reservationService.Update(Reservation);

        TempData["Message"] = "Réservation modifiée avec succès !";
        return RedirectToPage("/Reservations/MesReservations");
    }
}