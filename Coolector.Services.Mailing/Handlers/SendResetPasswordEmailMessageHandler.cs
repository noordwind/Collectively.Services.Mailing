using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Mailing;
using Coolector.Services.Mailing.Services;
using RawRabbit;

namespace Coolector.Services.Mailing.Handlers
{
    public class SendResetPasswordEmailMessageHandler : ICommandHandler<SendResetPasswordEmailMessage>
    {
        private readonly IBusClient _bus;
        private readonly IEmailMessenger _emailMessenger;

        public SendResetPasswordEmailMessageHandler(IBusClient bus, IEmailMessenger emailMessenger)
        {
            _bus = bus;
            _emailMessenger = emailMessenger;
        }

        public async Task HandleAsync(SendResetPasswordEmailMessage command)
        {
            await _emailMessenger.SendResetPasswordAsync(command.Email,
                command.Endpoint, command.Token, command.Request.Culture);
        }
    }
}