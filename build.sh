#!/bin/bash

# Valheim Mod Build Script for macOS
# This script builds the ItemAutoPickupIgnorer mod

echo "🔨 Building Valheim ItemAutoPickupIgnorer Mod"
echo "=============================================="

# Check if required DLLs exist
LIBS_DIR="./libs"
REQUIRED_DLLS=("0Harmony.dll" "assembly_valheim.dll" "BepInEx.dll" "UnityEngine.dll" "UnityEngine.CoreModule.dll")
MISSING_DLLS=()

echo "📦 Checking for required DLL files..."
for dll in "${REQUIRED_DLLS[@]}"; do
    if [ ! -f "$LIBS_DIR/$dll" ]; then
        MISSING_DLLS+=("$dll")
        echo "❌ Missing: $dll"
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
    echo "🔄 Building with fallback Unity assemblies (for syntax checking)..."
else
    echo ""
    echo "✅ All required DLLs found!"
    echo ""
    echo "🔄 Building with complete dependencies..."
fi

# Clean previous build
echo "🧹 Cleaning previous build artifacts..."
dotnet clean ItemAutoPickupIgnorer.csproj --nologo --verbosity quiet

# Restore packages
echo "📦 Restoring NuGet packages..."
dotnet restore ItemAutoPickupIgnorer.csproj --nologo --verbosity quiet

# Build the project
echo "🔨 Building project..."
if dotnet build ItemAutoPickupIgnorer.csproj --no-restore --verbosity minimal; then
    echo ""
    echo "🎉 Build completed successfully!"
    echo "   Output: bin/Debug/net472/ItemAutoPickupIgnorer.dll"
    
    if [ ${#MISSING_DLLS[@]} -gt 0 ]; then
        echo ""
        echo "⚠️  Note: Built with fallback assemblies. The DLL may not work without actual game files."
    fi
else
    echo ""
    echo "❌ Build failed!"
    if [ ${#MISSING_DLLS[@]} -gt 0 ]; then
        echo "   This is likely due to missing DLL files."
        echo "   Please add the required DLLs to the libs/ directory."
    fi
    exit 1
fi
