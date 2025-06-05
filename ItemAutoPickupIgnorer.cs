using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace ItemAutoPickupIgnorer
{
    [BepInPlugin(PluginGUID, PluginName, ModVersion)]
    [BepInProcess("valheim.exe")]
    public class ItemAutoPickupIgnorer : BaseUnityPlugin
    {
        public const string PluginGUID = "stal4gmite.ItemAutoPickupIgnorer";
        public const string PluginName = "Item Auto Pickup Ignorer";
        public const string ModVersion = "1.1.0";

        public enum ItemAutoPickupIgnorerMode { VALHEIM_DEFAULT, IGNORE_SOME, IGNORE_NONE }

        private readonly Harmony harmony = new Harmony(PluginGUID);
        private ConfigEntry<string> items;
        private ConfigEntry<KeyCode> toggleKey;
        private ConfigEntry<KeyCode> modifierKey;
        
        public static IEnumerable<string> ItemsToIgnore = new List<string> { "Stone(Clone)" };
        public static ItemAutoPickupIgnorerMode Mode = ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT;
        public static ManualLogSource logger;

        void Awake()
        {
            logger = Logger;
            
            try
            {
                harmony.PatchAll();
                Logger.LogInfo($"{PluginName} {ModVersion} loaded successfully!");
                
                // Configuration
                items = Config.Bind("Settings",
                                    "Items",
                                    string.Empty,
                                    "List of items. Remove '#' to ignore the item.");

                toggleKey = Config.Bind("Controls",
                                       "ToggleKey",
                                       KeyCode.L,
                                       "Key to toggle pickup modes");

                modifierKey = Config.Bind("Controls",
                                         "ModifierKey", 
                                         KeyCode.LeftControl,
                                         "Modifier key that must be held while pressing toggle key");

                ItemsToIgnore = items.Value.Split(',')
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrEmpty(i) && !i.StartsWith("#"))
                    .Select(i => $"{i}(Clone)");
                    
                Logger.LogInfo($"Configured to ignore {ItemsToIgnore.Count()} item types");
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error during Awake: {ex}");
            }
        }

        void Update()
        {
            try
            {
                if (Player.m_localPlayer == null) return;
                
                if (Input.GetKey(modifierKey.Value))
                {
                    if (Input.GetKeyDown(toggleKey.Value))
                    {
                        CycleMode();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error in Update: {ex}");
            }
        }

        private void CycleMode()
        {
            switch (Mode)
            {
                case ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT:
                    Mode = ItemAutoPickupIgnorerMode.IGNORE_SOME;
                    break;
                case ItemAutoPickupIgnorerMode.IGNORE_SOME:
                    Mode = ItemAutoPickupIgnorerMode.IGNORE_NONE;
                    break;
                case ItemAutoPickupIgnorerMode.IGNORE_NONE:
                    Mode = ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT;
                    break;
            }

            string modeText = GetModeDisplayText(Mode);
            Logger.LogInfo($"Switched to mode: {modeText}");
            
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"Item Auto Pickup Ignorer: {modeText}");
            }
        }

        string GetModeDisplayText(ItemAutoPickupIgnorerMode mode)
        {
            return mode switch
            {
                ItemAutoPickupIgnorerMode.IGNORE_SOME => "Ignoring Items",
                ItemAutoPickupIgnorerMode.IGNORE_NONE => "Ignoring Nothing", 
                ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT => "Normal Valheim Behavior",
                _ => "Unknown Behavior"
            };
        }

        void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(ItemDrop), "Pickup")]
        class ItemDropPickupPatch
        {
            // Patch the actual pickup method to control behavior
            static bool Prefix(ItemDrop __instance, Humanoid character)
            {
                try
                {
                    if (Mode == ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT)
                        return true; // Let normal behavior happen

                    if (character == null || !(character is Player))
                        return true; // Only affect players

                    // If we're ignoring some items and this is one of them, don't auto-pickup
                    if (Mode == ItemAutoPickupIgnorerMode.IGNORE_SOME)
                    {
                        if (ItemsToIgnore.Contains(__instance.name))
                        {
                            // Only block if this is an auto-pickup (not manual)
                            // We can detect this by checking if the player is close but not actively trying to pickup
                            Player player = character as Player;
                            if (player != null && !player.InPlaceMode() && !Input.GetKey(KeyCode.E))
                            {
                                return false; // Block auto-pickup
                            }
                        }
                    }
                    // For IGNORE_NONE mode, always allow pickup
                    return true;
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in ItemDrop.Pickup patch: {ex}");
                    return true; // Fallback to normal behavior
                }
            }
        }

        // Alternative approach: Patch the Player.AutoPickup method more carefully
        [HarmonyPatch(typeof(Player), "AutoPickup")]
        class PlayerAutoPickupPatch
        {
            static bool Prefix(Player __instance, float dt)
            {
                try
                {
                    if (Mode == ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT)
                        return true; // Use default Valheim behavior

                    if (__instance == null || __instance.IsTeleporting())
                        return true;

                    // For IGNORE_NONE mode, force enable all auto pickups in range
                    if (Mode == ItemAutoPickupIgnorerMode.IGNORE_NONE)
                    {
                        ForceEnableAutoPickup(__instance);
                        return true; // Let normal pickup logic continue
                    }

                    // For IGNORE_SOME mode, disable auto pickup for ignored items
                    if (Mode == ItemAutoPickupIgnorerMode.IGNORE_SOME)
                    {
                        DisableIgnoredItemsAutoPickup(__instance);
                        return true; // Let normal pickup logic continue
                    }

                    return true;
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in Player.AutoPickup patch: {ex}");
                    return true; // Fallback to normal behavior
                }
            }

            private static void ForceEnableAutoPickup(Player player)
            {
                try
                {
                    float range = 2f; // Default range
                    Vector3 position = player.transform.position + Vector3.up;
                    Collider[] colliders = Physics.OverlapSphere(position, range, LayerMask.GetMask("item"));

                    foreach (Collider collider in colliders)
                    {
                        if (collider?.attachedRigidbody == null) continue;

                        ItemDrop itemDrop = collider.attachedRigidbody.GetComponent<ItemDrop>();
                        if (itemDrop == null) continue;

                        ZNetView netView = itemDrop.GetComponent<ZNetView>();
                        if (netView == null || !netView.IsValid()) continue;

                        itemDrop.m_autoPickup = true;
                    }
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in ForceEnableAutoPickup: {ex}");
                }
            }

            private static void DisableIgnoredItemsAutoPickup(Player player)
            {
                try
                {
                    float range = 2f; // Default range
                    Vector3 position = player.transform.position + Vector3.up;
                    Collider[] colliders = Physics.OverlapSphere(position, range, LayerMask.GetMask("item"));

                    foreach (Collider collider in colliders)
                    {
                        if (collider?.attachedRigidbody == null) continue;

                        ItemDrop itemDrop = collider.attachedRigidbody.GetComponent<ItemDrop>();
                        if (itemDrop == null) continue;

                        ZNetView netView = itemDrop.GetComponent<ZNetView>();
                        if (netView == null || !netView.IsValid()) continue;

                        if (ItemsToIgnore.Contains(itemDrop.name))
                        {
                            itemDrop.m_autoPickup = false;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in DisableIgnoredItemsAutoPickup: {ex}");
                }
            }
        }
    }
}
