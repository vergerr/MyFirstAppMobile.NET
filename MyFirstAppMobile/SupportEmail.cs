using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace MyFirstAppMobile
{
    public static class SupportEmail
    {
        public static async Task<bool> TrySendLogsAsync(string supportEmail)
        {
            var logPath = Path.Combine(FileSystem.AppDataDirectory, "app.log");

            var body =
    $@"Bonjour,

Décrivez ce que vous faisiez au moment du bug :
- Action :
- Résultat attendu :
- Résultat obtenu :

Infos :
- App version : {AppInfo.VersionString}
- OS : {DeviceInfo.Platform} {DeviceInfo.VersionString}
- Device : {DeviceInfo.Manufacturer} {DeviceInfo.Model}
";
            ²
            // 1) Vérif “théorique”
            if (!Email.Default.IsComposeSupported)
            {
                await Clipboard.Default.SetTextAsync(body);
                await Shell.Current.DisplayAlert(
                    "Email indisponible",
                    "Aucune application e-mail n'est disponible/configurée sur ce téléphone.\n\nLe rapport a été copié dans le presse-papier.",
                    "OK");
                return false;
            }

            try
            {
                var message = new EmailMessage
                {
                    Subject = "Rapport d'erreur - MyFirstAppMobile",
                    Body = body,
                    To = new List<string> { supportEmail }
                };

                if (File.Exists(logPath))
                    message.Attachments.Add(new EmailAttachment(logPath));

                await Email.Default.ComposeAsync(message);
                return true;
            }
            catch (FeatureNotSupportedException)
            {
                // 2) Vérif “réalité terrain”
                await Clipboard.Default.SetTextAsync(body);
                await Shell.Current.DisplayAlert(
                    "Email non supporté",
                    "L'envoi d'e-mail n'est pas supporté sur cet appareil.\n\nLe rapport a été copié dans le presse-papier.",
                    "OK");
                return false;
            }
            catch (Exception ex)
            {
                // 3) Dernier filet de sécurité
                await Clipboard.Default.SetTextAsync(body + "\n\nErreur technique:\n" + ex);
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    "Impossible d'ouvrir l'application e-mail. Le rapport a été copié dans le presse-papier.",
                    "OK");
                return false;
            }
        }
    }
}
