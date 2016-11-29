using System;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Mailing;
using Coolector.Services.Mailing.Handlers;
using Coolector.Services.Mailing.Services;
using Machine.Specifications;
using Moq;
using RawRabbit;
using It = Machine.Specifications.It;

namespace Coolector.Services.Mailing.Tests.Specs.Handlers
{
    public abstract class SendResetPasswordEmailMessageHandler_specs
    {
        protected static SendResetPasswordEmailMessageHandler Handler;
        protected static Mock<IBusClient> BusClientMock;
        protected static Mock<IEmailMessenger> EmailMessengerMock;
        protected static SendResetPasswordEmailMessage Command;
        protected static Exception Exception;

        protected static void Initialize()
        {
            BusClientMock = new Mock<IBusClient>();
            EmailMessengerMock = new Mock<IEmailMessenger>();
            Command = new SendResetPasswordEmailMessage
            {
                Email = "test@coolector.com",
                Token = "xyz",
                Endpoint = "set-new-password",
                Request = Request.Create<SendResetPasswordEmailMessage>(Guid.NewGuid(), "origin", "en-gb")
            };
            Handler = new SendResetPasswordEmailMessageHandler(BusClientMock.Object, EmailMessengerMock.Object);
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

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_invoke_send_reset_password_async_on_email_messenger = () =>
        {
            EmailMessengerMock.Verify(x => x.SendResetPasswordAsync(Command.Email,
                Command.Endpoint, Command.Token, Command.Request.Culture), Times.Once);
        };
    }
}