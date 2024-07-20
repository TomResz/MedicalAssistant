FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
EXPOSE 8080
EXPOSE 8081

WORKDIR /src

COPY MedicalAssist.UI/MedicalAssist.UI.csproj MedicalAssist.UI/
RUN dotnet restore MedicalAssist.UI/MedicalAssist.UI.csproj

COPY . .
RUN dotnet build MedicalAssist.UI/MedicalAssist.UI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish MedicalAssist.UI/MedicalAssist.UI.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf