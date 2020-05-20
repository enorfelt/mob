FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
#WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-restore --verbosity normal