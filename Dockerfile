# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln ./
COPY WebPingzor.Authentication/*.csproj ./WebPingzor.Authentication/
COPY WebPingzor.Counters/*.csproj ./WebPingzor.Counters/
COPY WebPingzor.Data/*.csproj ./WebPingzor.Data/
COPY WebPingzor.Monitoring/*.csproj ./WebPingzor.Monitoring/
COPY WebPingzor.Todos/*.csproj ./WebPingzor.Todos/
COPY WebPingzor.UserManagement/*.csproj ./WebPingzor.UserManagement/
COPY WebPingzor.Web/*.csproj ./WebPingzor.Web/
COPY WebPingzor.Web.Client/*.csproj ./WebPingzor.Web.Client/

RUN dotnet restore WebPingzor.sln

COPY . ./
WORKDIR /app/WebPingzor.Web
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebPingzor.Web.dll"]
