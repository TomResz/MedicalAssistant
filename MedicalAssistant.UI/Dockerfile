FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY MedicalAssistant.UI.csproj MedicalAssistant.UI/

RUN dotnet restore MedicalAssistant.UI/MedicalAssistant.UI.csproj

COPY . . 
RUN dotnet build MedicalAssistant.UI.csproj -c Release -o /app/build

#RUN dotnet workload install wasm-tools

FROM build AS publish
RUN dotnet publish MedicalAssistant.UI.csproj -c Release -o /app/publish  -p:BlazorWasmPrerenderingKeepServer=false


FROM nginx:alpine AS final

WORKDIR /usr/share/nginx/html

COPY --from=publish /app/publish/wwwroot .

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80