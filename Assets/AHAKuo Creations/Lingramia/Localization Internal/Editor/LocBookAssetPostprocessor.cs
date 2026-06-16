#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using AHAKuo.Signalia.LocalizationStandalone.Framework;
using System.Linq;

namespace AHAKuo.Signalia.LocalizationStandalone.Internal.Editors
{
    /// <summary>
    /// Asset postprocessor that monitors .locbook file imports and automatically updates
    /// LocBook assets when their referenced files change.
    /// </summary>
    public class LocBookAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Check if auto-update is enabled
            LingramiaConfigAsset config = ConfigReader.GetConfig();
            if (config == null || !config.LocalizationSystem.AutoUpdateLocbooks)
            {
                return;
            }

            // Check if any .locbook files were imported
            bool hasLocbookFiles = importedAssets.Any(path => path.EndsWith(".locbook"));
            
            if (!hasLocbookFiles)
            {
                return;
            }

            // Find all LocBook assets in the project
            string[] locBookGuids = AssetDatabase.FindAssets("t:LocBook");
            
            foreach (string guid in locBookGuids)
            {
                string locBookPath = AssetDatabase.GUIDToAssetPath(guid);
                LocBook locBook = AssetDatabase.LoadAssetAtPath<LocBook>(locBookPath);
                
                if (locBook == null || locBook.LocbookFile == null)
                {
                    continue;
                }
                
                // Get the path of the referenced .locbook file
                string locbookFilePath = AssetDatabase.GetAssetPath(locBook.LocbookFile);
                
                if (string.IsNullOrEmpty(locbookFilePath))
                {
                    continue;
                }
                
                // Check if this .locbook file was imported
                if (importedAssets.Contains(locbookFilePath))
                {
                    // Check if the file has actually changed by comparing content hash
                    if (HasLocbookFileChanged(locBook, locbookFilePath))
                    {
                        Debug.Log($"[Signalia LocBook] Auto-updating {locBook.name} due to changes in {locbookFilePath}");
                        
                        try
                        {
                            locBook.UpdateAssetFromFile();
                            EditorUtility.SetDirty(locBook);
                            AssetDatabase.SaveAssets();
                            
                            // If auto-refresh in runtime is enabled and the game is playing, refresh the localization cache
                            if (config.LocalizationSystem.AutoRefreshCacheInRuntime && Application.isPlaying)
                            {
                                Debug.Log($"[Signalia LocBook] Refreshing localization cache in runtime for {locBook.name}");
                                
                                // Reinitialize the localization system to reload the updated data
                                Localization.Initialize(config.LocalizationSystem.LocBooks);
                                
                                // Trigger language changed event to update any UI elements
                                SIGS.TriggerLanguageChange();
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"[Signalia LocBook] Failed to auto-update {locBook.name}: {e.Message}");
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Checks if the .locbook file has changed by comparing entry counts and basic structure.
        /// This helps avoid unnecessary updates when the file hasn't actually changed.
        /// </summary>
        private static bool HasLocbookFileChanged(LocBook locBook, string locbookFilePath)
        {
            try
            {
                // Read the file content
                string json = System.IO.File.ReadAllText(locbookFilePath);
                
                if (string.IsNullOrEmpty(json))
                {
                    return false;
                }
                
                // Try to parse as External format
                var externalData = JsonUtility.FromJson<LocBook.ExternalLocBookData>(json);
                
                if (externalData != null && externalData.pages != null)
                {
                    // Count entries in the file
                    int fileEntryCount = 0;
                    foreach (var page in externalData.pages)
                    {
                        if (page.pageFiles != null)
                        {
                            fileEntryCount += page.pageFiles.Count;
                        }
                    }
                    
                    // Compare with current entry count
                    int currentEntryCount = locBook.EntryCount;
                    
                    // If counts differ, definitely changed
                    if (fileEntryCount != currentEntryCount)
                    {
                        return true;
                    }
                    
                    // If counts are the same, check page structure
                    if (externalData.pages.Count != locBook.PageCount)
                    {
                        return true;
                    }
                    
                    // Additional check: compare first entry's originalValue if available
                    if (externalData.pages.Count > 0 && 
                        externalData.pages[0].pageFiles != null && 
                        externalData.pages[0].pageFiles.Count > 0 &&
                        locBook.Pages.Count > 0 &&
                        locBook.Pages[0].entries.Count > 0)
                    {
                        string fileFirstValue = externalData.pages[0].pageFiles[0].originalValue;
                        string assetFirstValue = locBook.Pages[0].entries[0].originalValue;
                        
                        if (fileFirstValue != assetFirstValue)
                        {
                            return true;
                        }
                    }
                }
                
                // If we can't determine, assume it changed to be safe
                return true;
            }
            catch (System.Exception)
            {
                // If there's an error parsing, assume it changed
                return true;
            }
        }
    }
}
#endif
