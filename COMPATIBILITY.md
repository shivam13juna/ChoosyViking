# Compatibility Guide

## Valheim & BepInEx Version Compatibility

### ✅ **Supported Versions**

#### **Valheim**
| Version | Status | Notes |
|---------|--------|-------|
| 0.218.15+ | ✅ Fully Supported | Latest tested version |
| 0.217.x | ✅ Compatible | Should work without issues |
| 0.216.x | ✅ Compatible | Core functionality maintained |
| 0.215.x | ⚠️ Mostly Compatible | Some features may not work |
| 0.214.x and older | ❌ Not Recommended | May have compatibility issues |

#### **BepInEx**
| Version | Status | Notes |
|---------|--------|-------|
| 6.0.0+ | ✅ Fully Supported | Future-ready |
| 5.4.22+ | ✅ Fully Supported | Recommended |
| 5.4.21 | ✅ Compatible | Works well |
| 5.4.20 | ⚠️ Limited | Some features may not work |
| 5.4.19 and older | ❌ Not Supported | Please update |

#### **.NET Framework**
| Version | Status | Notes |
|---------|--------|-------|
| 4.8.1 | ✅ Target Version | Optimized for this version |
| 4.8.0 | ✅ Compatible | Works well |
| 4.7.2 | ⚠️ Older Target | Previous version support |
| 4.7.1 and older | ❌ Not Supported | Please update |

### 🔧 **Installation Requirements**

#### **Minimum System Requirements**
- **Valheim**: Version 0.215.2 or later
- **BepInEx**: Version 5.4.21 or later
- **.NET Framework**: 4.7.2 or later (4.8.1 recommended)
- **Operating System**: Windows 10/11, macOS 10.15+, or Linux (Ubuntu 18.04+)

#### **Recommended Setup**
- **Valheim**: Latest stable version (0.218.15+)
- **BepInEx**: Latest stable version (5.4.22+)
- **.NET Framework**: 4.8.1
- **HarmonyX**: 2.10.1+ (included with BepInEx)

### 🚨 **Known Issues & Solutions**

#### **Common Problems**

**Problem**: Mod doesn't load
- **Solution**: Ensure BepInEx is properly installed and you're using version 5.4.21+
- **Check**: Look for the mod in the BepInEx console output

**Problem**: Key bindings don't work
- **Solution**: Update your config file to include the `[Controls]` section
- **Example**:
  ```ini
  [Controls]
  ToggleKey = L
  ModifierKey = LeftControl
  ```

**Problem**: Items still auto-pickup when they shouldn't
- **Solution**: Check the item names in your config. They should match exactly (case-sensitive)
- **Tip**: Enable debug logging to see item names

**Problem**: Mod crashes the game
- **Solution**: Check BepInEx logs for errors. Usually indicates a version mismatch
- **Fix**: Update to compatible versions listed above

### 🔄 **Migration from v1.0.x**

#### **Step-by-Step Upgrade**

1. **Backup Current Setup**
   ```bash
   # Backup your current config
   cp BepInEx/config/shivam13juna.ChoosyViking.cfg ~/backup_config.cfg
   ```

2. **Update BepInEx** (if needed)
   - Download latest BepInEx from [official releases](https://github.com/BepInEx/BepInEx/releases)
   - Follow BepInEx installation guide

3. **Install New Mod Version**
   ```bash
   # Remove old version
   rm BepInEx/plugins/ChoosyViking.dll
   
   # Install new version
   cp ChoosyViking.dll BepInEx/plugins/
   cp shivam13juna.ChoosyViking.cfg BepInEx/config/
   ```

4. **Update Configuration**
   - The new version adds a `[Controls]` section
   - Your item list settings will be preserved
   - New keybind settings will use defaults if not specified

### 🛠️ **Troubleshooting**

#### **Debug Mode**
To enable detailed logging:
1. Open BepInEx config: `BepInEx/config/BepInEx.cfg`
2. Set `LogLevel = All`
3. Restart Valheim
4. Check logs in `BepInEx/LogOutput.log`

#### **Verification Steps**
1. **Check BepInEx Console**: Should show mod loading
2. **Test Key Bindings**: Try Ctrl+L in game
3. **Check Item Pickup**: Test with configured items
4. **Review Logs**: Look for any error messages

#### **Getting Help**
If you encounter issues:
1. Check this compatibility guide
2. Review the logs for error messages
3. Ensure you're using supported versions
4. Check if other mods are conflicting

### 🔮 **Future Updates**

#### **Planned Compatibility**
- **BepInEx 6.0**: Full support when stable
- **Valheim Updates**: Continuous compatibility updates
- **Unity Upgrades**: Forward compatibility built-in

#### **Deprecation Schedule**
- **Valheim < 0.215**: Support ends June 2025
- **BepInEx < 5.4.21**: Support ends March 2025
- **.NET < 4.7.2**: Support ends December 2024

### 📞 **Support Matrix**

| Component | Supported | Recommended | End of Life |
|-----------|-----------|-------------|-------------|
| Valheim 0.218.x | ✅ | ✅ | TBD |
| Valheim 0.217.x | ✅ | ✅ | Jun 2025 |
| Valheim 0.216.x | ⚠️ | ❌ | Mar 2025 |
| BepInEx 5.4.22+ | ✅ | ✅ | TBD |
| BepInEx 5.4.21 | ✅ | ⚠️ | Jun 2025 |
| BepInEx 5.4.20 | ⚠️ | ❌ | Mar 2025 |
