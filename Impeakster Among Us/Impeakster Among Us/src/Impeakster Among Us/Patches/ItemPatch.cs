using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace Impeakster_Among_Us.Patches
{
    [HarmonyPatch(typeof(Item))]
    public class ItemPatch
    {

        [HarmonyPatch(nameof(Item.Consume))]
        [HarmonyPostfix]
        private static void ImposterFood(Item __instance)
        {
            if (ImpostorData.isImpostor)
            {
                __instance.holderCharacter.AddExtraStamina(0.05f);
            }
        }
    }
}
