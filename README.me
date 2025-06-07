# Choosy Viking

### Author: shivam13juna

A Valheim BepInEx plugin that allows users to selectively ignore auto-pickup of specific items. The mod provides an intuitive in-game interface to manage which items should be ignored during auto-pickup, giving players full control over their inventory management without any external configuration.

**Version:** 2.2.0  
**Plugin GUID:** `shivam13juna.ChoosyViking`  
**Target Framework:** .NET Framework 4.8.1  
**Build Support:** Cross-platform (Windows/macOS/Linux)  
**BepInEx Compatibility:** 5.4.22+ and 6.0.0+  
**Valheim Compatibility:** 0.218.15+ (Latest tested)

---

## 🎮 What does this mod do?

This plugin provides precise control over Valheim's auto-pickup system with a simple, user-friendly approach:

### Core Features
- **Empty by Default**: No items are ignored initially - all auto-pickup works normally until you specify otherwise
- **In-Game Management**: Add/remove items from ignore list entirely through the game interface
- **Interactive Controls**: Click items in inventory + press `I` to add items to ignore list, `Shift + I` to remove
- **Real-Time Feedback**: Instant on-screen messages when items are added/removed from ignore list
- **Comprehensive Item Database**: Pre-configured with all Valheim items (commented out by default)
- **Cross-Platform Compatible**: Works on Windows, macOS, and Linux builds

### How It Works
1. **Fresh Start**: All items auto-pickup normally when first installed
2. **Selective Ignoring**: Add specific items you don't want to auto-pickup using in-game controls
3. **Smart Detection**: Handles item name variations and "(Clone)" suffixes automatically
4. **Persistent Settings**: Your ignore list is saved and loads automatically

---

## 🛠️ Development Setup

### Prerequisites

- **.NET SDK** (for building on macOS/Linux)
- **Valheim game files** (for complete building)
- **BepInEx** (for mod framework)

### Building on macOS

1. **Clone/download this repository**

2. **Set up required libraries:**
   ```bash
   # Copy required DLL files to libs/ directory
   # See libs/README.md for detailed instructions
   ```

3. **Build the mod:**
   ```bash
   # Option 1: Use the build script
   ./build.sh
   
   # Option 2: Manual build
   dotnet restore
   dotnet build
   ```

### Required DLL Files

Place these files in the `libs/` directory:

| File | Source | Purpose |
|------|--------|---------|
| `0Harmony.dll` | BepInEx/core/ | Harmony patching framework |
| `assembly_valheim.dll` | Valheim/valheim_Data/Managed/ | Valheim game types |
| `BepInEx.dll` | BepInEx/core/ | BepInEx plugin framework |
| `UnityEngine.dll` | Valheim/valheim_Data/Managed/ | Unity engine core |
| `UnityEngine.CoreModule.dll` | Valheim/valheim_Data/Managed/ | Unity core module |
| `UnityEngine.InputLegacyModule.dll` | Valheim/valheim_Data/Managed/ | Unity input system |
| `UnityEngine.PhysicsModule.dll` | Valheim/valheim_Data/Managed/ | Unity physics system |

> **Note:** The project can build without these files using fallback assemblies, but the resulting DLL won't function without the actual game files.

---

## 🎮 Installation & Usage

### For Players

1. **Install BepInEx** for Valheim (version 5.4.22+ or 6.0.0+)
2. **Download the mod**: Get `ChoosyViking.dll` from releases
3. **Install the mod**: Copy `ChoosyViking.dll` to `BepInEx/plugins/` folder
4. **Optional Configuration**: Copy `shivam13juna.ChoosyViking.cfg` to `BepInEx/config/` (will be auto-generated if missing)
5. **Launch Valheim** and start using the in-game controls

### In-Game Controls

| Key Combination | Action |
|----------------|--------|
| **Click item + I** | Add selected inventory item to ignore list |
| **Click item + Shift + I** | Remove selected inventory item from ignore list |

### Configuration Options

**Method 1: In-Game (Recommended)**
- Click any item in your inventory
- Press `I` to add it to the ignore list
- Press `Shift + I` to remove it from the ignore list
- Get instant feedback with on-screen messages

**Method 2: Manual Config File (Advanced)**
Edit `BepInEx/config/shivam13juna.ChoosyViking.cfg`:
```ini
[Settings]
# Remove '#' from items you want to ignore auto-pickup
Items = #Stone,#Wood,#Resin,#Flint,Wood,Stone
```

**Method 3: Key Customization**
```ini
[Controls]
AddItemKey = I        # Key to add items to ignore list
RemoveItemKey = I     # Key used with Shift to remove items
```

---

## 🏗️ Project Structure

```
ChoosyViking/
├── README.md                              # Project documentation
├── CHANGELOG.md                           # Version history and changes
├── COMPATIBILITY.md                       # Version compatibility guide
├── REPO_FIXES_SUMMARY.md                  # Repository maintenance log
├── ChoosyViking.cs                        # Main plugin implementation
├── ChoosyViking.csproj                    # Modern SDK-style project file
├── ChoosyViking.sln                       # Visual Studio solution
├── AssemblyInfo.cs                       # Assembly metadata and versioning
├── app.config                            # .NET Framework configuration
├── shivam13juna.ChoosyViking.cfg         # Default plugin configuration
├── build.sh                              # Cross-platform build script
├── libs/                                 # External DLL dependencies
└── bin/                                  # Build output directory
    ├── Debug/net481/                     # Debug build artifacts
    └── Release/net481/                   # Release build artifacts
```

---

## 🔧 Technical Details

### Core Architecture
- **Target Framework**: .NET Framework 4.8.1
- **Plugin System**: BepInEx 5.4.22+ and 6.0.0+ compatible
- **Patching**: Uses HarmonyX for method interception
- **Build System**: Modern SDK-style project with cross-platform support

### Dependencies
| Component | Version | Purpose |
|-----------|---------|---------|
| **BepInEx.Core** | 5.4.21+ | Plugin framework and mod loader |
| **HarmonyX** | 2.10.1+ | Runtime method patching |
| **Valheim Game Assemblies** | 0.218.15+ | Game integration and API access |
| **Unity Engine** | From Valheim | Game engine components |

### Build Features
- ✅ **Cross-Platform Building**: Windows, macOS, and Linux support
- ✅ **Intelligent Dependency Resolution**: Automatic NuGet fallbacks when local DLLs are missing  
- ✅ **Flexible Assembly Loading**: Conditional inclusion of game assemblies
- ✅ **Modern Project Structure**: SDK-style project without nested directories
- ✅ **Comprehensive Error Handling**: Robust input system detection and fallbacks

### Technical Implementation
- **Harmony Patches**: Intercepts `Player.AutoPickup` method to control item collection
- **Input System**: Cross-compatible input handling with reflection-based fallbacks
- **Configuration Management**: Real-time config updates with live ignore list display
- **Item Name Resolution**: Handles multiple item name formats including "(Clone)" suffixes

---

## 📋 Compatibility & Dependencies

### System Requirements
- **Valheim**: Version 0.218.15+ (tested with latest)
- **BepInEx**: 5.4.22+ or 6.0.0+ (denikson-BepInExPack_Valheim recommended)
- **.NET Framework**: 4.8.1 (included with modern Windows, available for macOS/Linux)
- **Platform**: Windows, macOS, Linux (via Mono/Wine for macOS/Linux)

### Version History Highlights

#### **Version 2.2.0 - Current** (Simplified Single Mode)
- ✅ **Major Simplification**: Removed complex mode cycling, single behavior only
- ✅ **Empty by Default**: No items ignored initially, user-driven ignore list
- ✅ **Streamlined Controls**: Only `I` and `Shift+I` for ignore list management
- ✅ **Better Performance**: Removed mode-checking overhead

#### **Version 2.1.0** (Bug Fixes and Reliability)
- ✅ **Fixed Critical Harmony Errors**: Resolved mod loading failures
- ✅ **Enhanced Item Detection**: Improved pickup blocking with better name matching
- ✅ **Configuration Manager Integration**: Real-time ignore list display
- ✅ **Stability Improvements**: Better error handling and logging

#### **Version 2.0.0 and Earlier**
- ✅ **Cross-Platform Build Support**: macOS/Linux development capabilities
- ✅ **Modern BepInEx Compatibility**: Updated for latest framework versions
- ✅ **Comprehensive Item Database**: All Valheim items pre-configured
- ✅ **SDK-Style Project**: Modern .NET project structure

### Known Compatibility
- **Other Mods**: Generally compatible with most Valheim mods
- **Game Updates**: Designed to be resilient to Valheim updates
- **BepInEx Versions**: Supports both 5.x and 6.x branches

---

## 🤝 Contributing & Development

This project uses a modern, cross-platform development approach designed for maintainability and ease of contribution.

### Development Setup

#### Prerequisites
- **.NET SDK 6.0+** (for building)
- **Valheim Installation** (for game assemblies)
- **BepInEx** (for framework assemblies)
- **Git** (for version control)

#### Building the Project

**Option 1: Automated Build Script (Recommended)**
```bash
# macOS/Linux
./build.sh

# Windows
# Use Git Bash or WSL to run build.sh
```

**Option 2: Manual Build**
```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build --configuration Release

# Output will be in bin/Release/net481/
```

#### Required Assembly Files
Place these files in the `libs/` directory for complete functionality:

| File | Source Location | Required |
|------|----------------|----------|
| `assembly_valheim.dll` | `Valheim/valheim_Data/Managed/` | ✅ Critical |
| `BepInEx.dll` | `BepInEx/core/` | ⚠️ NuGet fallback available |
| `0Harmony.dll` | `BepInEx/core/` | ⚠️ NuGet fallback available |
| `UnityEngine*.dll` | `Valheim/valheim_Data/Managed/` | ⚠️ NuGet fallback available |

> **Note**: The build system uses NuGet packages as fallbacks when local DLLs are unavailable, but game-specific assemblies like `assembly_valheim.dll` are required for full functionality.

### Code Style & Architecture
- **Modern C#**: Uses latest language features available in .NET 4.8.1
- **Error-First Design**: Comprehensive error handling and logging
- **Cross-Platform Compatibility**: Designed to work across different operating systems
- **Clean Dependencies**: Minimal external dependencies with intelligent fallbacks

### Contributing Guidelines
1. **Fork the Repository** and create a feature branch
2. **Follow Existing Code Style** and architectural patterns
3. **Test Thoroughly** on different Valheim/BepInEx versions when possible
4. **Update Documentation** if adding new features or changing behavior
5. **Submit Pull Request** with clear description of changes

### Project Principles
- ✅ **User Experience First**: Prioritize intuitive, in-game interaction
- ✅ **Compatibility Focus**: Support multiple BepInEx and Valheim versions
- ✅ **Clean Code**: Maintainable, well-documented implementation  
- ✅ **Cross-Platform**: Build and run on Windows, macOS, and Linux

---

## 📄 License & Legal

### License
This project follows Valheim modding community standards. Please respect:
- **Valheim Terms of Service**: Ensure compliance with game's modding policies
- **BepInEx License**: Respect the BepInEx framework licensing terms
- **Open Source Spirit**: Consider contributing improvements back to the community

### Disclaimer
- This mod is provided "as-is" without warranties
- Use at your own risk - always backup save files before using mods
- Not affiliated with Iron Gate AB or Coffee Stain Studios
- For educational and entertainment purposes

### Community
- **Issues & Support**: Use GitHub Issues for bug reports and feature requests
- **Discussions**: Share feedback and suggestions via GitHub Discussions
- **Updates**: Watch the repository for updates and new releases

---

*Made with ⚡ for the Valheim community*