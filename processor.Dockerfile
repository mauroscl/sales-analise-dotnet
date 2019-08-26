FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /app

COPY /SalesProcessor.*/*.csproj ./

COPY . ./
RUN dotnet publish -c Release -o out SalesProcessor.ConsoleApp

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime

COPY wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

WORKDIR /app
COPY --from=build /app/SalesProcessor.ConsoleApp/out ./
#ENTRYPOINT ["dotnet", "SalesProcessor.ConsoleApp.dll"]
