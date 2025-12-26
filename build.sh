#!/bin/bash
# Build script for SR2MP

echo "Building SR2MP in Release mode..."

# Navigate to project directory
cd "$(dirname "$0")"

# Clean previous builds
dotnet clean SR2MP/SR2MP.csproj

# Build in Release configuration
dotnet build SR2MP/SR2MP.csproj -c Release

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Build successful!"
    echo "📦 DLL location: SR2MP/bin/Release/net6.0/SR2MP.dll"
    echo ""

    # Optional: Copy to Slime Rancher 2 mods folder
    # Uncomment and modify path as needed:
    # SR2_MODS_PATH="C:/Program Files (x86)/Steam/steamapps/common/Slime Rancher 2/Mods"
    # if [ -d "$SR2_MODS_PATH" ]; then
    #     cp SR2MP/bin/Release/net6.0/SR2MP.dll "$SR2_MODS_PATH/"
    #     echo "✅ Copied to Slime Rancher 2 Mods folder"
    # fi
else
    echo "❌ Build failed!"
    exit 1
fi
