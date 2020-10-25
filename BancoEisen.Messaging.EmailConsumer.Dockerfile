FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BancoEisen.Messaging.EmailConsumer/BancoEisen.Messaging.EmailConsumer.csproj", "BancoEisen.Messaging.EmailConsumer/"]
RUN dotnet restore "BancoEisen.Messaging.EmailConsumer/BancoEisen.Messaging.EmailConsumer.csproj"
COPY . .
WORKDIR "/src/BancoEisen.Messaging.EmailConsumer"
RUN dotnet build "BancoEisen.Messaging.EmailConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancoEisen.Messaging.EmailConsumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoEisen.Messaging.EmailConsumer.dll"]