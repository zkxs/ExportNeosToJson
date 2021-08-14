using MelonLoader;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using System.Reflection;
using System.IO;
using BaseX;

namespace ExportNeosToJson
{
    public class ExportNeosToJson : MelonMod
    {
        private static readonly string EXTENSION_7ZBSON = "7ZBSON";
        private static readonly string EXTENSION_JSON = "JSON";

        public override void OnApplicationStart()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("net.michaelripley.ExportNeosToJson");
            FieldInfo formatsField = AccessTools.DeclaredField(typeof(ModelExportable), "formats");
            if (formatsField == null)
            {
                MelonLogger.Error("could not read ModelExportable.formats");
                return;
            }
            
            // inject addional formats
            List<string> modelFormats = new List<string>((string[])formatsField.GetValue(null));
            modelFormats.Add("7ZBSON");
            modelFormats.Add("JSON");
            formatsField.SetValue(null, modelFormats.ToArray());

            MethodInfo exportModelOriginal = AccessTools.DeclaredMethod(typeof(ModelExporter), nameof(ModelExporter.ExportModel), new Type[] { typeof(Slot), typeof(string) });
            if (exportModelOriginal == null)
            {
                MelonLogger.Error("Could not find ModelExporter.ExportModel(Slot, string)");
                return;
            }
            MethodInfo exportModelPrefix = AccessTools.DeclaredMethod(typeof(ExportNeosToJson), nameof(ExportModelPrefix));
            harmony.Patch(exportModelOriginal, prefix: new HarmonyMethod(exportModelPrefix));

            MelonLogger.Msg("Hook installed successfully");
        }

        private static bool ExportModelPrefix(Slot slot, string targetFile, ref Task<bool> __result)
        {
            string extension = Path.GetExtension(targetFile).Substring(1).ToUpper();
            if (EXTENSION_7ZBSON.Equals(extension))
            {
                __result = Export7zbson(slot, targetFile);
                return false; // skip original function
            }
            else if (EXTENSION_JSON.Equals(extension))
            {
                __result = ExportJson(slot, targetFile);
                return false; // skip original function
            }
            else
            {
                return true; // call original function
            }
        }

        private static async Task<bool> Export7zbson(Slot slot, string targetFile)
        {
            await new ToBackground();
            SavedGraph graph = slot.SaveObject(DependencyHandling.CollectAssets);
            using (FileStream fileStream = File.OpenWrite(targetFile))
            {
                DataTreeConverter.To7zBSON(graph.Root, fileStream);
            }
            MelonLogger.Msg(string.Format("exported {0} to {1}", slot.Name, targetFile));
            return true;
        }

        private static async Task<bool> ExportJson(Slot slot, string targetFile)
        {
            await new ToBackground();
            SavedGraph graph = slot.SaveObject(DependencyHandling.CollectAssets);
            File.WriteAllText(targetFile, DataTreeConverter.ToJSON(graph.Root));
            MelonLogger.Msg(string.Format("exported {0} to {1}", slot.Name, targetFile));
            return true;
        }
    }
}
