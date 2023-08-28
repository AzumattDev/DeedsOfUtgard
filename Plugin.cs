using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using LocalizationManager;
using ServerSync;
using UnityEngine;

namespace DeedsOfUtgard
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class DeedsOfUtgardPlugin : BaseUnityPlugin
    {
        internal const string ModName = "DeedsOfUtgard";
        internal const string ModVersion = "1.1.7";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource DeedsOfUtgardLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        internal static readonly List<GameObject> DeedItems = new();

        public void Awake()
        {
            ConfigSync.IsLocked = true;

            Localizer.Load();
            /*
             * Piece name naming convention
             *
             * $item_deed_utgard_10m
             *
             * $item_deed_10m
             *
             * $item_deed_utgard_10m_description
             *
             * $item_deed_10m_description
             * */

            /* Normal Deeds */
            Item deed10M = new("deedsofutgard", "Deed10m");
            Item deed20M = new("deedsofutgard", "Deed20m");
            Item deed50M = new("deedsofutgard", "Deed50m");
            DeedItems.Add(deed10M.Prefab);
            DeedItems.Add(deed20M.Prefab);
            DeedItems.Add(deed50M.Prefab);

            /* Utgard Deeds */
            Item deedutgard10M = new("deedsofutgard", "DeedUtgard10m");
            Item deedutgard20M = new("deedsofutgard", "DeedUtgard20m");
            Item deedutgard50M = new("deedsofutgard", "DeedUtgard50m");
            DeedItems.Add(deedutgard10M.Prefab);
            DeedItems.Add(deedutgard20M.Prefab);
            DeedItems.Add(deedutgard50M.Prefab);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                DeedsOfUtgardLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                DeedsOfUtgardLogger.LogError($"There was an issue loading your {ConfigFileName}");
                DeedsOfUtgardLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        //private static ConfigEntry<bool> _serverConfigLocked = null!;
        //private static ConfigEntry<bool>? _recipeIsActiveConfig = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }

        #endregion
    }

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    static class ZNetSceneAwakePatch
    {
        static void Postfix(ZNetScene __instance)
        {
            FixSize(__instance);
        }


        internal static void FixSize(ZNetScene __instance)
        {
            foreach (GameObject deedItem in DeedsOfUtgardPlugin.DeedItems)
            {
                GameObject deed = __instance.GetPrefab(deedItem.name);
                Transform? attachGameObject = Utils.FindChild(deed.gameObject.transform, "attach");
                attachGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}