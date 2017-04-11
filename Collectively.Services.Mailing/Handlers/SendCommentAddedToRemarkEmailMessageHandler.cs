using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Mailing;
using Collectively.Services.Mailing.Services;

namespace Collectively.Services.Mailing.Handlers
{
    public class SendCommentAddedToRemarkEmailMessageHandler : ICommandHandler<SendCommentAddedToRemarkEmailMessage>
    {
        private readonly IHandler _handler;
        private readonly IEmailMessenger _emailMessenger;

        public SendCommentAddedToRemarkEmailMessageHandler(IHandler handler,
            IEmailMessenger emailMessenger)
        {
            _handler = handler;
            _emailMessenger = emailMessenger;
        }

        public async Task HandleAsync(SendCommentAddedToRemarkEmailMessage command)
        {
            await _handler
                .Run(async () =>
                {
                    await _emailMessenger.SendCommentAddedToRemarkAsync(command.Email,
                        command.RemarkId, command.Category, command.Address,
                        command.Username, command.Date, command.Culture, command.Comment);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {command.GetType().Name} command"))
                .ExecuteAsync();
        }
    }
}