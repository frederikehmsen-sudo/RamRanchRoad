FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ["./", "."]
RUN dotnet publish "Server/API/Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080/
EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]