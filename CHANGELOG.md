# Changelog

## Version 2.2.0 - Simplified Single Mode

### 🎯 **Major Simplification**
- **Removed Multiple Modes**: No more mode cycling - mod now has single behavior
- **Always Ignore Items**: Mod only ignores items that are in your ignore list
- **Empty by Default**: No items are ignored initially - all auto-pickup works normally
- **Simpler Controls**: Removed `Ctrl+L` mode cycling, kept only `I` and `Shift+I` for managing ignore list

### ⚙️ **Behavior Changes**
- **Single Mode Only**: Mod always checks ignore list and blocks those items from auto-pickup
- **Clean Startup**: Starts with empty ignore list, user adds items as needed
- **Streamlined Logic**: Removed complex mode switching and multiple behaviors

### 🔧 **Code Improvements**
- **Removed Mode Enum**: Eliminated `ChoosyVikingMode` and related code
- **Simplified Patches**: Harmony patches now only handle ignore list logic
- **Cleaner Configuration**: Removed toggle key and modifier key settings
- **Better Performance**: Less overhead without mode checking

### 📝 **User Experience**
- **Easier to Understand**: One simple behavior - ignore items you don't want
- **Faster Setup**: No need to understand different modes
- **More Intuitive**: Works like most users expect - ignore specific items only

---

## Version 2.1.0 - Bug Fixes and Default Mode Change

### 🐛 **Critical Bug Fixes**
- **Fixed Harmony Patching Error**: Removed incompatible `PlayerUseItemPatch` that was causing mod load failures
- **Enhanced Item Name Matching**: Improved pickup blocking logic with multiple name format checks
- **Robust Item Detection**: Added support for GameObject names, ItemData names, and "(Clone)" suffix variations

### ⚙️ **Behavior Changes**
- **Default Mode**: Mod now starts in "Ignoring Items" mode by default instead of "Normal Valheim Behavior"
- **Improved Reliability**: Better item pickup blocking with comprehensive name matching logic

### 📊 **Configuration Manager Integration**
- **Real-time Display**: Added "Currently Ignored Items" status in configuration manager
- **Live Updates**: Ignored items list updates immediately when items are added/removed
- **Better UX**: View current ignore list without opening inventory

### 🔧 **Technical Improvements**
- **Enhanced Debugging**: Added detailed logging for item name comparisons
- **Code Cleanup**: Removed unused and problematic Harmony patches
- **Stability**: Improved overall mod reliability and error handling

---

## Version 2.0.0 - Interactive Item Management

### 🎯 **Major New Features**

#### ✅ **Click-to-Select Item Management**
- **Interactive Item Addition**: Click on any item in your inventory, then press `I` to add it to the ignore list
- **Interactive Item Removal**: Click on any item in your inventory, then press `Shift + I` to remove it from the ignore list
- **Real-time Feedback**: Instant confirmation messages when items are added or removed
- **Persistent Settings**: Items added/removed are immediately saved to configuration

#### 🎮 **Improved User Experience**
- **No More Config File Editing**: Add and remove items directly in-game
- **Visual Confirmation**: Clear messages show when items are added/removed
- **Error Prevention**: Smart validation prevents duplicate additions
- **Ergonomic Controls**: Simple click + key press interaction

#### ⚙️ **Enhanced Configuration**
```ini
[Controls]
ToggleKey = L              # Ctrl+L: cycle pickup modes (unchanged)
ModifierKey = LeftControl  # Ctrl modifier (unchanged)
AddItemKey = I             # I: add selected item to ignore list (NEW)
RemoveItemKey = I          # Shift+I: remove selected item from ignore list (NEW)
```

### 🚀 **How to Use New Features**

#### **Adding Items to Ignore List:**
1. Open your inventory (`Tab`)
2. Click on any item you want to ignore
3. Press `I` key
4. See confirmation: "✓ [ItemName] added to ignore list"

#### **Removing Items from Ignore List:**
1. Open your inventory (`Tab`)  
2. Click on any item you want to stop ignoring
3. Press `Shift + I`
4. See confirmation: "✗ [ItemName] removed from ignore list"

#### **Existing Features Still Work:**
- `Ctrl + L`: Cycle through pickup modes (Normal/Ignore Some/Ignore Nothing)
- Manual config file editing (if preferred)

### 🔧 **Technical Improvements**
- **Smart Config Updates**: Automatically manages configuration file
- **Harmony Patches**: Added inventory interaction tracking
- **Error Handling**: Comprehensive error checking and user feedback
- **Backward Compatibility**: Existing configurations work unchanged

### 📋 **Migration from v1.x**
- **Automatic**: No manual migration needed
- **Config Preserved**: Existing ignore lists remain intact
- **New Controls**: Additional keybinds added automatically
- **Full Compatibility**: All v1.x features continue to work

---

## Version 1.1.0 - Latest Valheim & BepInEx Compatibility Update + Rebranding

### 🎯 **Rebranding**
- **New Name**: "Choosy Viking" (formerly "Item Auto Pickup Ignorer")
- **New Author**: shivam13juna (originally by stal4gmite)
- **New Plugin GUID**: shivam13juna.ChoosyViking
- **New Config File**: shivam13juna.ChoosyViking.cfg

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
