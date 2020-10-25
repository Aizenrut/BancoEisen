FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BancoEisen.AuthProvider/BancoEisen.AuthProvider.csproj", "BancoEisen.AuthProvider/"]
RUN dotnet restore "BancoEisen.AuthProvider/BancoEisen.AuthProvider.csproj"
COPY . .
WORKDIR "/src/BancoEisen.AuthProvider"
RUN dotnet build "BancoEisen.AuthProvider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancoEisen.AuthProvider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoEisen.AuthProvider.dll"]