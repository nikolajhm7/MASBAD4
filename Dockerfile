FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Handin4/Handin4.csproj", "Handin4/"]
RUN dotnet restore "Handin4/Handin4.csproj"
COPY . .
WORKDIR "/src/Handin4"
RUN dotnet build "Handin4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Handin4.csproj" -c Release -o /app/publish

# Brug base image med ASP.NET runtime, kopier den byggede applikation fra publish stadiet
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Handin4.dll"]