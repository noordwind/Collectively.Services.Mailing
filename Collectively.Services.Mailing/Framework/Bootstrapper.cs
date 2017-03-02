using System;
using System.IO;
using System.Reflection;
using Autofac;
using Collectively.Messages.Commands;
using Collectively.Common.Exceptionless;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Common.Nancy;
using Collectively.Common.Security;
using Nancy.Serialization.JsonNet;
using Collectively.Common.RabbitMq;
using Collectively.Common.Services;
using Collectively.Services.Mailing.Repositories;
using Collectively.Services.Mailing.Services;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Configuration;
using NLog;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit.Configuration;

namespace Collectively.Services.Mailing.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#if DEBUG
        public override void Configure(INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(enabled: false, displayErrorTraces: true);
        }
#endif

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);

            container.Update(builder =>
            {
                builder.RegisterType<CustomJsonSerializer>().As<JsonSerializer>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>());
                builder.RegisterInstance(_configuration.GetSettings<SendGridSettings>());
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<DatabaseSeeder>().As<IDatabaseSeeder>();
                builder.RegisterType<SendGridClient>().As<ISendGridClient>();
                builder.RegisterType<SendGridEmailMessenger>().As<IEmailMessenger>();
                builder.RegisterType<EmailTemplateRepository>().As<IEmailTemplateRepository>();
                builder.RegisterType<Handler>().As<IHandler>();
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();

                var assembly = typeof(Startup).GetTypeInfo().Assembly;
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));

                SecurityContainer.Register(builder, _configuration);
                RabbitMqContainer.Register(builder, _configuration.GetSettings<RawRabbitConfiguration>());
            });
            LifetimeScope = container;
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "Collectively", "Service", "Mailing");

                return ctx.Response;
            });
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            if (databaseSettings.Seed)
            {
                var databaseSeeder = container.Resolve<IDatabaseSeeder>();
                databaseSeeder.SeedAsync();
            }
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers",
                    "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            pipelines.SetupTokenAuthentication(container);
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("Collectively.Services.Mailing API has started.");
        }
    }
}