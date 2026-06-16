using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.Internal
{
    /// <summary>
    /// Extension methods for strings to provide easy access to SIGS localization methods.
    /// These extensions allow you to call localization methods directly on string keys.
    /// 
    /// Example Usage:
    ///     string text = "welcome_message".GetLocalized();
    ///     string spanishText = "welcome_message".GetLocalized("es");
    ///     AudioClip clip = "narration_intro".GetLocalizedAudioClip();
    ///     bool exists = "my_key".HasLocalizationKey();
    /// </summary>
    public static class SIGSLocalizationStringExtensions
    {
        //// STRING LOCALIZATION EXTENSIONS ////
        
        /// <summary>
        /// Gets a localized string by this key for the current language.
        /// This is the primary method to retrieve localized text in your code.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <returns>The localized string for the current language</returns>
        public static string GetLocalized(this string key) => SIGS.GetLocalizedString(key);
        
        /// <summary>
        /// Gets a localized string by this key for a specific language.
        /// This method applies formatting (e.g., Arabic shaping) automatically.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <param name="languageCode">The language code to retrieve (e.g., "en", "es", "fr")</param>
        /// <returns>The localized string for the specified language</returns>
        public static string GetLocalized(this string key, string languageCode) => SIGS.GetLocalizedString(key, languageCode);
        
        /// <summary>
        /// Gets a raw localized string by this key for the current language without applying formatting.
        /// Use this when you need to apply string formatting (e.g., string.Format) before language-specific formatting.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <returns>The raw localized string without formatting, or the key itself if not found</returns>
        public static string GetRawLocalized(this string key) => SIGS.GetRawLocalizedString(key);
        
        /// <summary>
        /// Gets a raw localized string by this key for a specific language without applying formatting.
        /// Use this when you need to apply string formatting (e.g., string.Format) before language-specific formatting.
        /// </summary>
        /// <param name="key">The localization key or source string (if hybrid key is enabled)</param>
        /// <param name="languageCode">The language code to retrieve (e.g., "en", "es", "fr")</param>
        /// <returns>The raw localized string without formatting, or fallback value if not found</returns>
        public static string GetRawLocalized(this string key, string languageCode) => SIGS.GetRawLocalizedString(key, languageCode);
        
        //// AUDIO LOCALIZATION EXTENSIONS ////
        
        /// <summary>
        /// Gets a localized audio clip by this key for the current language.
        /// Audio localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The audio key</param>
        /// <returns>The localized audio clip, or null if not found</returns>
        public static AudioClip GetLocalizedAudioClip(this string key) => SIGS.GetLocalizedAudioClip(key);
        
        /// <summary>
        /// Gets a localized audio clip by this key for a specific language.
        /// Audio localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The audio key</param>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <returns>The localized audio clip, or null if not found</returns>
        public static AudioClip GetLocalizedAudioClip(this string key, string languageCode) => SIGS.GetLocalizedAudioClip(key, languageCode);
        
        //// SPRITE LOCALIZATION EXTENSIONS ////
        
        /// <summary>
        /// Gets a localized sprite by this key for the current language.
        /// Sprite localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The sprite key</param>
        /// <returns>The localized sprite, or null if not found</returns>
        public static Sprite GetLocalizedSprite(this string key) => SIGS.GetLocalizedSprite(key);
        
        /// <summary>
        /// Gets a localized sprite by this key for a specific language.
        /// Sprite localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <param name="key">The sprite key</param>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <returns>The localized sprite, or null if not found</returns>
        public static Sprite GetLocalizedSprite(this string key, string languageCode) => SIGS.GetLocalizedSprite(key, languageCode);
        
        //// ASSET LOCALIZATION EXTENSIONS ////
        
        /// <summary>
        /// Gets a localized asset by this key for the current language.
        /// Asset localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <typeparam name="T">The type of asset to retrieve (must inherit from UnityEngine.Object)</typeparam>
        /// <param name="key">The asset key</param>
        /// <returns>The localized asset, or null if not found</returns>
        public static T GetLocalizedAsset<T>(this string key) where T : UnityEngine.Object => SIGS.GetLocalizedAsset<T>(key);
        
        /// <summary>
        /// Gets a localized asset by this key for a specific language.
        /// Asset localization is managed exclusively through Unity in the LocBook asset.
        /// </summary>
        /// <typeparam name="T">The type of asset to retrieve (must inherit from UnityEngine.Object)</typeparam>
        /// <param name="key">The asset key</param>
        /// <param name="languageCode">The language code (e.g., "en", "es", "fr")</param>
        /// <returns>The localized asset, or null if not found</returns>
        public static T GetLocalizedAsset<T>(this string key, string languageCode) where T : UnityEngine.Object => SIGS.GetLocalizedAsset<T>(key, languageCode);
        
        //// KEY EXISTENCE CHECK EXTENSIONS ////
        
        /// <summary>
        /// Checks if this localization key exists in the loaded data.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the key exists</returns>
        public static bool HasLocalizationKey(this string key) => SIGS.HasLocalizationKey(key);
        
        /// <summary>
        /// Checks if a localized audio clip exists for this key.
        /// </summary>
        /// <param name="key">The audio key to check</param>
        /// <returns>True if the audio key exists</returns>
        public static bool HasLocalizedAudioClip(this string key) => SIGS.HasLocalizedAudioClip(key);
        
        /// <summary>
        /// Checks if a localized sprite exists for this key.
        /// </summary>
        /// <param name="key">The sprite key to check</param>
        /// <returns>True if the sprite key exists</returns>
        public static bool HasLocalizedSprite(this string key) => SIGS.HasLocalizedSprite(key);
        
        /// <summary>
        /// Checks if a localized asset exists for this key.
        /// </summary>
        /// <param name="key">The asset key to check</param>
        /// <returns>True if the asset key exists</returns>
        public static bool HasLocalizedAsset(this string key) => SIGS.HasLocalizedAsset(key);
    }
}

