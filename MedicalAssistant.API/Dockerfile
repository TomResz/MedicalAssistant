#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MedicalAssistant.API/MedicalAssistant.API.csproj", "MedicalAssistant.API/"]
COPY ["MedicalAssistant.Application/MedicalAssistant.Application.csproj", "MedicalAssistant.Application/"]
COPY ["MedicalAssistant.Domain/MedicalAssistant.Domain.csproj", "MedicalAssistant.Domain/"]
COPY ["MedicalAssistant.Shared/MedicalAssistant.Shared.csproj", "MedicalAssistant.Shared/"]
COPY ["MedicalAssistant.Infrastructure/MedicalAssistant.Infrastructure.csproj", "MedicalAssistant.Infrastructure/"]
COPY ["MedicalAssistant.UI/MedicalAssistant.UI.csproj", "MedicalAssistant.UI/"]
RUN dotnet restore "./MedicalAssistant.API/./MedicalAssistant.API.csproj"
COPY . .
WORKDIR "/src/MedicalAssistant.API"
RUN dotnet build "./MedicalAssistant.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MedicalAssistant.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalAssistant.API.dll"]