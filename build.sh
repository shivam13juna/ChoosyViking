#!/bin/bash

# Valheim Mod Build Script for macOS
# This script builds the ChoosyViking mod v1.1.0

echo "🔨 Building Valheim ChoosyViking Mod v1.1.0"
echo "===================================================="

# Check if required DLLs exist
LIBS_DIR="./libs"
REQUIRED_DLLS=("assembly_valheim.dll")
OPTIONAL_DLLS=("0Harmony.dll" "BepInEx.dll" "UnityEngine.dll" "UnityEngine.CoreModule.dll")
MISSING_DLLS=()

echo "📦 Checking for required DLL files..."
for dll in "${REQUIRED_DLLS[@]}"; do
    if [ ! -f "$LIBS_DIR/$dll" ]; then
        MISSING_DLLS+=("$dll")
        echo "❌ Missing: $dll (REQUIRED)"
    else
        echo "✅ Found: $dll"
    fi
done

echo "📦 Checking for optional DLL files (will use NuGet fallbacks)..."
for dll in "${OPTIONAL_DLLS[@]}"; do
    if [ ! -f "$LIBS_DIR/$dll" ]; then
        echo "⚠️  Missing: $dll (will use NuGet package)"
    else
        echo "✅ Found: $dll"
    fi
done

if [ ${#MISSING_DLLS[@]} -gt 0 ]; then
    echo ""
    echo "⚠️  Some required DLL files are missing!"
    echo "   Please copy the following files to the libs/ directory:"
    printf "   - %s\n" "${MISSING_DLLS[@]}"
    echo ""
    echo "   See libs/README.md for instructions on where to find these files."
    echo ""
    echo "🔄 Building with NuGet dependencies..."
else
    echo ""
    echo "✅ All required DLLs found!"
    echo ""
    echo "🔄 Building with complete dependencies..."
fi

# Clean previous build
echo "🧹 Cleaning previous build artifacts..."
dotnet clean ChoosyViking.csproj --nologo --verbosity quiet

# Restore packages
echo "📦 Restoring NuGet packages..."
dotnet restore ChoosyViking.csproj --nologo --verbosity quiet

# Build the project
echo "🔨 Building project..."
if dotnet build ChoosyViking.csproj --no-restore --verbosity minimal; then
    echo ""
    echo "🎉 Build completed successfully!"
    echo "   Output: bin/Debug/net481/ChoosyViking.dll"
    
    if [ ${#MISSING_DLLS[@]} -gt 0 ]; then
        echo ""
        echo "⚠️  Note: Built with some NuGet fallbacks. Ensure you have the actual Valheim game files for full functionality."
    fi
    
    echo ""
    echo "📝 Installation Instructions:"
    echo "   1. Copy ChoosyViking.dll to BepInEx/plugins/"
    echo "   2. Copy shivam13juna.ChoosyViking.cfg to BepInEx/config/"
    echo "   3. Launch Valheim and enjoy!"
else
    echo ""
    echo "❌ Build failed!"
    if [ ${#MISSING_DLLS[@]} -gt 0 ]; then
        echo "   This is likely due to missing required DLL files."
        echo "   Please add assembly_valheim.dll to the libs/ directory."
    fi
    exit 1
fi
