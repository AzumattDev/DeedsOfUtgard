using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using ServerSync;
using UnityEngine;

namespace DeedsOfUtgard
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class DeedsOfUtgardPlugin : BaseUnityPlugin
    {
        internal const string ModName = "DeedsOfUtgard";
        internal const string ModVersion = "1.1.0";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource DeedsOfUtgardLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public void Awake()
        {
            ConfigSync.IsLocked = true;

            /*
             * Piece name naming convention
             *
             * $item_deed_utgard_10m
             *
             * $item_deed_10m
             *
             * */
            
            /* Normal Deeds */
            Item deed10M = new("deedsofutgard", "Deed10m");
            Item deed20M = new("deedsofutgard", "Deed20m");
            Item deed50M = new("deedsofutgard", "Deed50m");
            deed10M.Name.English("Deed 10m");
            deed20M.Name.English("Deed 20m");
            deed50M.Name.English("Deed 50m");
            deed10M.Description.English("Deed 10m");
            deed20M.Description.English("Deed 20m");
            deed50M.Description.English("Deed 50m");

            /* Utgard Deeds */
            Item deedutgard10M = new("deedsofutgard", "DeedUtgard10m");
            Item deedutgard20M = new("deedsofutgard", "DeedUtgard20m");
            Item deedutgard50M = new("deedsofutgard", "DeedUtgard50m");
            deedutgard10M.Name.English("Deed Utgard 10m");
            deedutgard20M.Name.English("Deed Utgard 20m");
            deedutgard50M.Name.English("Deed Utgard 50m");
            deedutgard10M.Description.English("Deed Utgard 10m");
            deedutgard20M.Description.English("Deed Utgard 20m");
            deedutgard50M.Description.English("Deed Utgard 50m");


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

        private static ConfigEntry<bool> _serverConfigLocked = null!;
        private static ConfigEntry<bool>? _recipeIsActiveConfig = null!;

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
}