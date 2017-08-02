using System;
using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Mailing;
using Collectively.Messages.Events.Users;
using Collectively.Services.Mailing.Services;
using RawRabbit;

namespace Collectively.Services.Mailing
{
    public class SendActivateAccountEmailMessageHandler : ICommandHandler<SendActivateAccountEmailMessage>
    {
        private readonly IBusClient _bus;
        private readonly IEmailMessenger _emailMessenger;
        private readonly IHandler _handler;

        public SendActivateAccountEmailMessageHandler(IBusClient bus,
            IEmailMessenger emailMessenger,
            IHandler handler)
        {
            _bus = bus;
            _emailMessenger = emailMessenger;
            _handler = handler;
        }

        public async Task HandleAsync(SendActivateAccountEmailMessage command)
        {
            await _handler
                .Run(async () => await _emailMessenger.SendActivateAccountAsync(
                    command.Email, command.Endpoint, command.Token, command.Request.Culture))
                .OnSuccess(async () => await _bus
                    .PublishAsync(new ActivateAccountInitiated(command.Request.Id, command.Email)))
                .OnCustomError(async ex => await _bus
                    .PublishAsync(new ActivateAccountRejected(command.Request.Id, command.Email, ex.Code, ex.Message)))
                .OnError(async (ex, logger) =>
                {
                    var message = "There was an error while sending activate account email message";
                    logger.Error(ex, message);
                    await _bus.PublishAsync(new ActivateAccountRejected(command.Request.Id, command.Email,
                        OperationCodes.Error, message));
                })
                .ExecuteAsync();
        }
    }
}