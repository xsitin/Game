# Agent Handoff: WinForms → Godot migration

## Current branch/state
- Branch: `work`
- Latest migration direction: domain-first rewrite to `Game.Core`, Godot as thin UI shell.
- Combat loop, skills/buffs, turn queue, progressive encounter generation, and basic progress persistence are already in `Game.Core`.

## What is already done
1. `Game.Core/Combat/*`
   - battle engine and combat domain extracted from WinForms path.
   - skill catalog and targeting model connected to Godot UI.
2. `Game.Core/Progression/*`
   - JSON persistence for migration progress (`ProgressState`, `IProgressStorage`, `JsonFileProgressStorage`).
   - legacy save migration bootstrap (`LegacyPlayerSaveMigration`) for hero levels.
3. `GodotClient/scripts/Main.cs`
   - uses `BattleSetupFactory` + persisted progress.
   - supports skill/target selection and fallback quick actions.
4. `Game.Core.Tests/*`
   - unit tests for encounter determinism, skill-target API, persistence, and legacy migration.

## Key files to read first
- `docs/godot-migration-plan.md`
- `Game.Core/Combat/BattleEngine.cs`
- `Game.Core/Combat/BattleSetupFactory.cs`
- `Game.Core/Progression/JsonFileProgressStorage.cs`
- `Game.Core/Progression/LegacyPlayerSaveMigration.cs`
- `GodotClient/scripts/Main.cs`
- `Game.Core.Tests/*`

## How to run locally
1. Setup dependencies:
   - `bash scripts/setup-env.sh`
2. Run core tests:
   - `dotnet test /workspace/Game/Game.Core.Tests/Game.Core.Tests.csproj`
3. Build Godot client:
   - `dotnet build /workspace/Game/GodotClient/GameGodot.csproj`
4. Sanity-run Godot in headless editor mode:
   - `godot4 --headless --path /workspace/Game/GodotClient --editor --quit`

## Highest-priority remaining work
1. Expand legacy save migration beyond hero levels:
   - gold/economy
   - inventory/storage/shop
   - active team composition fidelity
2. Move progression UI out of prototype battle screen:
   - separate menu/roster/progression scenes
   - connect to `IProgressStorage`
3. Add migration validation tests for real legacy JSON samples from old saves.

## Risks / gotchas
- `godot4 --headless --editor --quit` prints non-blocking editor warnings in CI/container; this is expected.
- Full `Game.sln` build may include legacy Windows-targeted projects not suitable for Linux CI.
- The current migration importer intentionally keeps scope narrow (hero levels only); do not assume full save parity yet.
