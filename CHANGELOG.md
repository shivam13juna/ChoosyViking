# Changelog

## Version 1.1.0 - Latest Valheim & BepInEx Compatibility Update

### 🚀 Major Improvements

#### ✅ **Enhanced Compatibility**
- **Target Framework**: Updated from .NET Framework 4.7.2 to 4.8.1 for better compatibility
- **BepInEx Support**: Now compatible with BepInEx 5.4.22+ and BepInEx 6.0+
- **Valheim Support**: Tested with latest Valheim versions (0.218.15+)
- **HarmonyX**: Updated to use modern HarmonyX 2.10.1+ for better patching stability

#### 🔧 **Improved Architecture**
- **Better Harmony Patching**: Replaced intrusive Prefix patch with safer dual-patch approach
- **Multiple Patch Points**: Added patches to both `Player.AutoPickup` and `ItemDrop.Pickup` for better compatibility
- **Reflection Fallbacks**: Removed problematic reflection access to private fields
- **Error Handling**: Comprehensive try-catch blocks with proper logging

#### ⚙️ **Enhanced Configuration**
- **Configurable Keybinds**: Toggle key and modifier key are now configurable
- **Better Defaults**: Improved default settings for wider compatibility
- **Modern BepInEx Practices**: Updated configuration handling

#### 📦 **Build System Improvements**
- **NuGet Integration**: Added BepInEx NuGet packages as fallbacks
- **Smarter Dependencies**: Only require essential DLLs, use NuGet for others
- **Better Build Script**: Enhanced build script with clearer status messages
- **Cross-Platform**: Improved cross-platform building support

### 🐛 **Bug Fixes**
- **Field Access Issues**: Removed direct access to `m_autoPickupRange` and `m_autoPickupMask` which changed in newer Valheim versions
- **Null Reference Protection**: Added comprehensive null checking
- **Memory Leaks**: Proper cleanup with `OnDestroy` method
- **Input Handling**: More robust input detection

### 🔍 **Technical Changes**

#### **Code Structure**
```csharp
// Old approach (problematic)
[HarmonyPatch(typeof(Player), "AutoPickup")]
static void Prefix(ref float ___m_autoPickupRange, ref int ___m_autoPickupMask, Player __instance)

// New approach (robust)
[HarmonyPatch(typeof(Player), "AutoPickup")]
static bool Prefix(Player __instance, float dt)
// + 
[HarmonyPatch(typeof(ItemDrop), "Pickup")]
static bool Prefix(ItemDrop __instance, Humanoid character)
```

#### **Configuration Enhancements**
```csharp
// Added configurable controls
private ConfigEntry<KeyCode> toggleKey;
private ConfigEntry<KeyCode> modifierKey;
```

#### **Error Handling**
```csharp
// Comprehensive logging
public static ManualLogSource logger;
// Wrapped all major operations in try-catch
```

### 📋 **Dependencies Updated**
- **BepInEx**: 5.4.22+ (was: 5.4.901+)
- **HarmonyX**: 2.10.1+ (was: Harmony 2.x)
- **.NET Framework**: 4.8.1 (was: 4.7.2)
- **Valheim**: 0.218.15+ (was: 0.148.7+)

### 🎯 **Migration Guide**

If you're upgrading from v1.0.1:

1. **Backup your config**: The configuration format has slightly changed
2. **Update BepInEx**: Ensure you're running BepInEx 5.4.22 or later
3. **Replace DLL**: Replace the old DLL with the new one
4. **Update Config**: Copy the new config file or add the `[Controls]` section:
   ```ini
   [Controls]
   ToggleKey = L
   ModifierKey = LeftControl
   ```

### 🔮 **Future Compatibility**
This version is designed to be forward-compatible with:
- Future Valheim updates
- BepInEx 6.0+ when it becomes stable
- Unity engine updates
- Modern .NET Framework versions

### 📝 **Notes**
- All changes are backward compatible with existing configurations
- Performance improvements through better patching strategy
- More robust error handling prevents mod crashes
- Enhanced logging for better troubleshooting
