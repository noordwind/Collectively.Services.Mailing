using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Mailing;
using Coolector.Common.Services;
using Coolector.Services.Mailing.Services;
using Coolector.Services.Users.Shared.Events;
using RawRabbit;

namespace Coolector.Services.Mailing.Handlers
{
    public class SendResetPasswordEmailMessageHandler : ICommandHandler<SendResetPasswordEmailMessage>
    {
        private readonly IBusClient _bus;
        private readonly IEmailMessenger _emailMessenger;
        private readonly IHandler _handler;

        public SendResetPasswordEmailMessageHandler(IBusClient bus, IEmailMessenger emailMessenger,
            IHandler handler)
        {
            _bus = bus;
            _emailMessenger = emailMessenger;
            _handler = handler;
        }

        public async Task HandleAsync(SendResetPasswordEmailMessage command)
        {
            await _handler
                .Run(() => _emailMessenger.SendResetPasswordAsync(command.Email,
                    command.Endpoint, command.Token, command.Request.Culture))
                .OnSuccess(() => _bus.PublishAsync(new ResetPasswordInitiated(command.Request.Id, command.Email)))
                .OnCustomError(ex => _bus.PublishAsync(new ResetPasswordRejected(command.Request.Id,
                    ex.Message, ex.Code, command.Email)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, "There was an error while sending reset password email message.");
                    await _bus.PublishAsync(new ResetPasswordRejected(command.Request.Id, ex.Message,
                        OperationCodes.Error, command.Email));
                })
                .ExecuteAsync();
        }
    }
}