using System.Threading.Tasks;
using Coolector.Services.Mailing.Domain;

namespace Coolector.Services.Mailing.Services
{
    public interface ISendGridClient
    {
        Task SendMessageAsync(SendGridEmailMessage message);
    }
}