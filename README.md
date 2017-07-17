# Collectively.Services.Mailing

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Collectively.Services.Mailing.svg?branch=master)](https://travis-ci.org/noordwind/Collectively.Services.Mailing)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Collectively.Services.Mailing.svg?branch=develop)](https://travis-ci.org/noordwind/Collectively.Services.Mailing/branches)

**Let's go for the better, Collectively​​.**
----------------

**Collectively** is an open platform to enhance communication between counties and its residents​. It's made as a fully open source & cross-platform solution by [Noordwind](https://noordwind.com).

Find out more at [becollective.ly](http://becollective.ly)

**Collectively.Services.Mailing**
----------------

The **Collectively.Services.Mailing** is a service responsible for sending the email messages such as account verification, notifications, reset password procedures etc.

**Quick start**
----------------

## Docker way

Collectively is built as a set of microservices, therefore the easiest way is to run the whole system using the *docker-compose*.

Clone the [Collectively.Docker](https://github.com/noordwind/Collectively.Docker) repository and run the *start.sh* script:

```
git clone https://github.com/noordwind/Collectively.Docker
./starth.sh
```
For the list of available services and their endpoints [click here](https://github.com/noordwind/Collectively).

## Classic way

In order to run the **Collectively.Services.Mailing** you need to have installed:
- [.NET Core](https://dotnet.github.io)
- [MongoDB](https://www.mongodb.com)
- [RabbitMQ](https://www.rabbitmq.com)

Clone the repository and start the application via *dotnet run* command:

```
git clone https://github.com/noordwind/Collectively.Services.Mailing
cd Collectively.Services.Mailing/Collectively.Services.Mailing
dotnet restore --source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/collectively/api/v3/index.json --no-cache
dotnet run --urls "http://*:10006"
```

Once executed, you shall be able to access the service at [http://localhost:10006](http://localhost:10006)

Please note that the following solution will only run the Mailing Service which is merely one of the many parts required to run properly the whole Collectively system.

**Configuration**
----------------

Please edit the *appsettings.json* file in order to use the custom application settings. To configure the docker environment update the *dockerfile* - if you would like to change the exposed port, you need to also update it's value that can be found within *Program.cs*.
For the local testing purposes the *.local* or *.docker* configuration files are being used (for both *appsettings* and *dockerfile*), so feel free to create or edit them.

**Tech stack**
----------------
- **[.NET Core](https://dotnet.github.io)** - an open source & cross-platform framework for building applications using C# language.
- **[Nancy](http://nancyfx.org)** - an open source framework for building HTTP API.
- **[MongoDB](https://github.com/mongodb/mongo-csharp-driver)** - an open source library for integration with [MongoDB](https://www.mongodb.com) database.
- **[RawRabbit](https://github.com/pardahlman/RawRabbit)** - an open source library for integration with [RabbitMQ](https://www.rabbitmq.com) service bus.

**Solution structure**
----------------
- **Collectively.Services.Mailing** - core and executable project via *dotnet run* command.
- **Collectively.Services.Mailing.Shared** - shared package containing events, commands, DTOs & operation codes.
- **Collectively.Services.Mailing.Tests** - unit & integration tests executable via *dotnet test* command.
- **Collectively.Services.Mailing.Tests.EndToEnd** - End-to-End tests executable via *dotnet test* command.

