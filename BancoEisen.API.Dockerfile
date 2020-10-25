FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BancoEisen.API/BancoEisen.API.csproj", "BancoEisen.API/"]
COPY ["BancoEisen.Services/BancoEisen.Services.csproj", "BancoEisen.Services/"]
COPY ["BancoEisen.Data/BancoEisen.Data.csproj", "BancoEisen.Data/"]
COPY ["BancoEisen.Models/BancoEisen.Models.csproj", "BancoEisen.Models/"]
RUN dotnet restore "BancoEisen.API/BancoEisen.API.csproj"
COPY . .
WORKDIR "/src/BancoEisen.API"
RUN dotnet build "BancoEisen.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancoEisen.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoEisen.API.dll"]