# Car Maintenance Tracker — Phased Build Plan `NOT STARTED`

Covers APP-02 from `claude/TICKETS.md` (WPF, MVVM + data-binding, SQLite persistence,
multi-vehicle service log with a chart view). Follows the same phase shape used for
2048 and Minesweeper (APP-01) — foundation → model/persistence → view-model plumbing →
views/binding → charting → polish → SOLID/code-smell pass — with an extra phase up front
for the data layer, since this app is persistence-first rather than logic-first like the
games were.

Repo: `car-maintenance-tracker` (per GH-02 — own dedicated repo, no ChawaTech branding,
MIT license). Local clone at C:\laragon\projects\Car-Maintenance-Tracker (per DEV-02 —
this isn't a PHP app, so it doesn't need Laragon's `www`).

Folder structure:

```
car-maintenance-tracker/
├── CarMaintenanceTracker.sln
├── CarMaintenanceTracker/
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── Models/
│   │   ├── Vehicle.cs
│   │   └── ServiceEntry.cs
│   ├── Data/
│   │   ├── AppDbContext.cs        — EF Core DbContext, SQLite
│   │   └── Migrations/
│   ├── Services/
│   │   └── ServiceLogRepository.cs — CRUD against the DbContext, no UI concerns
│   ├── ViewModels/
│   │   ├── ObservableObject.cs     — base class (INotifyPropertyChanged)
│   │   ├── RelayCommand.cs         — ICommand helper
│   │   ├── MainViewModel.cs        — vehicle list, selected vehicle
│   │   └── ServiceLogViewModel.cs  — entries for the selected vehicle, add/edit/delete
│   ├── Views/
│   │   ├── MainWindow.xaml         — vehicle list + selected vehicle's log
│   │   ├── VehicleEditView.xaml    — add/edit a vehicle
│   │   ├── ServiceEntryEditView.xaml — add/edit a service entry
│   │   └── ChartView.xaml          — cost/mileage-over-time for selected vehicle
│   └── Converters/
├── README.md
├── LICENSE
└── .gitignore
```

Multi-vehicle scope throughout: a vehicle has many service entries; the UI always
operates in the context of a "selected vehicle" once more than one exists.

## Phase 1 — Foundation `TODO`
Repo scaffold matching the folder structure above, connected to the GitHub repo.
Empty WPF project builds and runs (blank window), `.gitignore` covers `bin/`, `obj/`,
and the local SQLite `.db` file. First commit: "Scaffold repo and project shell".

## Phase 2 — Model & persistence `TODO`
`Vehicle` and `ServiceEntry` models, EF Core + SQLite `AppDbContext` with an
initial migration, `ServiceLogRepository` exposing CRUD for both entities — no UI
yet, exercised via a throwaway console test or unit tests. Confirms the schema and
that data survives an app restart before any XAML is written.
Commit: "Add data models, EF Core context, and repository CRUD".

## Phase 3 — ViewModel plumbing `TODO`
`ObservableObject` base class, `RelayCommand`, `MainViewModel` (vehicles as an
`ObservableCollection<Vehicle>`, `SelectedVehicle`), `ServiceLogViewModel` (entries
for the selected vehicle, Add/Edit/Delete commands wired to the repository from
Phase 2). Still no real UI — a temporary data-bound `ListBox` is enough to confirm
the bindings and commands work end to end.
Commit: "Wire up MVVM plumbing and connect view models to the repository".

## Phase 4 — Views & data binding `TODO`
Real XAML: `MainWindow` showing the vehicle list and the selected vehicle's service
log side by side, `VehicleEditView` and `ServiceEntryEditView` for add/edit, basic
input validation (required fields, numeric odometer/cost). Switching the selected
vehicle correctly filters the log.
Commit: "Build main window, edit views, and vehicle-switching UI".

## Phase 5 — Charting `TODO`
Add a charting library (whichever's simplest to wire into WPF — evaluate at this
phase rather than locking it in early) and a `ChartView` showing cost-over-time or
mileage-over-time for the selected vehicle, updating live as entries are added.
Commit: "Add cost/mileage chart for the selected vehicle".

## Phase 6 — Persistence polish & UX `TODO`
Delete confirmations, sensible empty states (no vehicles yet / no entries yet),
sorting the service log (by date or odometer), and any rough edges from manual
testing across Phases 2-5.
Commit: "Polish empty states, confirmations, and sorting".

## Phase 7 — SOLID / code-smell pass `TODO`
Full review once everything above works: check for tight coupling between
ViewModels and the EF Core context (should go through the repository, not
direct), duplicated XAML/converters, magic values that belong in constants,
and any dead code left over from the Phase 2/3 scaffolding. Same spirit as
Minesweeper's Phase 6 — fixes only, no new features; flag anything that's a
genuine new feature (e.g. keyboard navigation, CSV export) as a future ticket
instead of bundling it in.
Commit: "Code review pass — SOLID principles and cleanup".

---

Ticket done-when bar (from `claude/TICKETS.md` APP-02): builds and runs standalone;
vehicles and their service entries can be added, edited, and deleted; data persists
in SQLite between runs; switching between vehicles filters the log correctly; at
least one chart/summary view renders from the logged data; README explains what it
does and one interesting decision. That bar is met once Phases 1–5 are done — Phases
6–7 are the same "polish then review" tail Minesweeper used, not required to check
the box, but worth doing if you want it portfolio-ready.
