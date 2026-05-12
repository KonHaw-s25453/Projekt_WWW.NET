# GameLibrary

Aplikacja webowa ASP.NET Core MVC do zarządzania biblioteką gier.

## Technologie

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core 8
- SQLite
- Razor Views + Bootstrap

## Zakres funkcjonalny

- CRUD dla graczy (`Player`)
- CRUD dla gier (`Game`)
- CRUD dla tagów (`Tag`)
- Relacja M:N `Player` ↔ `Game` przez `PlayerGame` (z polem `HoursPlayed`)
- Relacja M:N `Game` ↔ `Tag` skonfigurowana przez EF Core (`GameTags`)
- Relacja self M:N `Player` ↔ `Player` przez `Friend`
- Komponent widoku `FriendListViewComponent` na stronie szczegółów gracza
- Seed danych startowych (gracze, gry, tagi, powiązania)

## Struktura projektu

- `GameLibrary/Controllers` – kontrolery MVC
- `GameLibrary/Models` – encje i modele widoków
- `GameLibrary/Data/AppDbContext.cs` – konfiguracja modelu EF Core i seed danych
- `GameLibrary/ViewComponents` – komponenty widoków
- `GameLibrary/Views` – widoki Razor
- `GameLibrary/Migrations` – migracje EF Core

## Uruchomienie lokalne

```bash
cd GameLibrary
dotnet restore
dotnet build GameLibrary.sln --nologo
dotnet run --project GameLibrary.csproj
```

Aplikacja przy starcie automatycznie uruchamia migracje (`db.Database.Migrate()`), więc baza SQLite zostanie utworzona/zaktualizowana automatycznie.

## Dokumentacja szczegółowa

Szczegółowe opisy poszczególnych elementów znajdują się w katalogu `Dokumentacja/`.
