using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HarmonyLib;
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
            string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string jsonPath = Path.Combine(assemblyDir, "Localized_Text.json");
            var LocalizedTextTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(jsonPath));
            if (LocalizedTextTable != null)
            {
                foreach (var item in LocalizedTextTable)
                {
                    var values = item.Value;
                    string firstValue = values[0];
                    values = values.Select(x => string.IsNullOrEmpty(x) ? firstValue : x).ToList();
                    LocalizedText.MAIN_TABLE.Add($"Mod_Impeakster_{item.Key}".ToUpper(), values);
                }
            }
        }
    }
}
