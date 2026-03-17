FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["POC.KafkaProducer/POC.KafkaProducer.csproj", "POC.KafkaProducer/"]
COPY ["POC.Application/POC.Application.csproj", "POC.Application/"]
COPY ["POC.Domain/POC.Domain.csproj", "POC.Domain/"]

RUN dotnet restore "POC.KafkaProducer/POC.KafkaProducer.csproj"

COPY . .
WORKDIR "/src/POC.KafkaProducer"

RUN dotnet publish "POC.KafkaProducer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "POC.KafkaProducer.dll"]