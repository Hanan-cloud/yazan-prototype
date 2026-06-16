using UnityEngine;
using System.IO;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    public static class ConfigReader
    {
        private static LingramiaConfigAsset cachedConfig;
        private const string ResourcePath = "Lingramia/LinConfig"; // Path inside Resources folder
        private const string EditorPath = "Assets/Resources/Lingramia/LinConfig.asset"; // Editor-only save location

        /// <summary>
        /// Loads the config from Resources. If in Editor and missing, creates a default one.
        /// </summary>
        public static LingramiaConfigAsset GetConfig(bool forceLoad = false)
        {
            if (forceLoad)
            {
                LoadConfig();
            }

            if (cachedConfig == null)
            {
                LoadConfig();
            }
            return cachedConfig;
        }

        private static void LoadConfig()
        {
#if UNITY_EDITOR
            // In editor, load from AssetDatabase to get editable instance
            cachedConfig = UnityEditor.AssetDatabase.LoadAssetAtPath<LingramiaConfigAsset>(EditorPath);
            
            if (cachedConfig == null)
            {
                if (!ShouldAutoCreateConfig())
                {
                    Debug.LogError("[Lingramia] LingramiaConfigAsset not found in Resources/Lingramia/LinConfig.asset. " +
                                   "Auto-creation is suppressed in batch mode or while the AssetDatabase is still initializing.");
                    return;
                }
                
                // Check if Signalia folder structure exists
                if (!Directory.Exists("Assets/Resources/Lingramia"))
                {
                    Debug.LogWarning("Lingramia folder structure not found. Creating default folder structure...");
                    
                    if (!Directory.Exists("Assets/Resources"))
                    {
                        Directory.CreateDirectory("Assets/Resources");
                        Debug.Log("Created Assets/Resources folder");
                    }
                    
                    Directory.CreateDirectory("Assets/Resources/Lingramia");
                    Debug.Log("Created Assets/Resources/Lingramia folder");
                }

                // Create a new config asset in the Resources folder if it doesn't exist (Editor Only)
                cachedConfig = ScriptableObject.CreateInstance<LingramiaConfigAsset>();

                UnityEditor.AssetDatabase.CreateAsset(cachedConfig, EditorPath);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
                
                Debug.Log("Created new LingramiaConfigAsset at " + EditorPath);
            }
#else
            // At runtime, load from Resources
            cachedConfig = Resources.Load<LingramiaConfigAsset>(ResourcePath);
            
            if (cachedConfig == null)
            {
                Debug.LogError("LingramiaConfigAsset not found in Resources/Lingramia/LinConfig.asset. " +
                             "Please ensure the Lingramia folder structure exists and contains the config asset.");
            }
#endif
        }
        
#if UNITY_EDITOR
        private static bool ShouldAutoCreateConfig()
        {
            if (Application.isBatchMode)
            {
                return false;
            }

            if (UnityEditor.EditorApplication.isCompiling || UnityEditor.EditorApplication.isUpdating)
            {
                return false;
            }

#if UNITY_2020_1_OR_NEWER
            if (UnityEditor.AssetDatabase.IsAssetImportWorkerProcess())
            {
                return false;
            }
#endif

            return true;
        }
#endif

#if UNITY_EDITOR
        /// <summary>
        /// Saves changes made to the config asset in the Editor.
        /// </summary>
        /// <param name="configToSave">Optional config instance to save. If null, uses cached config.</param>
        public static void SaveConfig(LingramiaConfigAsset configToSave = null)
        {
            LingramiaConfigAsset config = configToSave ?? cachedConfig;
            
            if (config == null)
            {
                Debug.LogWarning("[Signalia Config] Cannot save config: config is null. Attempting to reload...");
                LoadConfig();
                config = cachedConfig;
            }
            
            if (config != null)
            {
                UnityEditor.EditorUtility.SetDirty(config);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
                Debug.Log($"[Signalia Config] Config saved successfully at: {UnityEditor.AssetDatabase.GetAssetPath(config)}");
            }
            else
            {
                Debug.LogError("[Signalia Config] Failed to save config: config is still null after reload attempt.");
            }
        }
#endif
    }
}