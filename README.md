# SIGMA — Sistema de Gestión de Reportes

##  Descripción

**SIGMA** es una aplicación web desarrollada con **ASP.NET Core MVC** que implementa autenticación, autorización basada en roles, persistencia de datos mediante Entity Framework Core y una estructura separada de servicios y pruebas automatizadas.

El sistema contempla principalmente dos tipos de usuarios:

*  **Ciudadano:** puede registrar y consultar reportes.
*  **Operador:** puede gestionar reportes y actualizar su estado.

La autenticación y autorización se implementan mediante **ASP.NET Core Identity**, utilizando los roles `Ciudadano` y `Operador`.

---

## Tecnologías utilizadas

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
## 📊 Reporte de pruebas E2E

Consulta el reporte HTML generado con ExtentReports, incluyendo las evidencias
capturadas durante la ejecución de las pruebas Selenium:

👉 [Ver reporte HTML de pruebas E2E](https://felipeisabela84.github.io/Proyecto-final-SIGMA---P3/Evidencias/ReporteHTML/ReporteSIGMA.html)
La estructura del repositorio incluye los módulos principales de la aplicación, las pruebas unitarias y el proyecto de pruebas E2E.

---

