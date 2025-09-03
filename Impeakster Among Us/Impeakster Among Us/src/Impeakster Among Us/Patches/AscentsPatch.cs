using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace Impeakster_Among_Us.Patches
{
    [HarmonyPatch(typeof(Ascents))]
    public class AscentsPatch
    {

        [HarmonyPatch(nameof(Ascents.canReviveDead), MethodType.Getter)]
        [HarmonyPrefix]
        private static void CantRevive(ref bool __result)
        {
            // Works, but re-balancing
            //__result = false;
            //return false;
        }
    }
}
