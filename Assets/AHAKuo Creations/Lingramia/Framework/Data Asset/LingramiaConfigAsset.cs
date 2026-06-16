using System;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using UnityEngine;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    [Icon("Assets/AHAKuo Creations/Lingramia/Framework/Graphics/manager_icon.png")]
    public class LingramiaConfigAsset : ScriptableObject
    {
        [Header("Framework Settings")]
        public bool KeepManagerAlive = false;

        // Debugging
        public bool EnableDebugging = false;

        [Header("Localization System")]
        public LocalizationSystemSettings LocalizationSystem = new LocalizationSystemSettings();
    }

    [Serializable]
    public class LocalizationSystemSettings
    {
        [Header("Hybrid Key Mode")]
        [Tooltip("When enabled, the localization system will search for strings by key, value, and aliases. Useful for projects with hardcoded strings that need localization.")]
        public bool HybridKey = false;
        
        [Header("LocBook Configuration")]
        [Tooltip("Array of LocBook assets containing localization data. Each LocBook will be loaded into the system.")]
        public LocalizationStandalone.Internal.LocBook[] LocBooks = Array.Empty<LocBook>();
        
        [Header("Text Style Cache")]
        [Tooltip("Cache of TextStyle assets for different languages. These define font and formatting settings per language.")]
        public LocalizationStandalone.Internal.TextStyle[] TextStyleCache = Array.Empty<TextStyle>();
        
        [Header("Default Settings")]
        [Tooltip("The default starting language code (e.g., 'en', 'es', 'fr'). This is used when no saved preference exists.")]
        public string DefaultStartingLanguageCode = "en";
        
        [Header("Save Settings")]
        [Tooltip("The key used to save/load the user's language preference using Unity's PlayerPrefs.")]
        public string LanguageOptionSaveKey = "language";
        
        [Header("Internal")]
        [Tooltip("When enabled, automatically updates LocBook assets when their referenced .locbook files are imported or modified.")]
        public bool AutoUpdateLocbooks = false;
        
        [Tooltip("When enabled, automatically refreshes the localization cache in runtime when LocBook assets are updated. WARNING: This will impact editor performance while playing.")]
        public bool AutoRefreshCacheInRuntime = false;
    }
}