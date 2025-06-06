using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.Reflection;

namespace ChoosyViking
{
    [BepInPlugin(PluginGUID, PluginName, ModVersion)]
    [BepInProcess("valheim.exe")]
    public class ChoosyViking : BaseUnityPlugin
    {
<<<<<<< Updated upstream:ItemAutoPickupIgnorer.cs
        public const string PluginGUID = "stal4gmite.ItemAutoPickupIgnorer";
        public const string PluginName = "Item Auto Pickup Ignorer";
        public const string ModVersion = "1.1.1";

        public enum ItemAutoPickupIgnorerMode { VALHEIM_DEFAULT, IGNORE_SOME, IGNORE_NONE }        private readonly Harmony harmony = new Harmony(PluginGUID);
=======
        public const string PluginGUID = "shivam13juna.ChoosyViking";
        public const string PluginName = "Choosy Viking";
        public const string ModVersion = "1.1.0";

        public enum ChoosyVikingMode { VALHEIM_DEFAULT, IGNORE_SOME, IGNORE_NONE }

        private readonly Harmony harmony = new Harmony(PluginGUID);
>>>>>>> Stashed changes:ChoosyViking.cs
        private ConfigEntry<string> items;
        private ConfigEntry<KeyCode> toggleKey;
        private ConfigEntry<KeyCode> modifierKey;
        
<<<<<<< Updated upstream:ItemAutoPickupIgnorer.cs
        // Input system fallback
        private static System.Type inputType;
        private static MethodInfo getKeyMethod;
        private static MethodInfo getKeyDownMethod;
        private static bool inputSystemAvailable = false;
          public static IEnumerable<string> ItemsToIgnore = new List<string> { "Stone(Clone)" };
        public static ItemAutoPickupIgnorerMode Mode = ItemAutoPickupIgnorerMode.IGNORE_SOME;
=======
        public static IEnumerable<string> ItemsToIgnore = new List<string> { "Stone(Clone)" };
        public static ChoosyVikingMode Mode = ChoosyVikingMode.VALHEIM_DEFAULT;
>>>>>>> Stashed changes:ChoosyViking.cs
        public static ManualLogSource logger;
        public static ItemAutoPickupIgnorer Instance;        void Awake()
        {
            Instance = this;
            logger = Logger;
            
            try
            {
                // Initialize input system
                InitializeInputSystem();
                  harmony.PatchAll();
                Logger.LogInfo($"{PluginName} {ModVersion} loaded successfully!");
                Logger.LogInfo($"Starting in mode: {GetModeDisplayText(Mode)}");                // Configuration
                items = Config.Bind("Settings",
                                    "Items",
                                    GetDefaultItemList(),
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
                Logger.LogError($"Error during Awake: {ex}");            }
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
        }

        void Update()
        {
            try
            {
                if (Player.m_localPlayer == null) return;
                if (!inputSystemAvailable) return;
                
                if (SafeGetKey(modifierKey.Value))
                {
                    if (SafeGetKeyDown(toggleKey.Value))
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
                case ChoosyVikingMode.VALHEIM_DEFAULT:
                    Mode = ChoosyVikingMode.IGNORE_SOME;
                    break;
                case ChoosyVikingMode.IGNORE_SOME:
                    Mode = ChoosyVikingMode.IGNORE_NONE;
                    break;
                case ChoosyVikingMode.IGNORE_NONE:
                    Mode = ChoosyVikingMode.VALHEIM_DEFAULT;
                    break;
            }

            string modeText = GetModeDisplayText(Mode);
            Logger.LogInfo($"Switched to mode: {modeText}");
            
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"Item Auto Pickup Ignorer: {modeText}");
            }
        }

        string GetModeDisplayText(ChoosyVikingMode mode)
        {
            return mode switch
            {
                ChoosyVikingMode.IGNORE_SOME => "Ignoring Items",
                ChoosyVikingMode.IGNORE_NONE => "Ignoring Nothing", 
                ChoosyVikingMode.VALHEIM_DEFAULT => "Normal Valheim Behavior",
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
                    if (Mode == ChoosyVikingMode.VALHEIM_DEFAULT)
                        return true; // Let normal behavior happen

                    if (character == null || !(character is Player))
                        return true; // Only affect players

                    // If we're ignoring some items and this is one of them, don't auto-pickup
                    if (Mode == ChoosyVikingMode.IGNORE_SOME)
                    {
                        if (ItemsToIgnore.Contains(__instance.name))
                        {                            // Only block if this is an auto-pickup (not manual)
                            // We can detect this by checking if the player is close but not actively trying to pickup
                            Player player = character as Player;
                            if (player != null && !player.InPlaceMode() && (Instance == null || !Instance.SafeGetKey(KeyCode.E)))
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
                    if (Mode == ChoosyVikingMode.VALHEIM_DEFAULT)
                        return true; // Use default Valheim behavior

                    if (__instance == null || __instance.IsTeleporting())
                        return true;

                    // For IGNORE_NONE mode, force enable all auto pickups in range
                    if (Mode == ChoosyVikingMode.IGNORE_NONE)
                    {
                        ForceEnableAutoPickup(__instance);
                        return true; // Let normal pickup logic continue
                    }

                    // For IGNORE_SOME mode, disable auto pickup for ignored items
                    if (Mode == ChoosyVikingMode.IGNORE_SOME)
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
