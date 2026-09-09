# CLAUDE.md

Guidance for Claude Code when working in this repository. Read this before making changes.

## What this is

A multi-vehicle car maintenance tracker: WPF desktop app, MVVM, SQLite-backed
persistence via EF Core, with a chart view of cost/mileage over time for the
selected vehicle.

## Stack

- WPF on .NET 8 (LTS), C#
- Plain MVVM — no Prism or other MVVM framework
- EF Core + SQLite for persistence
- Charting library TBD (evaluated in Phase 5)

## Folder structure

```
CarMaintenanceTracker.sln
CarMaintenanceTracker/
├── App.xaml / App.xaml.cs
├── Models/            — Vehicle, ServiceEntry (plain data classes)
├── Data/               — AppDbContext (EF Core, SQLite) + Migrations/
├── Services/           — ServiceLogRepository — CRUD against AppDbContext, no UI concerns
├── ViewModels/         — ObservableObject, RelayCommand, MainViewModel, ServiceLogViewModel
├── Views/              — MainWindow, VehicleEditView, ServiceEntryEditView, ChartView (XAML)
└── Converters/
```

Layering rule: Views never talk to Data/ directly — always through ViewModels and
Services. ViewModels never touch AppDbContext directly — always through the
repository in Services/.

## Code quality bar (every phase, not just the first)

- Follow SOLID principles — single-responsibility classes, depend on abstractions
  where it's not overkill for an app this size, no god classes or god methods.
- No code smells: no magic strings/numbers (use constants or config), no dead code,
  no duplicated logic, no tight coupling between layers.
- Consistent naming, file-per-class, matching the folder structure above.
- Keep classes small and focused — if a class is doing more than one job, split it.

## Phase plan

Work is broken into phases in `claude/CAR-TRACKER-PHASES.md`. Read the current
phase's scope there before making changes — each phase has an explicit "don't
touch anything past this phase" boundary. To pick up work, say "pick up Phase N"
and it has everything needed without re-explaining the stack or structure.

## Verifying changes

No test suite beyond what's introduced in Phase 2 (throwaway console test / unit
tests for the repository). Verify by building (`dotnet build`) and running
(`dotnet run --project CarMaintenanceTracker`) after each change — the app should
launch without errors at every phase boundary.
