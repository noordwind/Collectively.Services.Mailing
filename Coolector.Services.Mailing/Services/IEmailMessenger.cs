using System.Threading.Tasks;

 namespace Coolector.Services.Mailing.Services
 {
     public interface IEmailMessenger
     {
         Task SendResetPasswordAsync(string email, string endpoint, string token, string culture);
     }
 }