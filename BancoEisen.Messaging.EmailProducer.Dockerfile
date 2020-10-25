FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BancoEisen.Messaging.EmailProducer/BancoEisen.Messaging.EmailProducer.csproj", "BancoEisen.Messaging.EmailProducer/"]
RUN dotnet restore "BancoEisen.Messaging.EmailProducer/BancoEisen.Messaging.EmailProducer.csproj"
COPY . .
WORKDIR "/src/BancoEisen.Messaging.EmailProducer"
RUN dotnet build "BancoEisen.Messaging.EmailProducer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancoEisen.Messaging.EmailProducer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoEisen.Messaging.EmailProducer.dll"]