using System.Threading.Tasks;

namespace Coolector.Services.Mailing.Services
{
    public interface IEmailMessenger
    {
        Task SendPasswordResetAsync(string email, string token);
    }
}