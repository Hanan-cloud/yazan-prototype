#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework.Editors
{
    public class FrameworkSettings : EditorWindow
    {
        private Vector2 scrollPosition;
        private LingramiaConfigAsset config;

        private readonly string[] tabs = { "Localization", "Debug" };
        private int selectedTab;

        private readonly string[] localizationTabs = { "Internal", "External" };
        private int localizationTabIndex;

        private const float ButtonHeight = 28f;

        [MenuItem("Tools/Signalia Localization/Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<FrameworkSettings>("SIGS - Localization Settings");
            window.minSize = new Vector2(520f, 520f);
            window.Show();
        }

        private void OnEnable()
        {
            config = ConfigReader.GetConfig(true);
        }

        private void OnFocus()
        {
            if (config == null)
            {
                config = ConfigReader.GetConfig(true);
            }
        }

        public static void OpenToTab(int tabIndex)
        {
            var window = GetWindow<FrameworkSettings>("SIGS - Localization Settings");
            window.minSize = new Vector2(520f, 520f);
            window.selectedTab = Mathf.Clamp(tabIndex, 0, window.tabs.Length - 1);
            window.Show();
        }

        private void OnGUI()
        {
            Styles.Ensure();

            if (config == null)
            {
                DrawMissingConfigState();
                return;
            }
            
            DrawHeader();

            selectedTab = EditorUtilityMethods.RenderToolbar(selectedTab, tabs);
            GUILayout.Space(8f);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            switch (selectedTab)
            {
                case 0:
                    DrawLocalizationTab();
                    break;
                case 1:
                    DrawDebuggingTab();
                    break;
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(12f);
            DrawFooter();
        }

        private void DrawHeader()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 256);
            float imageWidth = 512;
            float imageHeight = 256;
            float x = (rect.width - imageWidth) * 0.5f;
            Rect imageRect = new Rect(rect.x + x, rect.y, imageWidth, imageHeight);
            GUI.DrawTexture(imageRect, Framework.GraphicLoader.LocalizationLingramiaSettings, ScaleMode.ScaleToFit);
        }


        private void DrawLocalizationTab()
        {
            var serializedConfig = new SerializedObject(config);
            serializedConfig.Update();

            localizationTabIndex = EditorUtilityMethods.RenderToolbar(localizationTabIndex, localizationTabs);
            GUILayout.Space(8f);

            if (localizationTabIndex == 0)
            {
                DrawInternalLocalizationTab(serializedConfig);
            }
            else
            {
                DrawExternalLocalizationTab();
            }

            if (serializedConfig.ApplyModifiedProperties())
            {
                MarkConfigDirty();
            }
        }

        private void DrawInternalLocalizationTab(SerializedObject serializedConfig)
        {
            var hybridKeyProperty = serializedConfig.FindProperty("LocalizationSystem.HybridKey");
            var locBooksProperty = serializedConfig.FindProperty("LocalizationSystem.LocBooks");
            var textStylesProperty = serializedConfig.FindProperty("LocalizationSystem.TextStyleCache");
            var defaultLanguageProperty = serializedConfig.FindProperty("LocalizationSystem.DefaultStartingLanguageCode");
            var saveKeyProperty = serializedConfig.FindProperty("LocalizationSystem.LanguageOptionSaveKey");
            var autoUpdateProperty = serializedConfig.FindProperty("LocalizationSystem.AutoUpdateLocbooks");
            var autoRefreshProperty = serializedConfig.FindProperty("LocalizationSystem.AutoRefreshCacheInRuntime");

            DrawSection("Hybrid Key Mode", "Search by both keys and localized values when resolving strings.", () =>
            {
                EditorGUI.BeginChangeCheck();
                bool newValue = EditorGUILayout.ToggleLeft(
                    new GUIContent("Enable Hybrid Key", "When enabled, searches by both key and value and aliases for localization entries."),
                    hybridKeyProperty.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Toggle Hybrid Key");
                    hybridKeyProperty.boolValue = newValue;
                }

                if (hybridKeyProperty.boolValue)
                {
                    EditorGUILayout.HelpBox("Hybrid Key will search strings by both key and value and aliases. Leave disabled unless migrating legacy content.", MessageType.Warning);
                }
            });

            DrawSection("LocBook Assets", "Assign localization books that should be available at runtime.", () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(locBooksProperty, new GUIContent("LocBooks", "ScriptableObject assets that store localization content."), true);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Edit LocBooks");
                }

                int assignedCount = locBooksProperty.arraySize;
                int totalEntries = 0;
                for (int i = 0; i < locBooksProperty.arraySize; i++)
                {
                    var element = locBooksProperty.GetArrayElementAtIndex(i);
                    var locBook = element.objectReferenceValue as LocalizationStandalone.Internal.LocBook;
                    if (locBook != null)
                    {
                        totalEntries += locBook.EntryCount;
                    }
                }

                if (assignedCount == 0)
                {
                    EditorGUILayout.HelpBox("No LocBooks assigned. Create or assign a LocBook asset to enable localization.", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.HelpBox($"{assignedCount} LocBook(s) assigned with {totalEntries} total entries.", MessageType.Info);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Create LocBook", GUILayout.Height(ButtonHeight)))
                    {
                        CreateLocBook();
                    }

                    if (GUILayout.Button("Find LocBooks", GUILayout.Height(ButtonHeight)))
                    {
                        AssignAllLocBooks();
                    }
                }
            });

            DrawSection("Text Styles", "Cache of language-specific typography settings.", () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(textStylesProperty, new GUIContent("Text Styles", "ScriptableObject assets describing per-language typography."), true);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Edit Text Styles");
                }

                int styleCount = textStylesProperty.arraySize;
                if (styleCount == 0)
                {
                    EditorGUILayout.HelpBox("No TextStyle assets cached. Add TextStyle assets to control fonts and formatting per language.", MessageType.Info);
                }
                else
                {
                    for (int i = 0; i < textStylesProperty.arraySize; i++)
                    {
                        var element = textStylesProperty.GetArrayElementAtIndex(i);
                        var style = element.objectReferenceValue as LocalizationStandalone.Internal.TextStyle;
                        if (style != null)
                        {
                            EditorGUILayout.LabelField($"• {style.name} ({style.LanguageCode})", EditorStyles.miniLabel);
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Create TextStyle", GUILayout.Height(ButtonHeight)))
                    {
                        CreateTextStyle();
                    }

                    if (GUILayout.Button("Find TextStyles", GUILayout.Height(ButtonHeight)))
                    {
                        AssignAllTextStyles();
                    }
                }
            });

            DrawSection("Defaults", "Set the language the game loads first and how preferences are stored.", () =>
            {
                EditorGUI.BeginChangeCheck();
                string newLanguage = EditorGUILayout.TextField(
                    new GUIContent("Default Language", "Language code used when no saved preference exists."),
                    defaultLanguageProperty.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Edit Default Language");
                    defaultLanguageProperty.stringValue = newLanguage;
                }

                EditorGUI.BeginChangeCheck();
                string newKey = EditorGUILayout.TextField(
                    new GUIContent("PlayerPrefs Key", "The key used when persisting the selected language."),
                    saveKeyProperty.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Edit Save Key");
                    saveKeyProperty.stringValue = newKey;
                }
            });

            DrawSection("Automation", "Control how editor tools keep localization data updated.", () =>
            {
                EditorGUI.BeginChangeCheck();
                bool autoUpdate = EditorGUILayout.ToggleLeft(
                    new GUIContent("Auto Update LocBooks", "Automatically refresh LocBook assets when their source files change."),
                    autoUpdateProperty.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Toggle Auto Update LocBooks");
                    autoUpdateProperty.boolValue = autoUpdate;
                }

                if (autoUpdateProperty.boolValue)
                {
                    EditorGUILayout.HelpBox("LocBooks will update automatically when their source files change.", MessageType.Info);
                }

                EditorGUI.BeginChangeCheck();
                bool autoRefresh = EditorGUILayout.ToggleLeft(
                    new GUIContent("Auto Refresh Cache In Play Mode", "Refresh cached localization data whenever LocBooks are modified during Play Mode."),
                    autoRefreshProperty.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Toggle Auto Refresh Cache");
                    autoRefreshProperty.boolValue = autoRefresh;
                }

                if (autoRefreshProperty.boolValue)
                {
                    EditorGUILayout.HelpBox("Auto refreshing in Play Mode can impact editor performance. Enable only when you need to observe live updates.", MessageType.Warning);
                }
            });

            DrawSection("Utilities", string.Empty, () =>
            {
                if (GUILayout.Button("View Localization Documentation", GUILayout.Height(ButtonHeight)))
                {
                    EditorUtility.DisplayDialog(
                        "Localization Documentation",
                        "Localization Internal System\n\n" +
                        "Getting Started:\n" +
                        "1. Create a LocBook asset (Create > Signalia Localization > LocBook)\n" +
                        "2. Add localization entries manually or reference a .locbook file\n" +
                        "3. Assign the LocBook in these settings\n" +
                        "4. Call SIGS.InitializeLocalization() at game start\n" +
                        "5. Use SIGS.GetLocalizedString(key) or LocalizedText component\n\n" +
                        "For more information, check the Signalia's Lingramia documentation.",
                        "OK");
                }

                if (GUILayout.Button("Initialize System (Play Mode)", GUILayout.Height(ButtonHeight)))
                {
                    if (Application.isPlaying)
                    {
                        if (config.LocalizationSystem.LocBooks != null && config.LocalizationSystem.LocBooks.Length > 0)
                        {
                            SIGS.InitializeLocalization();
                            EditorUtility.DisplayDialog("Localization Initialized", "The localization system has been initialized with the configured LocBooks.", "OK");
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("No LocBooks", "Assign at least one LocBook before initializing.", "OK");
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Not In Play Mode", "Localization can only be initialized while the editor is in Play Mode.", "OK");
                    }
                }
            });
        }

        private void DrawExternalLocalizationTab()
        {
            DrawSection("External Workflow", "Manage Lingramia-powered localization by working directly with LocBook assets.", () =>
            {
                EditorGUILayout.HelpBox("Each LocBook can reference a .locbook JSON file edited in Lingramia. Import updated files to synchronize content.", MessageType.Info);
            });
        }

        private void DrawDebuggingTab()
        {
            var serializedConfig = new SerializedObject(config);
            serializedConfig.Update();

            var debugProperty = serializedConfig.FindProperty("EnableDebugging");

            DrawSection("Debugging", "Turn on verbose logging for localization systems.", () =>
            {
                EditorGUI.BeginChangeCheck();
                bool newValue = EditorGUILayout.ToggleLeft(new GUIContent("Enable Localization Debug Logs"), debugProperty.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    RecordConfigChange("Toggle Localization Debugging");
                    debugProperty.boolValue = newValue;
                }
            });

            if (serializedConfig.ApplyModifiedProperties())
            {
                MarkConfigDirty();
            }
        }

        private void DrawFooter()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save Settings", Styles.SaveButton))
            {
                // Ensure any pending SerializedObject changes are applied before saving
                if (config != null)
                {
                    var serializedConfig = new SerializedObject(config);
                    serializedConfig.Update();
                    serializedConfig.ApplyModifiedProperties();
                }
                
                ConfigReader.SaveConfig(config);
                EditorGUI.FocusTextInControl(null);
                EditorUtility.DisplayDialog("Settings Saved", "Signalia's Lingramia settings have been saved.", "OK");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            // space after so it doesnt scrunch
            GUILayout.Space(12f);
        }

        private void DrawMissingConfigState()
        {
            GUILayout.Space(12f);
            EditorGUILayout.HelpBox("Signalia's Lingramia configuration asset could not be found. A new one will be generated when you continue.", MessageType.Warning);

            if (GUILayout.Button("Create Configuration", GUILayout.Height(ButtonHeight)))
            {
                config = ConfigReader.GetConfig(true);
                if (config != null)
                {
                    EditorUtility.DisplayDialog("Config Created", "A Signalia's Lingramia configuration asset has been generated in Resources/Signalia.", "OK");
                }
            }
        }

        private void DrawSection(string title, string description, Action content)
        {
            using (new EditorGUILayout.VerticalScope(Styles.Section))
            {
                EditorGUILayout.LabelField(title, Styles.SectionHeader);
                if (!string.IsNullOrEmpty(description))
                {
                    EditorGUILayout.LabelField(description, Styles.SectionDescription);
                }

                content?.Invoke();
            }

            GUILayout.Space(6f);
        }

        private void CreateLocBook()
        {
            var locBook = ScriptableObject.CreateInstance<LocalizationStandalone.Internal.LocBook>();
            locBook.name = "New LocBook";
            var path = EditorUtility.SaveFilePanelInProject("Save LocBook", "New LocBook", "asset", "Choose where to store the LocBook asset.");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            AssetDatabase.CreateAsset(locBook, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RecordConfigChange("Add LocBook");

            var current = new List<LocalizationStandalone.Internal.LocBook>(config.LocalizationSystem.LocBooks ?? Array.Empty<LocalizationStandalone.Internal.LocBook>());
            current.Add(locBook);
            config.LocalizationSystem.LocBooks = current.ToArray();
            MarkConfigDirty();
        }

        private void AssignAllLocBooks()
        {
            var locBooks = new List<LocalizationStandalone.Internal.LocBook>();
            string[] guids = AssetDatabase.FindAssets("t:LocBook", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var locBook = AssetDatabase.LoadAssetAtPath<LocalizationStandalone.Internal.LocBook>(path);
                if (locBook != null)
                {
                    locBooks.Add(locBook);
                }
            }

            RecordConfigChange("Assign All LocBooks");
            config.LocalizationSystem.LocBooks = locBooks.ToArray();
            MarkConfigDirty();
            Debug.Log($"[Signalia's Lingramia] Found and assigned {locBooks.Count} LocBook(s).");
        }

        private void CreateTextStyle()
        {
            var textStyle = ScriptableObject.CreateInstance<LocalizationStandalone.Internal.TextStyle>();
            textStyle.name = "New TextStyle";
            var path = EditorUtility.SaveFilePanelInProject("Save TextStyle", "New TextStyle", "asset", "Choose where to store the TextStyle asset.");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            AssetDatabase.CreateAsset(textStyle, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void AssignAllTextStyles()
        {
            var styles = new List<LocalizationStandalone.Internal.TextStyle>();
            if (config.LocalizationSystem.TextStyleCache != null)
            {
                styles.AddRange(config.LocalizationSystem.TextStyleCache);
            }

            string[] guids = AssetDatabase.FindAssets("t:TextStyle", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var style = AssetDatabase.LoadAssetAtPath<LocalizationStandalone.Internal.TextStyle>(path);
                if (style != null && !styles.Contains(style))
                {
                    styles.Add(style);
                }
            }

            RecordConfigChange("Assign All TextStyles");
            config.LocalizationSystem.TextStyleCache = styles.ToArray();
            MarkConfigDirty();
            Debug.Log($"[Signalia's Lingramia] Found and assigned {styles.Count} TextStyle(s).");
        }

        private void RecordConfigChange(string label)
        {
            if (config == null)
            {
                return;
            }

            Undo.RecordObject(config, label);
            MarkConfigDirty();
        }

        private void MarkConfigDirty()
        {
            if (config != null)
            {
                EditorUtility.SetDirty(config);
            }
        }

        private static class Styles
        {
            private static bool initialized;

            public static GUIContent HeaderIcon;
            public static GUIStyle HeaderTitle;
            public static GUIStyle HeaderSubtitle;
            public static GUIStyle Section;
            public static GUIStyle SectionHeader;
            public static GUIStyle SectionDescription;
            public static GUIStyle TabLeft;
            public static GUIStyle TabMid;
            public static GUIStyle TabRight;
            public static GUIStyle TabSolo;
            public static GUIStyle SaveButton;

            public static void Ensure()
            {
                if (initialized)
                {
                    return;
                }

                HeaderIcon = EditorGUIUtility.IconContent("SettingsIcon");
                if (HeaderIcon.image == null)
                {
                    HeaderIcon = EditorGUIUtility.IconContent("ScriptableObject Icon");
                }

                HeaderTitle = new GUIStyle(EditorStyles.label)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    margin = new RectOffset(0, 0, 2, 0)
                };

                HeaderSubtitle = new GUIStyle(EditorStyles.wordWrappedMiniLabel)
                {
                    richText = false
                };

                Section = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(12, 12, 10, 12),
                    margin = new RectOffset(0, 0, 8, 8)
                };

                SectionHeader = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 12
                };

                SectionDescription = new GUIStyle(EditorStyles.wordWrappedMiniLabel)
                {
                    margin = new RectOffset(0, 0, 0, 6)
                };

                TabLeft = new GUIStyle(EditorStyles.miniButtonLeft)
                {
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 24f
                };

                TabMid = new GUIStyle(EditorStyles.miniButtonMid)
                {
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 24f
                };

                TabRight = new GUIStyle(EditorStyles.miniButtonRight)
                {
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 24f
                };

                TabSolo = new GUIStyle(EditorStyles.miniButton)
                {
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 24f
                };

                SaveButton = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 36f,
                    padding = new RectOffset(16, 16, 6, 6)
                };

                initialized = true;
            }

            public static GUIStyle GetTabStyle(int index, int length)
            {
                if (length <= 1)
                {
                    return TabSolo;
                }

                if (index == 0)
                {
                    return TabLeft;
                }

                if (index == length - 1)
                {
                    return TabRight;
                }

                return TabMid;
            }
        }
    }
}
#endif
