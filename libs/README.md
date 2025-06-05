# Required Libraries for Valheim Mod Development

This directory should contain the following DLL files for building the mod:

## Required Files:
- `0Harmony.dll` - From BepInEx installation
- `assembly_valheim.dll` - From Valheim game files
- `BepInEx.dll` - From BepInEx installation
- `UnityEngine.dll` - From Valheim game files
- `UnityEngine.CoreModule.dll` - From Valheim game files
- `UnityEngine.InputLegacyModule.dll` - From Valheim game files
- `UnityEngine.PhysicsModule.dll` - From Valheim game files

## Where to Find These Files:

### BepInEx Files:
- Download BepInEx 5.4.21 or later from [BepInEx Releases](https://github.com/BepInEx/BepInEx/releases)
- Extract and find:
  - `0Harmony.dll` in `BepInEx/core/`
  - `BepInEx.dll` in `BepInEx/core/`

### Valheim Game Files:
From your Valheim installation directory:
- `assembly_valheim.dll` in `valheim_Data/Managed/`
- Unity engine DLLs in `valheim_Data/Managed/`

### Typical Paths:
**Steam (Windows):**
```
C:\Program Files (x86)\Steam\steamapps\common\Valheim\valheim_Data\Managed\
```

**Steam (macOS):**
```
~/Library/Application Support/Steam/steamapps/common/Valheim/valheim.app/Contents/Resources/Data/Managed/
```

**Steam (Linux):**
```
~/.steam/steam/steamapps/common/Valheim/valheim_Data/Managed/
```

## Building Without Game Files:
If you don't have access to the actual game files, you can:
1. Use Unity reference assemblies for Unity-related DLLs
2. Download BepInEx publicly available assemblies
3. Create stub assemblies for development (build may succeed but mod won't work without real DLLs)

## Note:
These DLL files are copyrighted by their respective owners and should not be redistributed.
Only use them for personal mod development.
