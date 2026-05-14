using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Helpers;
using ReservationHotelProjet.Services;

namespace ReservationHotelProjet.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ClientService _clientService;

    public LoginModel(ClientService clientService)
    {
        _clientService = clientService;
    }

    [BindProperty]
    [Required(ErrorMessage = "L'email est obligatoire")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = "";

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _clientService.GetByEmail(Email);

        if (client is null || !PasswordHelper.VerifyPassword(Password, client.PasswordHash))
        {
            ModelState.AddModelError("", "Email ou mot de passe incorrect");
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, client.IdClient.ToString()),
            new Claim(ClaimTypes.Name, $"{client.Prenom} {client.Nom}"),
            new Claim(ClaimTypes.Email, client.Email),
            new Claim(ClaimTypes.Role, client.Role),
        };

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        return RedirectToPage("/Index");
    }
}