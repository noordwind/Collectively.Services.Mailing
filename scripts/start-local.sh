#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Services.Mailing
dotnet run --no-restore --urls "http://*:10006"