# Builds and runs the tutorial on Linux. The project picks the SelectPdf.Universal.Linux
# package when it is built on Linux, which bundles everything Chromium needs - no apt-get.
#   docker build -t selectpdf-universal-tutorial .
#   docker run --rm -e SELECTPDF_LICENSE_KEY -v "$PWD/output:/tutorial/output" selectpdf-universal-tutorial
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release --no-restore -o /app

# The aspnet image only because section 22 compiles an ASP.NET Core controller;
# a console app needs just mcr.microsoft.com/dotnet/runtime:8.0.
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /tutorial
COPY --from=build /app /app
COPY --chown=app:app assets ./assets
RUN chown app:app /tutorial
# Runs as the image's non-root user - no extra flags or capabilities needed.
USER app
ENTRYPOINT ["dotnet", "/app/HtmlToPdf.Universal.Tutorial.dll", "/tutorial"]
