using System.Threading.Tasks;
using Collectively.Services.Mailing.Domain;

namespace Collectively.Services.Mailing.Services
{
    public interface ISendGridClient
    {
        Task SendMessageAsync(SendGridEmailMessage message);
    }
}