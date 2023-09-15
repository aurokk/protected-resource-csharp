FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source
COPY *.sln                    .
COPY Directory.Build.props    .
COPY Directory.Packages.props .
COPY global.json              .
COPY src/Api/*.csproj         ./src/Api/
RUN dotnet restore

COPY src/.   ./src/
WORKDIR /source/src/Api
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "Api.dll"]