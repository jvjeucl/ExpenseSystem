FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExpenseSystem.API/ExpenseSystem.API.csproj", "ExpenseSystem.API/"]
COPY ["ExpenseSystem.Database/ExpenseSystem.Database.csproj", "ExpenseSystem.Database/"]
COPY ["ExpenseSystem.Shared/ExpenseSystem.Shared.csproj", "ExpenseSystem.Shared/"]
RUN dotnet restore "ExpenseSystem.API/ExpenseSystem.API.csproj"
COPY . .
WORKDIR "/src/ExpenseSystem.API"
RUN dotnet build "ExpenseSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpenseSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ExpenseSystem.API.dll"]