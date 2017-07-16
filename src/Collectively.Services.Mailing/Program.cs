using Collectively.Common.Host;
using Collectively.Services.Mailing.Framework;
using Collectively.Messages.Commands.Mailing;

namespace Collectively.Services.Mailing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>()
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<SendResetPasswordEmailMessage>()
                .SubscribeToCommand<SendRemarkCreatedEmailMessage>()
                .SubscribeToCommand<SendRemarkStateChangedEmailMessage>()
                .SubscribeToCommand<SendPhotosAddedToRemarkEmailMessage>()
                .SubscribeToCommand<SendCommentAddedToRemarkEmailMessage>()
                .Build()
                .Run();
        }
    }
}
