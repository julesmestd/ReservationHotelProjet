using MailKit.Net.Smtp;
using MimeKit;

namespace ReservationHotelProjet.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void EnvoyerConfirmation(string destinataire, string prenom,
        int numeroChambre, string typeChambre,
        DateTime dateDebut, DateTime dateFin)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Hôtel", _config["Email:From"]));
        message.To.Add(new MailboxAddress(prenom, destinataire));
        message.Subject = "Confirmation de votre réservation";

        message.Body = new TextPart("plain")
        {
            Text = $"Bonjour {prenom},\n\n" +
                   $"Votre réservation est confirmée !\n\n" +
                   $"Chambre : {numeroChambre} ({typeChambre})\n" +
                   $"Arrivée  : {dateDebut.ToString("dd/MM/yyyy")}\n" +
                   $"Départ   : {dateFin.ToString("dd/MM/yyyy")}\n\n" +
                   $"Merci de votre confiance.\n" +
                   $"L'équipe de l'hôtel"
        };

        using var smtp = new SmtpClient();
        smtp.Connect(_config["Email:Host"], int.Parse(_config["Email:Port"]!), false);
        smtp.Send(message);
        smtp.Disconnect(true);
    }
}