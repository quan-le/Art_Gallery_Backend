# Art Gallery Backend Service

![.NET](https://img.shields.io/badge/.NET-9-blue) ![EF Core](https://img.shields.io/badge/EF%20Core-9.0-blue) ![Auth0](https://img.shields.io/badge/Auth0-JWT-orange) ![AutoMapper](https://img.shields.io/badge/AutoMapper-12.0-lightgrey)

## Table of Contents

1. [Project Overview](#project-overview)  
2. [Technologies & Tools](#technologies--tools)  
3. [Architecture & Design](#architecture--design)  
4. [Prerequisites](#prerequisites)  
5. [Setup & Installation](#setup--installation)  
6. [Configuration](#configuration)  
7. [Database Schema](#database-schema)  

---

## Project Overview

A .NET 7 Web API backend for an Art Gallery application, providing CRUD operations for **Artifacts**, **Artists**, and **Tags**. Features:

- **Entity Framework Core** (Database-First) with SQL Server  
- **Auth0** for OAuth2/JWT authentication and policy-based authorization  
- **AutoMapper** for DTO-to-entity mapping  
- **Swagger** (Swashbuckle) & **Scalar.UI** for interactive docs  
- Many-to-many relations and junction tables

---

## Technologies & Tools

- **.NET 7** ASP.NET Core Web API  
- **C#**  
- **Entity Framework Core 7.0**  
- **SQL Server**  
- **Auth0** (OAuth2 + JWT Bearer)  
- **AutoMapper**  
- **Swagger & Scalar.UI**  
- **Visual Studio 2022** 

---

## Architecture & Design

- **Layers**: Controllers → DTOs → Services/DAOs → EF Core Models → Microsoft SQL Server
- **Mapping**: AutoMapper Profiles isolate mapping logic  
- **Auth**: JWT Bearer via Auth0; policies: `Admin` (write), `User` (read)  

---

## Prerequisites

- .NET 7 SDK installed  
- SQL Server instance available  
- Auth0 tenant with API & Application configured  
- `appsettings.json` populated with connection string and Auth0 settings  

---

## Setup & Installation

```bash
git clone https://github.com/yourusername/art-gallery-backend.git
cd art-gallery-backend
# Scaffolding EF Core and Handlebar Template:
dotnet ef dbcontext scaffold "SQL_Connection_String" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context GalleryDBContext --use-database-names --no-onconfiguring --data-annotations -f
```

---

## Configuration
```bash
{
  "ConnectionStrings": {
    "ArtGalleryDb": "SQL_)Connection_String;"
  },
  "Auth0": {
    "Domain": "Your Auth0 Domain",
    "Audience": "Your Auth0 Audience",
    "SwaggerClientId": "YOUR_CLIENT_ID",
    "SwaggerClientSecret": "YOUR_CLIENT_SECRET"
  }
}
```

---

## Database Schema
![Screenshot 2025-05-28 224620](https://github.com/user-attachments/assets/93e3c4e7-a170-4739-9487-17260cf84ca1)
