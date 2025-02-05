# Medical Assistant
## Overview
A medical assistant application developed as part of an engineering thesis. It focuses on managing basic health-related matters on the patient’s side. Built with .NET technology following the Clean Architecture approach.  
## Technologies
### 🖥️ Frontend

![Blazor WebAssembly](https://img.shields.io/badge/Blazor%20WebAssembly-512BD4?style=for-the-badge&logo=blazor&logoColor=white)  
![MudBlazor](https://img.shields.io/badge/MudBlazor-0277BD?style=for-the-badge)  
![Radzen](https://img.shields.io/badge/Radzen-3B3B3B?style=for-the-badge)  

---

### ⚙️ Backend

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-5C2D91?style=for-the-badge&logo=.net&logoColor=white)  
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)  
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=signalr&logoColor=white)  
![Hangfire](https://img.shields.io/badge/Hangfire-DD3333?style=for-the-badge)  
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=ef&logoColor=white) 
## 📥 Installation Guide  

This guide provides step-by-step instructions to set up and run the **MedicalAssistant** application.  

---

### 🔑 Step 1: Configure Secrets File  

To ensure a proper installation and launch, you need to create a secrets file inside the **MedicalAssistant.API** folder. This file should contain sensitive data such as API keys, SMTP credentials, and AES keys.  

#### 📌 Create the file:  
#### 📂 Path: `MedicalAssistant.API/appsettings.Secrets.json`  

#### 📂 File Structure: `appsettings.Secrets.json`
```json
{
  "smtp:Port": "587",
  "smtp:Password": "passwordpawssword",
  "smtp:Host": "host.example.com",
  "smtp:Email": "example@mail.com",
  "Authentication:Google:ClientSecret": "googleSecretKey",
  "Authentication:Google:ClientId": "googleclientid",
  "Authentication:Google:CallbackPath": "http://localhost:8080/authentication/google-callback",
  "Authentication:Facebook:ClientSecret": "facebookSecretKey",
  "Authentication:Facebook:ClientId": "000000000000000",
  "Authentication:Facebook:CallbackPath": "http://localhost:8080/authentication/facebook-callback",
  "auth:signInKey": "signInKeySuperStrongKey12345678@#!#!#!#!#*&^%$#@!",
  "auth:refreshTokenExpiration": "07.00:00:00",
  "auth:issuer": "issuer",
  "auth:expiry": "00.00:15:00",
  "auth:audience": "audience",
  "AES:Key": "32charactertab",
  "AES:IV": "16charactertab"
}
```
### 🎨 Step 2: Configure Frontend Settings 
After configuring the backend secrets, modify the public keys on the frontend.
#### 📌 Edit the configuration file:
#### 📂 Path: `MedicalAssistantUI/wwwroot/appsettings.json`
#### 📂 File Structure: `appsettings.json`
``` json
{
  "Auth": {
    "Google": {
      "ClientId": "597248992595-3p01h3b9dh7ofninobhn7oi6hfpcfuuk.asas.googleusercontent.com",
      "CallbackPath": "http://localhost:8080/authentication/google-callback"
    },
    "Facebook": {
      "ClientId": "asadas",
      "CallbackPath": "http://localhost:8080/authentication/facebook-callback"
    }
  },
  "Api": {
    "Url": "http://localhost:5000/api/",
    "NotificationHubUrl": "http://localhost:5000/notifications"
  }
}
```
### 🚀 Step 3: Run the Application
Once all configurations are set, start the application using Docker.
#### 📌 Run the following command:
``` bash
docker-compose up -d
```
## 🧪 Tests
To run tests, ensure that the **Docker daemon** is running. The application depends on services managed by Docker, so they must be available before executing tests.  
### 📌 Run the Tests:
``` bash
dotnet test
```

