using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationHotelProjet.Helpers;
using ReservationHotelProjet.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ReservationHotelProjet.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly ClientService _clientService;

    public RegisterModel(ClientService clientService)
    {
        _clientService = clientService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string Nom { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    public string Prenom { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [MinLength(6, ErrorMessage = "Le mot de passe doit faire au moins 6 caractères")]
    public string Password { get; set; } = "";

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();

        if (_clientService.GetByEmail(Email) is not null)
        {
            ModelState.AddModelError("Email", "Cet email est déjà utilisé");
            return Page();
        }

        var hash = PasswordHelper.HashPassword(Password);
        _clientService.Create(Nom, Prenom, Email, hash);
        
        var client = _clientService.GetByEmail(Email)!;
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