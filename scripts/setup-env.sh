#!/usr/bin/env bash
set -euo pipefail

# Installs prerequisites for building/running the Godot C# client in this repo.
# Target OS: Ubuntu/Debian.

SCRIPT_DIR="$(cd "$(dirname "$(realpath "${BASH_SOURCE[0]}")")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

if ! command -v curl >/dev/null 2>&1; then
  apt-get update
  apt-get install -y curl ca-certificates
fi

if ! command -v unzip >/dev/null 2>&1; then
  apt-get update
  apt-get install -y unzip wget
fi

if ! command -v dotnet >/dev/null 2>&1; then
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  bash /tmp/dotnet-install.sh --version 8.0.401 --install-dir /usr/share/dotnet
  ln -sf /usr/share/dotnet/dotnet /usr/local/bin/dotnet
fi

if ! command -v godot4 >/dev/null 2>&1; then
  apt-get update
  apt-get install -y \
    libfontconfig1 libx11-6 libxcursor1 libxinerama1 libxrandr2 libxi6 \
    libxrender1 libxext6 libxfixes3 libxkbcommon0 libgl1 libasound2t64

  mkdir -p /opt/godot
  cd /opt/godot
  wget -q https://github.com/godotengine/godot/releases/download/4.2.2-stable/Godot_v4.2.2-stable_mono_linux_x86_64.zip -O godot_mono.zip
  unzip -o godot_mono.zip
  chmod +x /opt/godot/Godot_v4.2.2-stable_mono_linux_x86_64/Godot_v4.2.2-stable_mono_linux.x86_64
  ln -sf /opt/godot/Godot_v4.2.2-stable_mono_linux_x86_64/Godot_v4.2.2-stable_mono_linux.x86_64 /usr/local/bin/godot4
  ln -sf /usr/local/bin/godot4 /usr/local/bin/godot
fi

echo "Installed dotnet version:"
dotnet --version

echo "Installed Godot version:"
godot4 --headless --version

echo "Building Godot C# client..."
dotnet build "$REPO_ROOT/GodotClient/GameGodot.csproj"
