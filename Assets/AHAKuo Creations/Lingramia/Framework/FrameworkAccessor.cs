using System;
using System.Collections.Generic;
using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    /// <summary>
    /// SIGS (Signalia Global Shorthand) is a static gateway to Signalia localization tools and systems.
    /// This class simplifies interaction with the localization system, providing easy access to
    /// localization features through a centralized interface.
    /// 
    /// Example Usage:
    ///     string text = SIGS.GetLocalizedString("welcome_message");
    ///     SIGS.ChangeLanguage("es");
    ///     SIGS.TriggerLanguageChange();
    /// </summary>
    public static class SIGS
    {
        //// FRAMEWORK METHODS ////

        /// <summary>
        /// Resets the Signalia runtime values, clearing all tracked values and resetting the state of the framework.
        /// This is useful for resetting the framework to a clean state, such as when reloading scenes or restarting the game.
        /// WARNING: This will destroy the active watchman and all static values, so use with caution.
        /// </summary>
        public static void ResetSignaliaRuntime() => Watchman.ResetEverything(true);

        //// GAME SYSTEM METHODS: Localization System ////

        /// <summary>
        /// Gets a localized string by its key for the current language.
        /// This is the primary method to retrieve localized text in your code.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <returns>The localized string for the current language</returns>
        public static string GetLocalizedString(string key) => LocalizationStandalone.Internal.Localization.ReadKey(key);

        /// <summary>
        /// Gets a localized string by its key for a specific language.
        /// This method applies formatting (e.g., Arabic shaping) automatically.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <param name="languageCode">The language code to retrieve (e.g., "en", "es", "fr")</param>
        /// <param name="paragraphStyle">Optional paragraph style to filter by (e.g., "Header", "Description")</param>
        /// <returns>The localized string for the specified language</returns>
        public static string GetLocalizedString(string key, string languageCode, string paragraphStyle = "") => LocalizationStandalone.Internal.Localization.ReadKey(key, languageCode, paragraphStyle);

        /// <summary>
        /// Gets a raw localized string by its key for the current language without applying formatting.
        /// Use this when you need to apply string formatting (e.g., string.Format) before language-specific formatting.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <returns>The raw localized string without formatting, or the key itself if not found</returns>
        public static string GetRawLocalizedString(string key) => LocalizationStandalone.Internal.Localization.ReadKeyRaw(key);

        /// <summary>
        /// Gets a raw localized string by its key for a specific language without applying formatting.
        /// Use this when you need to apply string formatting (e.g., string.Format) before language-specific formatting.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <param name="languageCode">The language code to retrieve (e.g., "en", "es", "fr")</param>
        /// <returns>The raw localized string without formatting, or fallback value if not found</returns>
        public static string GetRawLocalizedString(string key, string languageCode) => LocalizationStandalone.Internal.Localization.ReadKeyRaw(key, languageCode);

        /// <summary>
        /// Gets the TextStyle asset for a specific language code.
        /// TextStyles define font and formatting settings for different languages.
        /// </summary>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <param name="paragraphStyle">Optional paragraph style to filter by (e.g., "Header", "Description"). Leave empty or null for default style.</param>
        /// <returns>The TextStyle asset for the language, or null if not found</returns>
        public static LocalizationStandalone.Internal.TextStyle GetTextStyle(string languageCode, string paragraphStyle = "") => LocalizationStandalone.Internal.LocalizationRuntime.GetTextStyle(languageCode, paragraphStyle);

        /// <summary>
        /// Changes the current language and optionally saves the preference.
        /// This will trigger a language changed event that UI elements can respond to.
        /// </summary>
        /// <param name="code">The new language code (e.g., "en", "es", "fr")</param>
        /// <param name="save">Whether to save this language preference for future sessions</param>
        public static void ChangeLanguage(string code, bool save = true) => LocalizationStandalone.Internal.LocalizationRuntime.ChangeLanguage(code, save);

        /// <summary>
        /// Gets the current active language code.
        /// </summary>
        /// <returns>The current language code</returns>
        public static string GetCurrentLanguage() => LocalizationStandalone.Internal.LocalizationRuntime.CurrentLanguageCode;

        /// <summary>
        /// Triggers the language changed event manually.
        /// Use this to manually trigger UI updates after batch language operations.
        /// </summary>
        public static void TriggerLanguageChange() => LocalizationStandalone.Internal.LocalizationRuntime.FireLanguageChangedEvent();

        /// <summary>
        /// Initializes the localization system from a LocBook asset.
        /// This should be called once at game startup before using any localization features.
        /// </summary>
        /// <param name="locBook">The LocBook asset to load localization data from</param>
        //public static void InitializeLocalization(AHAKuo.Signalia.LocalizationStandalone.Internal.LocBook locBook) => AHAKuo.Signalia.LocalizationStandalone.Internal.Localization.Initialize(locBook);

        /// <summary>
        /// Initializes the localization system from the LocBook configured in Signalia settings.
        /// This is the recommended initialization method as it uses the configured LocBook.
        /// </summary>
        public static void InitializeLocalization()
        {
            LingramiaConfigAsset config = ConfigReader.GetConfig();
            if (config != null && config.LocalizationSystem.LocBooks != null && config.LocalizationSystem.LocBooks.Length > 0)
            {
                LocalizationStandalone.Internal.Localization.Initialize(config.LocalizationSystem.LocBooks);
            }
            else
            {
                Debug.LogWarning("[Signalia Localization] No LocBooks configured in Signalia settings.");
            }
        }

        /// <summary>
        /// Checks if a localization key exists in the loaded data.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the key exists</returns>
        public static bool HasLocalizationKey(string key) => LocalizationStandalone.Internal.Localization.HasKey(key);

        /// <summary>
        /// Gets all available language codes from the loaded localization data.
        /// </summary>
        /// <returns>List of language codes</returns>
        public static System.Collections.Generic.List<string> GetAvailableLanguages() => LocalizationStandalone.Internal.Localization.GetAvailableLanguages();
        
        /// <summary>
        /// Gets a localized audio clip by its key for the current language.
        /// Audio localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The audio key</param>
        /// <returns>The localized audio clip, or null if not found</returns>
        public static AudioClip GetLocalizedAudioClip(string key) => LocalizationStandalone.Internal.Localization.ReadAudioClip(key);
        
        /// <summary>
        /// Gets a localized audio clip by its key for a specific language.
        /// Audio localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The audio key</param>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <returns>The localized audio clip, or null if not found</returns>
        public static AudioClip GetLocalizedAudioClip(string key, string languageCode) => LocalizationStandalone.Internal.Localization.ReadAudioClip(key, languageCode);
        
        /// <summary>
        /// Gets a localized sprite by its key for the current language.
        /// Sprite localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The sprite key</param>
        /// <returns>The localized sprite, or null if not found</returns>
        public static Sprite GetLocalizedSprite(string key) => LocalizationStandalone.Internal.Localization.ReadSprite(key);
        
        /// <summary>
        /// Gets a localized sprite by its key for a specific language.
        /// Sprite localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The sprite key</param>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <returns>The localized sprite, or null if not found</returns>
        public static Sprite GetLocalizedSprite(string key, string languageCode) => LocalizationStandalone.Internal.Localization.ReadSprite(key, languageCode);
        
        /// <summary>
        /// Gets a localized asset by its key for the current language.
        /// Asset localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <typeparam name="T">The type of asset to retrieve (must inherit from UnityEngine.Object)</typeparam>
        /// <param name="key">The asset key</param>
        /// <returns>The localized asset, or null if not found</returns>
        public static T GetLocalizedAsset<T>(string key) where T : UnityEngine.Object => LocalizationStandalone.Internal.Localization.ReadAsset<T>(key);
        
        /// <summary>
        /// Gets a localized asset by its key for a specific language.
        /// Asset localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <typeparam name="T">The type of asset to retrieve (must inherit from UnityEngine.Object)</typeparam>
        /// <param name="key">The asset key</param>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <returns>The localized asset, or null if not found</returns>
        public static T GetLocalizedAsset<T>(string key, string languageCode) where T : UnityEngine.Object => LocalizationStandalone.Internal.Localization.ReadAsset<T>(key, languageCode);
        
        /// <summary>
        /// Checks if a localized audio clip exists for a given key.
        /// </summary>
        /// <param name="key">The audio key to check</param>
        /// <returns>True if the audio key exists</returns>
        public static bool HasLocalizedAudioClip(string key) => LocalizationStandalone.Internal.Localization.HasAudioKey(key);
        
        /// <summary>
        /// Checks if a localized sprite exists for a given key.
        /// </summary>
        /// <param name="key">The sprite key to check</param>
        /// <returns>True if the sprite key exists</returns>
        public static bool HasLocalizedSprite(string key) => LocalizationStandalone.Internal.Localization.HasSpriteKey(key);
        
        /// <summary>
        /// Checks if a localized asset exists for a given key.
        /// </summary>
        /// <param name="key">The asset key to check</param>
        /// <returns>True if the asset key exists</returns>
        public static bool HasLocalizedAsset(string key) => LocalizationStandalone.Internal.Localization.HasAssetKey(key);
    }
}