using BaseX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ExportNeosToJson
{
    public class ExportNeosToJson : NeosMod
    {
        public override string Name => "ExportNeosToJson";
        public override string Author => "runtime";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/zkxs/ExportNeosToJson";

        private static readonly string EXTENSION_7ZBSON = "7ZBSON";
        private static readonly string EXTENSION_JSON = "JSON";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.michaelripley.ExportNeosToJson");
            FieldInfo formatsField = AccessTools.DeclaredField(typeof(ModelExportable), "formats");
            if (formatsField == null)
            {
                Error("could not read ModelExportable.formats");
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
                Error("Could not find ModelExporter.ExportModel(Slot, string)");
                return;
            }
            MethodInfo exportModelPrefix = AccessTools.DeclaredMethod(typeof(ExportNeosToJson), nameof(ExportModelPrefix));
            harmony.Patch(exportModelOriginal, prefix: new HarmonyMethod(exportModelPrefix));

            Msg("Hook installed successfully");
        }

        private static bool ExportModelPrefix(Slot slot, string targetFile, ref Task<bool> __result)
        {
            string extension = Path.GetExtension(targetFile).Substring(1).ToUpper();
            if (EXTENSION_7ZBSON.Equals(extension))
            {
                SavedGraph graph = slot.SaveObject(DependencyHandling.CollectAssets);
                __result = Export7zbson(graph, targetFile);
                return false; // skip original function
            }
            else if (EXTENSION_JSON.Equals(extension))
            {
                SavedGraph graph = slot.SaveObject(DependencyHandling.CollectAssets);
                __result = ExportJson(graph, targetFile);
                return false; // skip original function
            }
            else
            {
                return true; // call original function
            }
        }

        private static async Task<bool> Export7zbson(SavedGraph graph, string targetFile)
        {
            await new ToBackground();
            using (FileStream fileStream = File.OpenWrite(targetFile))
            {
                DataTreeConverter.To7zBSON(graph.Root, fileStream);
            }
            Msg(string.Format("exported {0}", targetFile));
            return true;
        }

        private static async Task<bool> ExportJson(SavedGraph graph, string targetFile)
        {
            await new ToBackground();
            File.WriteAllText(targetFile, DataTreeConverter.ToJSON(graph.Root));
            Msg(string.Format("exported {0}", targetFile));
            return true;
        }
    }
}
