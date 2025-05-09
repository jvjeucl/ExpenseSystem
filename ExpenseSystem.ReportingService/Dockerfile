FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExpenseSystem.ReportingService/ExpenseSystem.ReportingService.csproj", "ExpenseSystem.ReportingService/"]
COPY ["ExpenseSystem.Database/ExpenseSystem.Database.csproj", "ExpenseSystem.Database/"]
COPY ["ExpenseSystem.Shared/ExpenseSystem.Shared.csproj", "ExpenseSystem.Shared/"]
RUN dotnet restore "ExpenseSystem.ReportingService/ExpenseSystem.ReportingService.csproj"
COPY . .
WORKDIR "/src/ExpenseSystem.ReportingService"
RUN dotnet build "ExpenseSystem.ReportingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpenseSystem.ReportingService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseSystem.ReportingService.dll"]