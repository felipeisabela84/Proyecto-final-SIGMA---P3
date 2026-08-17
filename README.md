# SIGMA — Sistema de Gestión de Reportes

Sistema web desarrollado como proyecto final para la **gestión y seguimiento de reportes ciudadanos** relacionados con incidencias y servicios públicos.

La aplicación permite a los ciudadanos registrar y consultar reportes, mientras que los operadores pueden gestionarlos y actualizar su estado.

## 📋 Descripción

**SIGMA** es una aplicación web desarrollada con **ASP.NET Core MVC** que implementa autenticación, autorización basada en roles, persistencia de datos mediante Entity Framework Core y una estructura separada de servicios y pruebas automatizadas.

El sistema contempla principalmente dos tipos de usuarios:

* 👤 **Ciudadano:** puede registrar y consultar reportes.
* 🛠️ **Operador:** puede gestionar reportes y actualizar su estado.

La autenticación y autorización se implementan mediante **ASP.NET Core Identity**, utilizando los roles `Ciudadano` y `Operador`.

---

## 🚀 Tecnologías utilizadas

### Aplicación

* **C#**
* **.NET 8**
* **ASP.NET Core MVC**
* **Razor Pages**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **Bootstrap / HTML / CSS / JavaScript**

### Pruebas

* **NUnit**
* **Selenium WebDriver**
* **Selenium Support**
* Pruebas unitarias
* Pruebas E2E (End-to-End)

El proyecto principal está configurado sobre `net8.0` y utiliza **Entity Framework Core 8** para SQL Server.

---

## 🏗️ Arquitectura del proyecto

El repositorio está organizado de la siguiente manera:

```text
Proyecto-final-SIGMA---P3/
│
├── Areas/
│   └── Identity/
│       └── Pages/
│
├── Controllers/
│
├── Data/
│
├── Models/
│
├── Services/
│
├── Views/
│
├── wwwroot/
│
├── SIGMA.Tests.Unit/
│   ├── Integration/
│   ├── ReporteConsultaServiceTests.cs
│   ├── ReporteEstadisticasServiceTests.cs
│   ├── ReporteEstadoService.cs
│   ├── ReporteValidacionServiceTests.cs
│   └── SIGMA.Tests.Unit.csproj
│
├── SIGMA.Tests.E2E/
│   ├── AccesoRolesTests.cs
│   ├── BaseTest.cs
│   ├── CambiarEstadoTests.cs
│   ├── CrearReporteTests.cs
│   ├── LoginTests.cs
│   ├── SIGMA.Tests.E2E.csproj
│   └── TestSetup.cs
│
├── Program.cs
├── SIGMA_PROJECT.csproj
├── SIGMA_PROJECT.slnx
├── appsettings.json
└── .gitignore
```

La estructura del repositorio incluye los módulos principales de la aplicación, las pruebas unitarias y el proyecto de pruebas E2E.

---

