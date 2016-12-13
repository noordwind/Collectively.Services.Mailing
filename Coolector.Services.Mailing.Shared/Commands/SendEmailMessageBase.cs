using Coolector.Common.Commands;

namespace Coolector.Services.Mailing.Shared.Commands
{
    public abstract class SendEmailMessageBase : ICommand
    {
        public Request Request { get; set; }
        public string Email { get; set; }
    }
}