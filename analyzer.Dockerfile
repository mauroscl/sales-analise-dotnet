FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /app

COPY /SalesAnalyzer.*/*.csproj ./

COPY . ./
RUN dotnet publish -c Release -o out SalesAnalyzer.ConsoleApp

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime

COPY wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

WORKDIR /app
COPY --from=build /app/SalesAnalyzer.ConsoleApp/out ./
#ENTRYPOINT ["dotnet", "SalesAnalyzer.ConsoleApp.dll"]
