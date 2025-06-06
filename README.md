# Choosy Viking

### Author: shivam13juna

A Valheim BepInEx plugin that allows users to disable auto pickup of specific items.

**Version:** 1.1.0  
**Target:** .NET Framework 4.8.1  
**Compatibility:** Cross-platform building (Windows/macOS/Linux)  
**BepInEx Version:** 5.4.22+ (Compatible with BepInEx 6.0+)  
**Valheim Version:** 0.218.15+ (Latest tested)

---

## 🎮 What does this mod do?

This plugin provides fine-grained control over Valheim's auto-pickup system:

- **3 modes**: Normal Valheim behavior, ignore specific items, or ignore nothing
- **Configurable item list**: Easily customize which items to ignore
- **Configurable controls**: Customize toggle and modifier keys
- **In-game controls**: Press `Left Ctrl + L` to cycle through modes (configurable)
- **Real-time feedback**: On-screen messages show current mode
- **Improved compatibility**: Updated for latest Valheim and BepInEx versions
- **Better error handling**: Comprehensive logging and error recovery

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

1. **Install BepInEx** for Valheim
2. **Copy the mod DLL** to `BepInEx/plugins/`
3. **Copy configuration file** `shivam13juna.ChoosyViking.cfg` to `BepInEx/config/`
4. **Configure items** by editing the config file (remove `#` to ignore items)
5. **Launch Valheim** and use `Left Ctrl + L` to cycle modes

### Configuration

Edit `shivam13juna.ChoosyViking.cfg`:

```ini
# Remove '#' from items you want to ignore auto-pickup
Items = #Amber,#Stone,#Wood,#Coal,#Resin,...
```

### In-Game Controls

- **Left Ctrl + L**: Cycle through modes
  - **Normal Valheim Behavior**: Default game auto-pickup
  - **Ignoring Items**: Skip configured items
  - **Ignoring Nothing**: Force pickup everything

---

## 🏗️ Project Structure

```
ChoosyViking/
├── README.md                              # This file
├── ChoosyViking.cs                        # Main plugin code
├── ChoosyViking.csproj                    # Modern SDK-style project file
├── ChoosyViking.sln                       # Visual Studio solution
├── AssemblyInfo.cs                       # Assembly metadata
├── app.config                            # .NET configuration
├── shivam13juna.ChoosyViking.cfg         # Plugin configuration
├── build.sh                              # macOS/Linux build script
├── libs/                                 # Required DLL files
│   └── README.md                         # Instructions for obtaining DLLs
└── .gitignore                            # Git ignore rules
```

---

## 🔧 Technical Details

- **Target Framework**: .NET Framework 4.7.2
- **Build System**: Modern SDK-style project with cross-platform support
- **Dependencies**: BepInEx 5.x, Harmony, Unity Engine, Valheim assemblies
- **Architecture**: Harmony patches on Player.AutoPickup method

### Build Features

- ✅ Cross-platform building (Windows/macOS/Linux)
- ✅ Automatic fallback to Unity reference assemblies
- ✅ Conditional DLL inclusion
- ✅ Clean project structure without nested directories
- ✅ Modern MSBuild project format

---

## 📋 Dependencies

- **Valheim** 0.218.15+ (latest tested - should work with older versions)
- **BepInEx** 5.4.22+ or 6.0+ (denikson-BepInExPack_Valheim)
- **HarmonyX** 2.10.1+ (included with BepInEx or via NuGet)
- **Unity Engine** (from Valheim installation)

### What's New in v1.1.0

- ✅ **Updated for latest Valheim and BepInEx**: Compatible with current game versions
- ✅ **Improved Harmony patching**: More stable and less intrusive approach
- ✅ **Configurable keybinds**: Customize toggle and modifier keys
- ✅ **Better error handling**: Comprehensive logging and graceful error recovery
- ✅ **Modern BepInEx practices**: Updated to use latest BepInEx features
- ✅ **Enhanced logging**: Better debugging and troubleshooting information
- ✅ **.NET 4.8.1 target**: Updated from 4.7.2 for better compatibility

---

## 🤝 Contributing

This project uses a clean, modern structure suitable for cross-platform development:

1. All source files are at the root level
2. DLL dependencies are conditionally loaded
3. Build scripts handle missing dependencies gracefully
4. SDK-style project enables modern .NET tooling

---

## 📄 License

Check the original mod license and Valheim's modding terms.