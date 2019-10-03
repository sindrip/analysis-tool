FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim as base
WORKDIR /app
EXPOSE 80
EXPOSE 443 

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster as build
WORKDIR /src
COPY AnalysisTool.sln .
COPY src/WebApplication/WebApplication.csproj src/WebApplication/
COPY src/Analysis/Analysis.csproj src/Analysis/
COPY src/Parser/Parser.csproj src/Parser/
COPY src/ConsoleApp/ConsoleApp.csproj src/ConsoleApp/
RUN dotnet restore AnalysisTool.sln
COPY . .
WORKDIR /src/src/WebApplication
RUN dotnet build WebApplication.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish WebApplication.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WebApplication.dll"]
CMD ["dotnet", "WebApplication.dll" ]
