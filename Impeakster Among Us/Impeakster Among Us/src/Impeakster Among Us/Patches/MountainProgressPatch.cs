using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using System.Xml.Linq;
using System.IO;

namespace Impeakster_Among_Us.Patches
{
    [HarmonyPatch(typeof(MountainProgressHandler))]
    internal class MountainProgressPatch
    {
        public static System.Random seededRandom = new System.Random();
        public static bool seeded = false;

        [HarmonyPatch(nameof(MountainProgressHandler.TriggerReached))]
        [HarmonyPrefix]
        private static void PatchTitleText(MountainProgressHandler __instance)
        {

            if (!seeded)
            {
                int currentTimeSeed = DateTime.UtcNow.Year + DateTime.UtcNow.Day + DateTime.UtcNow.Hour + DateTime.UtcNow.Minute / 15;
                seededRandom = new System.Random(currentTimeSeed);
                seeded = true;
            }

            List<string> names = new List<string>();
            foreach (Character allCharacter in Character.AllCharacters)
            {
                if (!allCharacter.data.dead)
                {
                    names.Add(allCharacter.characterName);
                }
            }
            // Sort names alphabetically:
            names.Sort();
            int numImposters = Math.Max(1, names.Count / 4);
            List<string> chosenImpostors = names.OrderBy(x => seededRandom.Next()).Take(numImposters).ToList();
            int randInt = seededRandom.Next();
            if (chosenImpostors.Contains(Player.localPlayer.character.characterName))
            {
                ImpostorData.isImpostor = true;
                if (chosenImpostors.Count > 1)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        __instance.progressPoints[i].title = "Impostors: " + string.Join(", ", chosenImpostors);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        //__instance.progressPoints[i].title = GetText("Impostor");
                        __instance.progressPoints[i].title = "Mod_Impeakster_Impostor";
                    }
                    //__instance.progressPoints[i].title = "Impostor";
                }
            }
            else
            {
                ImpostorData.isImpostor = false;
                for (int i = 0; i < 6; i++)
                {
                    __instance.progressPoints[i].title = "Scout";
                }
            }
        }

        public static string GetText(string key, params object[] args)
        {
            string fullKey = $"Mod_Impeakster_{key}".ToUpperInvariant();

            try
            {
                string template = LocalizedText.GetText(fullKey);
                if (string.IsNullOrEmpty(template))
                {
                    return $"[{fullKey}]";
                }

                return args != null && args.Length > 0
                    ? string.Format(template, args)
                    : template;
            }
            catch (Exception ex)
            {
                return $"[{fullKey}]";
            }
        }
    }
}
