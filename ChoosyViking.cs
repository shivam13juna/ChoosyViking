using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.Reflection;

namespace ChoosyViking
{    [BepInPlugin(PluginGUID, PluginName, ModVersion)]
    [BepInProcess("valheim.exe")]
    public class ChoosyViking : BaseUnityPlugin
    {
        public const string PluginGUID = "shivam13juna.ChoosyViking";
        public const string PluginName = "Choosy Viking";
        public const string ModVersion = "2.2.0";

        private readonly Harmony harmony = new Harmony(PluginGUID);
        private ConfigEntry<string> items;
        private ConfigEntry<string> currentIgnoredItems; // Config entry to show current state
        private ConfigEntry<KeyCode> addItemKey;
        private ConfigEntry<KeyCode> removeItemKey;
        
        // Input system fallback
        private static System.Type inputType;
        private static MethodInfo getKeyMethod;
        private static MethodInfo getKeyDownMethod;
        private static bool inputSystemAvailable = false;
          // Selected item tracking
        public static ItemDrop.ItemData selectedItem = null;
        public static IEnumerable<string> ItemsToIgnore = new List<string>();
        public static ManualLogSource logger;
        public static ChoosyViking Instance;        void Awake()
        {
            Instance = this;
            logger = Logger;
            
            try
            {
                // Initialize input system
                InitializeInputSystem();
                
                harmony.PatchAll();
                Logger.LogInfo($"{PluginName} {ModVersion} loaded successfully!");
                Logger.LogInfo("Mod will ignore items in the ignore list (empty by default)");// Configuration
                items = Config.Bind("Settings",
                                    "Items",
                                    GetDefaultItemList(),
                                    "List of items. Remove '#' to ignore the item.");                currentIgnoredItems = Config.Bind("Status",
                                                 "CurrentlyIgnoredItems",
                                                 "None",
                                                 "Currently ignored items (READ-ONLY - managed automatically)");

                addItemKey = Config.Bind("Controls",
                                        "AddItemKey",
                                        KeyCode.I,
                                        "Key to add selected item to ignore list");

                removeItemKey = Config.Bind("Controls",
                                           "RemoveItemKey",
                                           KeyCode.I,
                                           "Key to remove selected item from ignore list (use with Shift)");ItemsToIgnore = items.Value.Split(',')
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrEmpty(i) && !i.StartsWith("#"))
                    .Select(i => $"{i}(Clone)");
                    
                Logger.LogInfo($"Configured to ignore {ItemsToIgnore.Count()} item types");
                
                // Update the display of currently ignored items
                UpdateCurrentIgnoredItemsDisplay();
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error during Awake: {ex}");
            }
        }        private string GetDefaultItemList()
        {
            // Comprehensive list of all Valheim items that players might want to ignore
            // All prefixed with '#' so they're commented out by default
            // Users can remove the '#' to ignore specific items
            return "#Acorn,#Amber,#AmberPearl,#AncientSeed,#ArmorAshlandsMediumChest,#ArmorAshlandsMediumlegs,#ArmorBronzeChest,#ArmorBronzeLegs,#ArmorCarapaceChest,#ArmorCarapaceLegs,#ArmorDress1,#ArmorDress10,#ArmorDress2,#ArmorDress3,#ArmorDress4,#ArmorDress5,#ArmorDress6,#ArmorDress7,#ArmorDress8,#ArmorDress9,#ArmorFenringChest,#ArmorFenringLegs,#ArmorFlametalChest,#ArmorFlametalLegs,#ArmorHarvester1,#ArmorHarvester2,#ArmorIronChest,#ArmorIronLegs,#ArmorLeatherChest,#ArmorLeatherLegs,#ArmorMageChest,#ArmorMageChest_Ashlands,#ArmorMageLegs,#ArmorMageLegs_Ashlands,#ArmorPaddedCuirass,#ArmorPaddedGreaves,#ArmorRagsChest,#ArmorRagsLegs,#ArmorRootChest,#ArmorRootLegs,#ArmorTrollLeatherChest,#ArmorTrollLeatherLegs,#ArmorTunic1,#ArmorTunic10,#ArmorTunic2,#ArmorTunic3,#ArmorTunic4,#ArmorTunic5,#ArmorTunic6,#ArmorTunic7,#ArmorTunic8,#ArmorTunic9,#ArmorWolfChest,#ArmorWolfLegs,#ArrowBronze,#ArrowCarapace,#ArrowCharred,#ArrowFire,#ArrowFlint,#ArrowFrost,#ArrowIron,#ArrowNeedle,#ArrowObsidian,#ArrowPoison,#ArrowSilver,#ArrowWood,#AskBladder,#AskHide,#AsksvinCarrionNeck,#AsksvinCarrionPelvic,#AsksvinCarrionRibcage,#AsksvinCarrionSkull,#AsksvinEgg,#AsksvinMeat,#AtgeirBlackmetal,#AtgeirBronze,#AtgeirHimminAfl,#AtgeirIron,#AxeBerzerkr,#AxeBerzerkrBlood,#AxeBerzerkrLightning,#AxeBerzerkrNature,#AxeBlackMetal,#AxeBronze,#AxeFlint,#AxeIron,#AxeJotunBane,#AxeStone,#BarberKit,#Barley,#BarleyFlour,#BarleyWine,#BarleyWineBase,#BarrelRings,#Battleaxe,#BattleaxeCrystal,#BeechSeeds,#Bell,#BellFragment,#BeltStrength,#Bilebag,#BirchSeeds,#BlackCore,#BlackMarble,#BlackMetal,#BlackMetalScrap,#BlackSoup,#Blackwood,#Bloodbag,#BloodPudding,#Blueberries,#BoarJerky,#BoltBlackmetal,#BoltBone,#BoltCarapace,#BoltCharred,#BoltIron,#BombBile,#BombLava,#BombOoze,#BombSiege,#BombSmoke,#BoneFragments,#BoneMawSerpentMeat,#BonemawSerpentScale,#BonemawSerpentTooth,#Bow,#BowAshlands,#BowAshlandsBlood,#BowAshlandsRoot,#BowAshlandsStorm,#BowDraugrFang,#BowFineWood,#BowHuntsman,#BowSpineSnap,#Bread,#BreadDough,#Bronze,#BronzeNails,#BronzeScrap,#BugMeat,#CandleWick,#CapeAsh,#CapeAsksvin,#CapeDeerHide,#CapeFeather,#CapeLinen,#CapeLox,#CapeOdin,#CapeTrollHide,#CapeWolf,#Carapace,#Carrot,#CarrotSeeds,#CarrotSoup,#Catapult_ammo,#CelestialFeather,#CeramicPlate,#Chain,#CharcoalResin,#CharredBone,#CharredCogwheel,#Charredskull,#chest_hildir1,#chest_hildir2,#chest_hildir3,#ChickenEgg,#ChickenMeat,#Chitin,#Cloudberry,#Club,#Coal,#Coins,#CookedAsksvinMeat,#CookedBoneMawSerpentMeat,#CookedBugMeat,#CookedChickenMeat,#CookedDeerMeat,#CookedEgg,#CookedHareMeat,#CookedLoxMeat,#CookedMeat,#CookedVoltureMeat,#CookedWolfMeat,#Copper,#CopperOre,#CopperScrap,#CrossbowArbalest,#CrossbowRipper,#CrossbowRipperBlood,#CrossbowRipperLightning,#CrossbowRipperNature,#CryptKey,#Crystal,#Cultivator,#CuredSquirrelHamstring,#Dandelion,#DeerHide,#DeerMeat,#DeerStew,#Demister,#DragonEgg,#DragonTear,#DvergerArbalest,#DvergerArbalest_shoot,#DvergerArbalest_shootAshlands,#DvergrKey,#DvergrKeyFragment,#DvergrNeedle,#DyrnwynBladeFragment,#DyrnwynHiltFragment,#DyrnwynTipFragment,#Eitr,#ElderBark,#Entrails,#Eyescream,#FaderDrop,#FeastAshlands,#FeastAshlands_Material,#FeastBlackforest,#FeastBlackforest_Material,#Feaster,#FeastMeadows,#FeastMeadows_Material,#FeastMistlands,#FeastMistlands_Material,#FeastMountains,#FeastMountains_Material,#FeastOceans,#FeastOceans_Material,#FeastPlains,#FeastPlains_Material,#FeastSwamps,#FeastSwamps_Material,#Feathers,#Fiddleheadfern,#FierySvinstew,#FineWood,#FirCone,#FireworksRocket_Blue,#FireworksRocket_Cyan,#FireworksRocket_Green,#FireworksRocket_Purple,#FireworksRocket_Red,#FireworksRocket_White,#FireworksRocket_Yellow,#FishAndBread,#FishAndBreadUncooked,#FishAnglerRaw,#FishCooked,#FishingBait,#FishingBaitAshlands,#FishingBaitCave,#FishingBaitDeepNorth,#FishingBaitForest,#FishingBaitMistlands,#FishingBaitOcean,#FishingBaitPlains,#FishingBaitSwamp,#FishingRod,#FishRaw,#FishWraps,#FistFenrirClaw,#Flametal,#FlametalNew,#FlametalOre,#FlametalOreNew,#Flax,#Flint,#FragrantBundle,#FreezeGland,#FreshSeaweed,#GemstoneBlue,#GemstoneGreen,#GemstoneRed,#GiantBloodSack,#GoblinTotem,#Grausten,#GreydwarfEye,#Guck,#Hammer,#HardAntler,#HareMeat,#HelmetAshlandsMediumHood,#HelmetBronze,#HelmetCarapace,#HelmetDrake,#HelmetDverger,#HelmetFenring,#HelmetFishingHat,#HelmetFlametal,#HelmetHat1,#HelmetHat10,#HelmetHat2,#HelmetHat3,#HelmetHat4,#HelmetHat5,#HelmetHat6,#HelmetHat7,#HelmetHat8,#HelmetHat9,#HelmetIron,#HelmetLeather,#HelmetMage,#HelmetMage_Ashlands,#HelmetMidsummerCrown,#HelmetOdin,#HelmetPadded,#HelmetPointyHat,#HelmetRoot,#HelmetStrawHat,#HelmetTrollLeather,#HelmetYule,#HildirKey_forestcrypt,#HildirKey_mountaincave,#HildirKey_plainsfortress,#Hoe,#Honey,#HoneyGlazedChicken,#HoneyGlazedChickenUncooked,#Iron,#IronNails,#IronOre,#Ironpit,#IronScrap,#JuteBlue,#JuteRed,#KnifeBlackMetal,#KnifeButcher,#KnifeChitin,#KnifeCopper,#KnifeFlint,#KnifeSilver,#KnifeSkollAndHati,#Lantern,#Larva,#LeatherScraps,#LinenThread,#LoxMeat,#LoxPelt,#LoxPie,#LoxPieUncooked,#MaceBronze,#MaceEldner,#MaceEldnerBlood,#MaceEldnerLightning,#MaceEldnerNature,#MaceIron,#MaceNeedle,#MaceSilver,#MagicallyStuffedShroom,#MagicallyStuffedShroomUncooked,#Mandible,#MarinatedGreens,#MashedMeat,#MeadBaseBugRepellent,#MeadBaseBzerker,#MeadBaseEitrLingering,#MeadBaseEitrMinor,#MeadBaseFrostResist,#MeadBaseHasty,#MeadBaseHealthLingering,#MeadBaseHealthMajor,#MeadBaseHealthMedium,#MeadBaseHealthMinor,#MeadBaseLightFoot,#MeadBasePoisonResist,#MeadBaseStaminaLingering,#MeadBaseStaminaMedium,#MeadBaseStaminaMinor,#MeadBaseStrength,#MeadBaseSwimmer,#MeadBaseTamer,#MeadBaseTasty,#MeadBugRepellent,#MeadBzerker,#MeadEitrLingering,#MeadEitrMinor,#MeadFrostResist,#MeadHasty,#MeadHealthLingering,#MeadHealthMajor,#MeadHealthMedium,#MeadHealthMinor,#MeadLightfoot,#MeadPoisonResist,#MeadStaminaLingering,#MeadStaminaMedium,#MeadStaminaMinor,#MeadStrength,#MeadSwimmer,#MeadTamer,#MeadTasty,#MeadTrollPheromones,#MeatPlatter,#MeatPlatterUncooked,#MechanicalSpring,#MinceMeatSauce,#MisthareSupreme,#MisthareSupremeUncooked,#MoltenCore,#MorgenHeart,#MorgenSinew,#Mushroom,#MushroomBlue,#MushroomBzerker,#MushroomJotunPuffs,#MushroomMagecap,#MushroomOmelette,#MushroomSmokePuff,#MushroomYellow,#NeckTail,#NeckTailGrilled,#Needle,#Obsidian,#Onion,#OnionSeeds,#OnionSoup,#Ooze,#PickaxeAntler,#PickaxeBlackMetal,#PickaxeBronze,#PickaxeIron,#PickaxeStone,#PineCone,#PiquantPie,#PiquantPieUncooked,#Pot_Shard_Green,#Pot_Shard_Red,#PowderedDragonEgg,#ProustitePowder,#Pukeberries,#PungentPebbles,#QueenBee,#QueenDrop,#QueensJam,#Raspberry,#RawMeat,#Resin,#RoastedCrustPie,#RoastedCrustPieUncooked,#Root,#RottenMeat,#RoundLog,#RoyalJelly,#Ruby,#SaddleAsksvin,#SaddleLox,#Salad,#Sap,#Sausages,#ScaleHide,#ScorchingMedley,#Scythe,#ScytheHandle,#SeekerAspic,#SerpentMeat,#SerpentMeatCooked,#SerpentScale,#SerpentStew,#SharpeningStone,#ShieldBanded,#ShieldBlackmetal,#ShieldBlackmetalTower,#ShieldBoneTower,#ShieldBronzeBuckler,#ShieldCarapace,#ShieldCarapaceBuckler,#ShieldCore,#ShieldFlametal,#ShieldFlametalTower,#ShieldIronBuckler,#ShieldIronSquare,#ShieldIronTower,#ShieldKnight,#ShieldSerpentscale,#ShieldSilver,#ShieldWood,#ShieldWoodTower,#ShocklateSmoothie,#Silver,#SilverNecklace,#SilverOre,#SizzlingBerryBroth,#SledgeDemolisher,#SledgeIron,#SledgeStagbreaker,#Softtissue,#Sparkler,#SparklingShroomshake,#SpearBronze,#SpearCarapace,#SpearChitin,#SpearElderbark,#SpearFlint,#SpearSplitner,#SpearSplitner_Blood,#SpearSplitner_Lightning,#SpearSplitner_Nature,#SpearWolfFang,#SpiceAshlands,#SpiceForests,#SpiceMistlands,#SpiceMountains,#SpiceOceans,#SpicePlains,#SpicyMarmalade,#StaffClusterbomb,#StaffFireball,#StaffGreenRoots,#StaffIceShards,#StaffLightning,#StaffRedTroll,#StaffShield,#StaffSkeleton,#Stone,#SulfurStone,#SurtlingCore,#SwordBlackmetal,#SwordBronze,#SwordDyrnwyn,#SwordIron,#SwordIronFire,#SwordMistwalker,#SwordNiedhogg,#SwordNiedhoggBlood,#SwordNiedhoggLightning,#SwordNiedhoggNature,#SwordSilver,#Tankard,#Tankard_dvergr,#TankardAnniversary,#TankardOdin,#Tar,#Thistle,#THSwordKrom,#THSwordSlayer,#THSwordSlayerBlood,#THSwordSlayerLightning,#THSwordSlayerNature,#Thunderstone,#Tin,#TinOre,#Torch,#TorchMist,#TrollHide,#TrophyAbomination,#TrophyAsksvin,#TrophyBlob,#TrophyBoar,#TrophyBonemass,#TrophyBonemawSerpent,#TrophyCharredArcher,#TrophyCharredMage,#TrophyCharredMelee,#TrophyCultist,#TrophyCultist_Hildir,#TrophyDeathsquito,#TrophyDeer,#TrophyDragonQueen,#TrophyDraugr,#TrophyDraugrElite,#TrophyDraugrFem,#TrophyDvergr,#TrophyEikthyr,#TrophyFader,#TrophyFallenValkyrie,#TrophyFenring,#TrophyForestTroll,#TrophyFrostTroll,#TrophyGjall,#TrophyGoblin,#TrophyGoblinBrute,#TrophyGoblinBruteBrosBrute,#TrophyGoblinBruteBrosShaman,#TrophyGoblinKing,#TrophyGoblinShaman,#TrophyGreydwarf,#TrophyGreydwarfBrute,#TrophyGreydwarfShaman,#TrophyGrowth,#TrophyHare,#TrophyHatchling,#TrophyLeech,#TrophyLox,#TrophyMorgen,#TrophyNeck,#TrophySeeker,#TrophySeekerBrute,#TrophySeekerQueen,#TrophySerpent,#TrophySGolem,#TrophySkeleton,#TrophySkeletonHildir,#TrophySkeletonPoison,#TrophySurtling,#TrophyTheElder,#TrophyTick,#TrophyUlv,#TrophyVolture,#TrophyWolf,#TrophyWraith,#Turnip,#TurnipSeeds,#TurnipStew,#TurretBolt,#TurretBoltBone,#TurretBoltFlametal,#TurretBoltWood,#Vineberry,#VineberrySeeds,#VineGreenSeeds,#VoltureEgg,#VoltureMeat,#Wishbone,#Wisp,#WitheredBone,#WolfClaw,#WolfFang,#WolfHairBundle,#WolfJerky,#WolfMeat,#WolfMeatSkewer,#WolfPelt,#Wood,#YagluthDrop,#YggdrasilPorridge,#YggdrasilWood,#YmirRemains";
        }

        private void InitializeInputSystem()
        {
            try
            {
                // Try to get the Input type using reflection for better compatibility
                inputType = System.Type.GetType("UnityEngine.Input, UnityEngine.InputLegacyModule") ?? 
                           System.Type.GetType("UnityEngine.Input, UnityEngine") ??
                           typeof(Input);
                
                if (inputType != null)
                {
                    getKeyMethod = inputType.GetMethod("GetKey", new System.Type[] { typeof(KeyCode) });
                    getKeyDownMethod = inputType.GetMethod("GetKeyDown", new System.Type[] { typeof(KeyCode) });
                    
                    if (getKeyMethod != null && getKeyDownMethod != null)
                    {
                        inputSystemAvailable = true;
                        Logger.LogInfo("Input system initialized successfully using reflection");
                    }
                    else
                    {
                        Logger.LogWarning("Input methods not found, trying direct access");
                        // Try direct access as fallback
                        inputSystemAvailable = true;
                    }
                }
                else
                {
                    Logger.LogWarning("Input type not found, input functionality will be limited");
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Failed to initialize input system: {ex}");
                Logger.LogInfo("Input functionality will be disabled");
                inputSystemAvailable = false;
            }
        }

        private bool SafeGetKey(KeyCode keyCode)
        {
            try
            {
                if (!inputSystemAvailable) return false;
                
                if (getKeyMethod != null)
                {
                    return (bool)getKeyMethod.Invoke(null, new object[] { keyCode });
                }
                else
                {
                    // Direct fallback
                    return Input.GetKey(keyCode);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error checking key {keyCode}: {ex}");
                return false;
            }
        }

        private bool SafeGetKeyDown(KeyCode keyCode)
        {
            try
            {
                if (!inputSystemAvailable) return false;
                
                if (getKeyDownMethod != null)
                {
                    return (bool)getKeyDownMethod.Invoke(null, new object[] { keyCode });
                }
                else
                {
                    // Direct fallback
                    return Input.GetKeyDown(keyCode);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error checking key down {keyCode}: {ex}");
                return false;
            }
        }        void Update()
        {
            try
            {
                if (Player.m_localPlayer == null) return;
                if (!inputSystemAvailable) return;
                
                // Add item to ignore list: I
                if (SafeGetKeyDown(addItemKey.Value) && !SafeGetKey(KeyCode.LeftShift) && !SafeGetKey(KeyCode.RightShift))
                {
                    AddSelectedItemToIgnoreList();
                }
                
                // Remove item from ignore list: Shift + I
                if (SafeGetKeyDown(removeItemKey.Value) && (SafeGetKey(KeyCode.LeftShift) || SafeGetKey(KeyCode.RightShift)))
                {
                    RemoveSelectedItemFromIgnoreList();
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error in Update: {ex}");
            }        }

        void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }        [HarmonyPatch(typeof(ItemDrop), "Pickup")]
        class ItemDropPickupPatch
        {
            // Patch the actual pickup method to control behavior
            static bool Prefix(ItemDrop __instance, Humanoid character)
            {
                try
                {
                    if (character == null || !(character is Player))
                        return true; // Only affect players

                    // Check if this item should be ignored
                    string itemName = __instance.name;
                    string itemDataName = __instance.m_itemData?.m_shared?.m_name ?? "unknown";
                    logger?.LogInfo($"ItemDrop pickup check: GameObject name='{itemName}', ItemData name='{itemDataName}'");
                    logger?.LogInfo($"Items to ignore: {string.Join(", ", ItemsToIgnore)}");
                    
                    // Check multiple possible name formats
                    bool shouldIgnore = ItemsToIgnore.Contains(itemName) || 
                                      ItemsToIgnore.Contains(itemDataName) ||
                                      ItemsToIgnore.Contains($"{itemDataName}(Clone)") ||
                                      ItemsToIgnore.Contains(itemName.Replace("(Clone)", "")) ||
                                      ItemsToIgnore.Any(ignored => ignored.Replace("(Clone)", "") == itemDataName);
                    
                    if (shouldIgnore)
                    {
                        logger?.LogInfo($"Blocking pickup for ignored item: {itemName}");
                        // Only block if this is an auto-pickup (not manual)
                        // We can detect this by checking if the player is close but not actively trying to pickup
                        Player player = character as Player;
                        if (player != null && !player.InPlaceMode() && (Instance == null || !Instance.SafeGetKey(KeyCode.E)))
                        {
                            return false; // Block auto-pickup
                        }
                    }
                    
                    return true; // Allow pickup
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in ItemDrop.Pickup patch: {ex}");
                    return true; // Fallback to normal behavior
                }
            }
        }        // Patch the Player.AutoPickup method to disable ignored items
        [HarmonyPatch(typeof(Player), "AutoPickup")]
        class PlayerAutoPickupPatch
        {
            static bool Prefix(Player __instance, float dt)
            {
                try
                {
                    if (__instance == null || __instance.IsTeleporting())
                        return true;

                    // Disable auto pickup for ignored items
                    DisableIgnoredItemsAutoPickup(__instance);
                    return true; // Let normal pickup logic continue
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in Player.AutoPickup patch: {ex}");
                    return true; // Fallback to normal behavior
                }
            }            private static void DisableIgnoredItemsAutoPickup(Player player)
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

                        // Debug logging for auto pickup
                        string gameObjectName = itemDrop.name;
                        string itemDataName = itemDrop.m_itemData?.m_shared?.m_name ?? "unknown";
                        
                        // Check multiple possible name formats
                        bool shouldIgnore = ItemsToIgnore.Contains(gameObjectName) || 
                                          ItemsToIgnore.Contains(itemDataName) ||
                                          ItemsToIgnore.Contains($"{itemDataName}(Clone)") ||
                                          ItemsToIgnore.Contains(gameObjectName.Replace("(Clone)", "")) ||
                                          ItemsToIgnore.Any(ignored => ignored.Replace("(Clone)", "") == itemDataName);

                        if (shouldIgnore)
                        {
                            logger?.LogDebug($"Disabling auto pickup for ignored item: {gameObjectName} (ItemData: {itemDataName})");
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

        // Harmony patch to track selected items in inventory
        [HarmonyPatch(typeof(InventoryGui), "OnSelectedItem")]
        class InventorySelectionPatch
        {
            static void Postfix(InventoryGui __instance, InventoryGrid grid, ItemDrop.ItemData item)
            {
                try
                {
                    // Store the selected item
                    selectedItem = item;
                    
                    if (item != null && logger != null)
                    {
                        logger.LogDebug($"Selected item: {item.m_shared.m_name}");
                    }
                }
                catch (System.Exception ex)
                {
                    logger?.LogError($"Error in inventory selection patch: {ex}");
                }
            }
        }        // Note: Removed PlayerUseItemPatch as it was causing compatibility issues
        // The InventorySelectionPatch above already handles item selection tracking

        private void AddSelectedItemToIgnoreList()
        {
            try
            {
                if (selectedItem == null)
                {
                    if (Player.m_localPlayer != null)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, "No item selected. Click on an item in your inventory first.");
                    }
                    return;
                }

                string itemName = selectedItem.m_shared.m_name;
                string itemWithClone = $"{itemName}(Clone)";

                // Check if already in ignore list
                if (ItemsToIgnore.Contains(itemWithClone))
                {
                    if (Player.m_localPlayer != null)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, $"{itemName} is already in ignore list");
                    }
                    return;
                }

                // Add to current runtime list
                var currentItems = ItemsToIgnore.ToList();
                currentItems.Add(itemWithClone);
                ItemsToIgnore = currentItems;

                // Update config and save
                UpdateConfigFromIgnoreList();

                if (Player.m_localPlayer != null)
                {
                    Player.m_localPlayer.Message(MessageHud.MessageType.Center, $"✓ {itemName} added to ignore list");
                }

                Logger.LogInfo($"Added {itemName} to ignore list");
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error adding item to ignore list: {ex}");
            }
        }

        private void RemoveSelectedItemFromIgnoreList()
        {
            try
            {
                if (selectedItem == null)
                {
                    if (Player.m_localPlayer != null)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, "No item selected. Click on an item in your inventory first.");
                    }
                    return;
                }

                string itemName = selectedItem.m_shared.m_name;
                string itemWithClone = $"{itemName}(Clone)";

                // Check if in ignore list
                if (!ItemsToIgnore.Contains(itemWithClone))
                {
                    if (Player.m_localPlayer != null)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, $"{itemName} is not in ignore list");
                    }
                    return;
                }

                // Remove from current runtime list
                var currentItems = ItemsToIgnore.ToList();
                currentItems.Remove(itemWithClone);
                ItemsToIgnore = currentItems;

                // Update config and save
                UpdateConfigFromIgnoreList();

                if (Player.m_localPlayer != null)
                {
                    Player.m_localPlayer.Message(MessageHud.MessageType.Center, $"✗ {itemName} removed from ignore list");
                }

                Logger.LogInfo($"Removed {itemName} from ignore list");
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error removing item from ignore list: {ex}");
            }
        }        private void UpdateConfigFromIgnoreList()
        {
            try
            {
                // Convert back to config format (without (Clone) and with # prefix)
                var configItems = ItemsToIgnore
                    .Select(item => item.Replace("(Clone)", ""))
                    .Select(item => $"#{item}");

                // Get items that were manually added (not in default list)
                var defaultItems = GetDefaultItemList().Split(',')
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrEmpty(i));

                // Combine: default items stay commented, manually added items are uncommented
                var allConfigItems = new List<string>();
                
                // Add default items (keep them commented)
                foreach (var defaultItem in defaultItems)
                {
                    allConfigItems.Add(defaultItem.StartsWith("#") ? defaultItem : $"#{defaultItem}");
                }
                
                // Add manually added items (uncommented)
                foreach (var ignoredItem in ItemsToIgnore)
                {
                    string itemWithoutClone = ignoredItem.Replace("(Clone)", "");
                    if (!defaultItems.Any(d => d.Replace("#", "").Trim() == itemWithoutClone))
                    {
                        allConfigItems.Add(itemWithoutClone);
                    }
                    else
                    {
                        // Update default item to be uncommented
                        for (int i = 0; i < allConfigItems.Count; i++)
                        {
                            if (allConfigItems[i].Replace("#", "").Trim() == itemWithoutClone)
                            {
                                allConfigItems[i] = itemWithoutClone;
                                break;
                            }
                        }
                    }
                }

                items.Value = string.Join(",", allConfigItems);
                Config.Save();
                
                // Update the display of currently ignored items
                UpdateCurrentIgnoredItemsDisplay();
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error updating config: {ex}");
            }
        }

        private void UpdateCurrentIgnoredItemsDisplay()
        {
            try
            {
                if (ItemsToIgnore == null || !ItemsToIgnore.Any())
                {
                    currentIgnoredItems.Value = "None";
                }
                else
                {
                    // Show items without the (Clone) suffix for cleaner display
                    var displayItems = ItemsToIgnore
                        .Select(item => item.Replace("(Clone)", ""))
                        .OrderBy(item => item)
                        .ToList();
                    
                    currentIgnoredItems.Value = string.Join(", ", displayItems);
                }
                
                Logger.LogDebug($"Updated ignored items display: {currentIgnoredItems.Value}");
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error updating ignored items display: {ex}");
            }
        }
    }
}
