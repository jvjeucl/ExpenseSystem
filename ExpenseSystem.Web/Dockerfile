FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExpenseSystem.Web/ExpenseSystem.Web.csproj", "ExpenseSystem.Web/"]
COPY ["ExpenseSystem.Shared/ExpenseSystem.Shared.csproj", "ExpenseSystem.Shared/"]
RUN dotnet restore "ExpenseSystem.Web/ExpenseSystem.Web.csproj"
COPY . .
WORKDIR "/src/ExpenseSystem.Web"
RUN dotnet build "ExpenseSystem.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpenseSystem.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ExpenseSystem.Web.dll"]