using System;
using System.Threading.Tasks;

namespace Collectively.Services.Mailing.Services
{
    public interface IEmailMessenger
    {
        Task SendSupportAsync(string email, string name, string title, string message);
        Task SendResetPasswordAsync(string email, string endpoint, string token, string culture);

        Task SendActivateAccountAsync(string email, string username,
            string endpoint, string token, string culture);

        Task SendRemarkCreatedAsync(string email, Guid remarkId, 
            string category, string address, string username,
            DateTime date, string culture, string url);

        Task SendRemarkStateChangedAsync(string email, Guid remarkId,
            string category, string address, string username,
            DateTime date, string culture, string url, string state);

        Task SendCommentAddedToRemarkAsync(string email, Guid remarkId,
            string category, string address, string username,
            DateTime date, string culture, string url, string comment);

        Task SendPhotosAddedToRemarkEmailAsync(string email, Guid remarkId,
            string category, string address, string culture, string url);
    }
}