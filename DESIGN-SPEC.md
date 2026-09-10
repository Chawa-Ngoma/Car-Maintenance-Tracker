# Car Maintenance Tracker — Visual Design Spec

Approved direction for restyling the WPF app. Implement as WPF resource dictionaries
(colors + control styles), merged into App.xaml, applied across MainWindow,
VehicleEditView, and ServiceEntryEditView. This is a visual-only pass — no
ViewModel, command, or binding logic changes.

## Palette — car-friendly, two accents + neutral base

| Token              | Hex       | Use                                                          |
|--------------------|-----------|---------------------------------------------------------------|
| `AccentBrush`       | `#C8372E` | ALL action buttons (Add New, Save, Delete-confirm), selected vehicle highlight, primary CTAs. One color for every action — never mix accent colors on buttons. |
| `AccentDarkBrush`   | `#A32D26` | Accent button hover/pressed state.                           |
| `SecondaryBrush`    | `#3B5166` | Icons, section headers, non-action structural elements (e.g. service-type badge backgrounds' icon color). |
| `SecondaryLightBrush` | `#E8ECEF` | Badge/icon circle backgrounds (paired with `SecondaryBrush` icon color on top). |
| `SurfaceBrush`      | `#FFFFFF` | Card backgrounds (list panels, edit panels).                 |
| `PageBackgroundBrush` | `#F4F3EF` | Window/page background — warm off-white, not stark white.  |
| `BorderBrush`       | `#E0DED8` | Card/panel hairline borders.                                 |
| `TextPrimaryBrush`  | `#22262B` | Primary text (graphite, not pure black).                     |
| `TextSecondaryBrush`| `#6B6F73` | Secondary/muted text (dates, subtitles).                      |
| `DangerBrush`       | `#B02A2A` | Validation error text/borders only — distinct from AccentBrush so "error" never looks like "action". |
| `SuccessBrush`      | `#3C7A4E` | Optional: cost-trending-down or positive indicators, if used. |

Danger red and Accent red must read as visually distinct — Danger is desaturated/darker so an error never gets mistaken for a clickable action.

## Typography

- Primary font family: `Segoe UI Variable` (falls back to `Segoe UI` on older Windows) — keep it system-native, no custom font files.
- Section headers ("Vehicles", "Service Log"): 15px, SemiBold.
- Card titles (vehicle nickname, service type): 14px, SemiBold.
- Body/subtitles (make/model/year, date/odometer): 12px, Regular, `TextSecondaryBrush`.
- Metric numbers (summary cards): 22px, SemiBold.

## Corner radius & spacing

- Cards/panels: 8px corner radius.
- Buttons and input fields: 4px corner radius.
- Icon badge circles: fully rounded (Ellipse or 50% CornerRadius), 36x36px.
- Card padding: 16px.
- Gap between list rows: 8px vertical padding, hairline `BorderBrush` divider between rows (not full card borders per row).

## Layout changes from current state

1. **Vehicle list (left panel):** each vehicle becomes a card-style row — icon badge (car icon, `SecondaryLightBrush` circle background, `SecondaryBrush` icon) + nickname (bold) + make/model/year subtitle (muted). Selected vehicle gets `AccentBrush`-tinted background (a light accent tint, not the full solid red — solid red is reserved for buttons) with the icon badge and text switching to accent-colored variants.
2. **Summary row (new, top of right panel):** 3 metric cards — Total spent, Entry count, Last service date — flat `SurfaceBrush` cards, no border, per the metric-card treatment (muted label above, bold number below).
3. **Service log (right panel):** rows instead of a bare list — icon badge per service type (see mapping below) + service type (bold) + date/odometer (muted subtitle) + cost (right-aligned, bold) + an edit icon-button. Row separated by a hairline, not individual card borders.
4. **Edit panels:** keep the existing field layout from Phase 4, but restyle inputs to 4px corner radius with `BorderBrush` border and `AccentBrush` focus outline; buttons restyled per the button spec below.
5. **Chart (Phase 5, once built):** use `AccentBrush` for the primary data series; `SecondaryBrush` only if a second series/comparison is ever added.

## Service-type icon mapping

Use these as a starting map (extend as new service types are added); icon badge background is always `SecondaryLightBrush` with the icon itself in `SecondaryBrush` — icons are never accent-colored, to keep accent reserved for actions/selection:

| Service type (case-insensitive match) | Icon                          |
|----------------------------------------|-------------------------------|
| Oil change                              | droplet                       |
| Brake / brake pads                      | tool (wrench)                 |
| Inspection / service                    | clipboard-check                |
| Tyres / tires                           | circle (or a wheel-style icon if available) |
| Battery                                 | battery                       |
| Default / unmatched                     | car                            |

## Buttons

- **Primary action** (Add New, Save): `AccentBrush` background, white text, 4px radius, `AccentDarkBrush` on hover/pressed.
- **Secondary action** (none currently, but reserve for future non-destructive secondary actions): outline style — `BorderBrush` border, `TextPrimaryBrush` text, transparent background, `SecondaryLightBrush` on hover.
- **Delete**: keep visually distinct from Save/Add — outline style using `DangerBrush` for border and text, not a solid fill, so it doesn't visually compete with the accent-colored primary actions. Confirmation MessageBox stays as-is from Phase 4.

## What NOT to change

- No ViewModel, command, binding, or validation logic changes — this is WPF resource dictionaries (`Colors.xaml`, `Controls.xaml` or similar) and template/style application only.
- Don't touch `IsEnabled` logic fixed in the last bug pass.
- Keep the existing two-column overall structure (vehicles left, log right) — this is a restyle, not a re-architecture.
