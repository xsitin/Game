# Environment setup for Godot rewrite branch

This branch requires both **.NET SDK** and **Godot Mono editor/runtime**.

## One-command setup
```bash
sudo bash /path/to/repo/scripts/setup-env.sh
```

## What is installed
- `dotnet` SDK `8.0.401` in `/usr/share/dotnet`
- symlink `/usr/local/bin/dotnet`
- Godot Mono `4.2.2` binary from the official GitHub release in `/opt/godot`
- symlink `/usr/local/bin/godot4` (and `godot`)

## Verification
```bash
dotnet --info
godot4 --headless --version
dotnet build GodotClient/GameGodot.csproj
godot4 --headless --path GodotClient --editor --quit
```

## Why not apt
In this environment `apt` packages `godot`/`godot4` are unavailable, so setup uses the official release archive from GitHub.


`setup-env.sh` now resolves the repository root from its own location, so it can be launched from any current directory.
