using System.Threading.Tasks;

namespace Coolector.Services.Mailing.Services
{
    public interface ISendGridClient
    {
        Task SendMessageAsync(SendGridEmailMessage message);
    }
}