using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;

namespace ItemAutoPickupIgnorer
{
    [BepInPlugin("stal4gmite.ItemAutoPickupIgnorer", "Item Auto Pickup Ignorer", "1.0.1")]
    [BepInProcess("valheim.exe")]
    public class ItemAutoPickupIgnorer : BaseUnityPlugin
    {
		public enum ItemAutoPickupIgnorerMode { VALHEIM_DEFAULT, IGNORE_SOME, IGNORE_NONE }

        private readonly Harmony harmony = new Harmony("stal4gmite.ItemAutoPickupIgnorer");
		private ConfigEntry<string> items;
		
		public static IEnumerable<string> ItemsToIgnore = new List<string> { "Stone(Clone)" };

		public static ItemAutoPickupIgnorerMode Mode = ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT;

		void Awake()
        {
            harmony.PatchAll();
			items = Config.Bind("Settings",
								"Items",
								string.Empty,
								"List of items. Remove '#' to ignore the item.");

			ItemsToIgnore = items.Value.Split(',').Select(i => i.Trim()).Where(i => !i.StartsWith("#")).Select(i => $"{i}(Clone)");
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetKeyDown(KeyCode.L))
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

					Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"Item Auto Pickup Ignorer: {GetModeDisplayText(Mode)}");
				}
			}
		}

		string GetModeDisplayText(ItemAutoPickupIgnorerMode mode)
		{
			string text = string.Empty;

			switch (mode)
			{
				case ItemAutoPickupIgnorerMode.IGNORE_SOME:
					text = "Ignoring Items";
					break;
				case ItemAutoPickupIgnorerMode.IGNORE_NONE:
					text = "Ignoring Nothing";
					break;
				case ItemAutoPickupIgnorerMode.VALHEIM_DEFAULT:
					text = "Normal Valheim Behavior";
					break;
				default:
					text = "Unknown Behavior";
					break;
			}

			return text;
		}

		[HarmonyPatch(typeof(Player), "AutoPickup")]
        class ItemAutoPickupIgnorerPlayerPatch
        {
			static void Prefix(ref float ___m_autoPickupRange, ref int ___m_autoPickupMask, Player __instance)
            {
				if (__instance.IsTeleporting())
				{
					return;
				}

				Vector3 vector = __instance.transform.position + Vector3.up;

				Collider[] array = Physics.OverlapSphere(vector, ___m_autoPickupRange, ___m_autoPickupMask);

				foreach (Collider collider in array)
				{
					if (!collider.attachedRigidbody)
					{
						continue;
					}

					ItemDrop component = collider.attachedRigidbody.GetComponent<ItemDrop>();

					if (component == null || __instance.HaveUniqueKey(component.m_itemData.m_shared.m_name) || !component.GetComponent<ZNetView>().IsValid())
					{
						continue;
					}

					if (Mode == ItemAutoPickupIgnorerMode.IGNORE_SOME && ItemsToIgnore.Contains(component.name))
					{
						component.m_autoPickup = false;
					}
					else if (Mode == ItemAutoPickupIgnorerMode.IGNORE_NONE)
					{
						component.m_autoPickup = true;
					}
				}
			}
        }
    }
}
