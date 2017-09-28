using System.Threading.Tasks;
using Collectively.Messages.Commands;
using Collectively.Common.Services;
using Collectively.Services.Mailing.Services;
using Collectively.Messages.Commands.Mailing;
using Collectively.Messages.Events.Users;
using RawRabbit;


namespace Collectively.Services.Mailing.Handlers
{
    public class SendSupportEmailMessageHandler : ICommandHandler<SendSupportEmailMessage>
    {
        private readonly IBusClient _bus;
        private readonly IEmailMessenger _emailMessenger;
        private readonly IHandler _handler;

        public SendSupportEmailMessageHandler(IBusClient bus, IEmailMessenger emailMessenger,
            IHandler handler)
        {
            _bus = bus;
            _emailMessenger = emailMessenger;
            _handler = handler;
        }

        public async Task HandleAsync(SendSupportEmailMessage command)
            => await _handler
                .Run(() => _emailMessenger.SendSupportAsync(command.Email,
                    command.Name, command.Title, command.Message))
                .OnError((ex, logger) => 
                    logger.Error(ex, "There was an error while sending support email message."))
                .ExecuteAsync();
    }
}