using System.Threading.Tasks;

 namespace Collectively.Services.Mailing.Services
 {
     public interface IEmailMessenger
     {
         Task SendResetPasswordAsync(string email, string endpoint, string token, string culture);
     }
 }