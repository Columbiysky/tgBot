FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /App

COPY IsTheKittenFed.csproj .
RUN dotnet restore IsTheKittenFed.csproj

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "IsTheKittenFed.dll"]