using Coolector.Common.Host;
using Coolector.Services.Mailing.Framework;
using Coolector.Services.Mailing.Shared.Commands;

namespace Coolector.Services.Mailing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10005)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<SendResetPasswordEmailMessage>()
                .Build()
                .Run();
        }
    }
}
