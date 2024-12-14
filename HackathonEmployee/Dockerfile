FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HackathonEmployee/HackathonEmployee.csproj", "HackathonEmployee/"]
COPY ["HackathonContract/HackathonContract.csproj", "HackathonContract/"]
RUN dotnet restore "HackathonEmployee/HackathonEmployee.csproj"
COPY . .
WORKDIR "/src/HackathonEmployee"

RUN echo "Building with EMPLOYER_TYPE=${EMPLOYER_TYPE} and EMPLOYER_ID=${EMPLOYER_ID}"
RUN dotnet build "HackathonEmployee.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackathonEmployee.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackathonEmployee.dll"]
