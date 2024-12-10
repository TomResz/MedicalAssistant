FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY MedicalAssistant.UI.csproj MedicalAssistant.UI/

RUN dotnet restore MedicalAssistant.UI/MedicalAssistant.UI.csproj

COPY . . 
RUN dotnet build MedicalAssistant.UI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish MedicalAssistant.UI.csproj -c Release -o /app/publish


# Certs
FROM alpine:latest AS certs

RUN apk add --no-cache openssl

RUN mkdir -p /Certificates

RUN openssl genpkey -algorithm RSA -out /Certificates/key.pem -pkeyopt rsa_keygen_bits:2048 && \
    openssl req -new -key /Certificates/key.pem -out /Certificates/csr.pem -subj "/C=PL/ST=Silesian/L=Gliwice/O=MedicalAssistant/CN=localhost" && \
    openssl x509 -req -in /Certificates/csr.pem -signkey /Certificates/key.pem -out /Certificates/cert.pem -days 365 && \
    rm /Certificates/csr.pem

FROM nginx:alpine AS final

COPY --from=certs /Certificates/cert.pem /etc/ssl/certs/cert.pem
COPY --from=certs /Certificates/key.pem /etc/ssl/private/key.pem

WORKDIR /usr/share/nginx/html

COPY --from=publish /app/publish/wwwroot .

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 443
EXPOSE 80