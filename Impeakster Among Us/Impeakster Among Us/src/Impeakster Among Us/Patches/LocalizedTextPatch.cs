using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HarmonyLib;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Impeakster_Among_Us.Patches
{
    [HarmonyPatch(typeof(LocalizedText))]
    [HarmonyWrapSafe]
    public static class LocalizedTextPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("LoadMainTable")]
        public static void LoadMainTable()
        {
            LoadLocalizedText();
        }

        public static void LoadLocalizedText()
        {
            var LocalizedTextTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(Properties.Resources.Localized_Text);
            if (LocalizedTextTable != null)
            {
                Debug.Log($"Loaded {LocalizedTextTable.Count} localized text entries from embedded resource.");
                foreach (var item in LocalizedTextTable)
                {
                    var values = item.Value;
                    string firstValue = values[0];
                    values = values.Select(x => string.IsNullOrEmpty(x) ? firstValue : x).ToList();
                    var key = $"Mod_Impeakster_{item.Key}".ToUpper();
                    if (LocalizedText.MAIN_TABLE.ContainsKey(key))
                    {
                        LocalizedText.MAIN_TABLE[key] = values;
                    }
                    else
                    {
                        LocalizedText.MAIN_TABLE.Add(key, values);
                    }
                }
            }
        }
    }
}
