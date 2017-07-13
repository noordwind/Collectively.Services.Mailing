using System;
using Collectively.Messages.Commands;
using Collectively.Common.Services;
using Collectively.Services.Mailing.Handlers;
using Collectively.Services.Mailing.Services;
using Collectively.Messages.Commands.Mailing;
using Machine.Specifications;
using Moq;
using RawRabbit;
using It = Machine.Specifications.It;

namespace Collectively.Services.Mailing.Tests.Specs.Handlers
{
    public abstract class SendResetPasswordEmailMessageHandler_specs
    {
        protected static SendResetPasswordEmailMessageHandler CommandHandler;
        protected static Mock<IBusClient> BusClientMock;
        protected static Mock<IEmailMessenger> EmailMessengerMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static IHandler Handler;
        protected static SendResetPasswordEmailMessage Command;
        protected static Exception Exception;

        protected static void Initialize()
        {
            BusClientMock = new Mock<IBusClient>();
            EmailMessengerMock = new Mock<IEmailMessenger>();
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            Command = new SendResetPasswordEmailMessage
            {
                Email = "test@collectively.com",
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