FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY ./ ./
RUN dotnet publish -c Release -o publish
RUN chmod +x ./entrypoint.sh

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /source/publish ./ 
ENTRYPOINT ["bash", "./entrypoint.sh"]
