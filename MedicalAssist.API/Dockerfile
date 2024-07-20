FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5100
EXPOSE 5101

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MedicalAssist.API/MedicalAssist.API.csproj", "MedicalAssist.API/"]
COPY ["MedicalAssist.Application/MedicalAssist.Application.csproj", "MedicalAssist.Application/"]
COPY ["MedicalAssist.Domain/MedicalAssist.Domain.csproj", "MedicalAssist.Domain/"]
COPY ["MedicalAssist.Shared/MedicalAssist.Shared.csproj", "MedicalAssist.Shared/"]
COPY ["MedicalAssist.Infrastructure/MedicalAssist.Infrastructure.csproj", "MedicalAssist.Infrastructure/"]
RUN dotnet restore "./MedicalAssist.API/./MedicalAssist.API.csproj"
COPY . .
WORKDIR "/src/MedicalAssist.API"
RUN dotnet build "./MedicalAssist.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MedicalAssist.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalAssist.API.dll"]