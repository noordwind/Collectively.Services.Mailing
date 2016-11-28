using Coolector.Common.Commands.Mailing;
using Coolector.Common.Host;
using Coolector.Services.Mailing.Framework;

namespace Coolector.Services.Mailing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10020)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<SendResetPasswordEmailMessage>()
                .Build()
                .Run();
        }
    }
}
