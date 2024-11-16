FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HrDirector/HrDirector.csproj", "HrDirector/"]
COPY ["HackathonContract/HackathonContract.csproj", "HackathonContract/"]
COPY ["HackathonDatabase/HackathonDatabase.csproj", "HackathonDatabase/"]
RUN dotnet restore "HrDirector/HrDirector.csproj"
COPY . .
WORKDIR "/src/HrDirector"
RUN dotnet build "HrDirector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HrDirector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HrDirector.dll"]