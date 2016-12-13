using System;
using Coolector.Common.Commands;
using Coolector.Common.Services;
using Coolector.Services.Mailing.Handlers;
using Coolector.Services.Mailing.Services;
using Coolector.Services.Mailing.Shared.Commands;
using Machine.Specifications;
using Moq;
using RawRabbit;
using It = Machine.Specifications.It;

namespace Coolector.Services.Mailing.Tests.Specs.Handlers
{
    public abstract class SendResetPasswordEmailMessageHandler_specs
    {
        protected static SendResetPasswordEmailMessageHandler CommandHandler;
        protected static Mock<IBusClient> BusClientMock;
        protected static Mock<IEmailMessenger> EmailMessengerMock;
        protected static IHandler Handler;
        protected static SendResetPasswordEmailMessage Command;
        protected static Exception Exception;

        protected static void Initialize()
        {
            BusClientMock = new Mock<IBusClient>();
            EmailMessengerMock = new Mock<IEmailMessenger>();
            Handler = new Handler();
            Command = new SendResetPasswordEmailMessage
            {
                Email = "test@coolector.com",
                Token = "xyz",
                Endpoint = "set-new-password",
                Request = Request.Create<SendResetPasswordEmailMessage>(Guid.NewGuid(), "origin", "en-gb")
            };
            CommandHandler = new SendResetPasswordEmailMessageHandler(BusClientMock.Object,
                EmailMessengerMock.Object, Handler);
        }
    }

    [Subject("SendResetPasswordEmailMessageHandler HandleAsync")]
    public class when_invoking_send_reset_password_email_message_handle_async :
        SendResetPasswordEmailMessageHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
        };

        Because of = () => CommandHandler.HandleAsync(Command).Await();

        It should_invoke_send_reset_password_async_on_email_messenger = () =>
        {
            EmailMessengerMock.Verify(x => x.SendResetPasswordAsync(Command.Email,
                Command.Endpoint, Command.Token, Command.Request.Culture), Times.Once);
        };
    }
}