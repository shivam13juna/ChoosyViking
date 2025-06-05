# Valheim Item Auto Pickup Ignorer

### Author: stal4gmite

A Valheim BepInEx plugin that allows users to disable auto pickup of specific items.

**Version:** 1.0.1  
**Target:** .NET Framework 4.7.2  
**Compatibility:** Cross-platform building (Windows/macOS/Linux)

---

## 🎮 What does this mod do?

This plugin provides fine-grained control over Valheim's auto-pickup system:

- **3 modes**: Normal Valheim behavior, ignore specific items, or ignore nothing
- **Configurable item list**: Easily customize which items to ignore
- **In-game controls**: Press `Left Ctrl + L` to cycle through modes
- **Real-time feedback**: On-screen messages show current mode

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
3. **Copy configuration file** `stal4gmite.ItemAutoPickupIgnorer.cfg` to `BepInEx/config/`
4. **Configure items** by editing the config file (remove `#` to ignore items)
5. **Launch Valheim** and use `Left Ctrl + L` to cycle modes

### Configuration

Edit `stal4gmite.ItemAutoPickupIgnorer.cfg`:

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
ItemAutoPickupIgnorer/
├── README.md                              # This file
├── ItemAutoPickupIgnorer.cs               # Main plugin code
├── ItemAutoPickupIgnorer.csproj           # Modern SDK-style project file
├── ItemAutoPickupIgnorer.sln             # Visual Studio solution
├── AssemblyInfo.cs                       # Assembly metadata
├── app.config                            # .NET configuration
├── stal4gmite.ItemAutoPickupIgnorer.cfg  # Plugin configuration
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

- **Valheim** 0.148.7+ (tested version)
- **BepInEx** 5.4.901+ (denikson-BepInExPack_Valheim)
- **Harmony** (included with BepInEx)
- **Unity Engine** (from Valheim installation)

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