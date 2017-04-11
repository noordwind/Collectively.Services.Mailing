using System;
using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Mailing;
using Collectively.Services.Mailing.Services;

namespace Collectively.Services.Mailing.Handlers
{
    public class SendRemarkCreatedEmailMessageHandler : ICommandHandler<SendRemarkCreatedEmailMessage>
    {
        private readonly IHandler _handler;
        private readonly IEmailMessenger _emailMessenger;

        public SendRemarkCreatedEmailMessageHandler(IHandler handler,
            IEmailMessenger emailMessenger)
        {
            _handler = handler;
            _emailMessenger = emailMessenger;
        }

        public async Task HandleAsync(SendRemarkCreatedEmailMessage command)
        {
            await _handler
                .Run(async () =>
                {
                    await _emailMessenger.SendRemarkCreatedAsync(command.Email, command.RemarkId,
                        command.Category, command.Address, command.Username, command.Date,
                        command.Culture);
                })
                .OnError((ex, logger) => logger.Error($"Error while handling {command.GetType().Name} command"))
                .ExecuteAsync();
        }
    }
}