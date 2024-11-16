FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HrManager/HrManager.csproj", "HrManager/"]
COPY ["HackathonContract/HackathonContract.csproj", "HackathonContract/"]
COPY ["Hackathon/Hackathon.csproj", "Hackathon/"]
COPY ["HackathonDatabase/HackathonDatabase.csproj", "HackathonDatabase/"]
COPY ["HackathonStrategy/HackathonStrategy.csproj", "HackathonStrategy/"]
RUN dotnet restore "HrManager/HrManager.csproj"
COPY . .
WORKDIR "/src/HrManager"
RUN dotnet build "HrManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HrManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HrManager.dll"]
