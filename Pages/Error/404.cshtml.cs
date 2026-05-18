using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReservationHotelProjet.Pages.Error;

[AllowAnonymous]
public class NotFoundModel : PageModel
{
    public void OnGet() { }
}