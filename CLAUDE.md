# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build

This is a .NET Framework 4.7.2 WPF application. Build with Visual Studio or MSBuild:

```bash
msbuild "MOTION-DIAGNOSTICS.sln" /p:Configuration=Release
```

No CI/CD pipeline or test projects exist yet.

## Architecture

**MOTION-DIAGNOSTICS** is a WPF desktop application for motion controller diagnostics and wafer alignment. It follows a layered architecture with plugin-based device drivers.

### Projects

- **MotionDiagnostics** — WPF entry point (`App.xaml.cs`). Registers UI ViewModels, builds the Autofac container, initializes the database, and launches the shell window.
- **EPLE** — Core business logic: managers, services, data access, and ViewModels.
- **EPLE.Core** — Shared abstractions: `IDeviceHandler` interface, `AbstractService` base class, utilities.
- **Device.Dummy / Device.PowerPMAC / Device.ACS / Device.CAM** — Device driver plugins loaded at runtime via reflection.

### Dependency Injection

DI is configured in `EPLE/Startup.cs` using **Autofac** + `Microsoft.Extensions.DependencyInjection`. The UI project calls `Startup.Builder()`, adds its own registrations, then builds the container. Key registrations: configuration, NHibernate session factory, `DataRepository`, all managers, all services, all ViewModels.

### Device Plugin System

Devices implement `IDeviceHandler` (defined in `EPLE.Core/Device/Interface/IDeviceHandler.cs`). The interface defines typed communication methods (`GET_INT_IN`, `SET_DOUBLE_OUT`, `GET_STRING_IN`, etc.) plus lifecycle methods (`DeviceAttach`, `DeviceDettach`, `DeviceInit`, `DeviceReset`).

`DeviceManager` loads device DLLs dynamically via `Assembly.LoadFile()` using paths from `DeviceConfigEntity` in the database. Each loaded assembly is instantiated by its `InstanceName` and cast to `IDeviceHandler`.

### Data Flow

```
UI → ViewModel → DataManager → DeviceManager → IDeviceHandler → Hardware
Hardware → IDeviceHandler → DataManager (polling) → ViewModel binding → UI
```

`DataManager` runs a polling loop that reads input data from devices at configurable intervals and publishes `DataChangedEvent` on value changes.

### Data Access

Uses **NHibernate** with SQL Server. Mapping files are XML-based (`.hbm.xml`) in `EPLE/Data/Database/Config/`. Schema auto-updates via `hbm2ddl.auto=update`. The `DataRepository` loads all config entities at startup into in-memory collections.

Entities: `DataConfigEntity`, `DeviceConfigEntity`, `AlarmConfigEntity`, `MeasureConfigEntity`.

### ViewModel Layer

ViewModels extend `ViewModel` (implements `INotifyPropertyChanged`) in `EPLE/ViewModel/`. The VM classes wrap config entities with MVVM bindings and expose `SaveChanges()` for persistence. UI-specific ViewModels live in `MotionDiagnostics/ViewModel/` and use **Prism** for MVVM and navigation.

### Key Enums

`DataConfigEnum.cs` defines: `DataType` (INT/DOUBLE/STRING/OBJECT), `Direction` (IN/OUT/BOTH), `DevMode` (UNKNOWN/DETTACHED/CONNECT/DISCONNECT/SIMULATE/ERROR), `AXES` (X/Y/T/Z1/Z2/Z3).

### Key Libraries

- **Autofac** — IoC container
- **NHibernate** — ORM (SQL Server)
- **Prism** — WPF MVVM framework
- **Serilog** — Structured logging
- **MaterialDesign Themes** — UI styling
- **OxyPlot** — Charting
- **HalconDotNet** — Machine vision / image processing

## Configuration

- `EPLE/appsettings.json` — Serilog config and database connection string (`DataOptions`)
- `EPLE/hibernate.cfg.xml` — NHibernate SQL Server connection
- `EPLE/Data/Database/Config/*.hbm.xml` — Entity mappings
- Device DLL paths are stored in the database (`DeviceConfigEntity.FileName`)
