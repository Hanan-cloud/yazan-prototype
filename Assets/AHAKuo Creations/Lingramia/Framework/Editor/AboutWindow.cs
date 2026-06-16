#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework.Editors
{
    public class AboutWindow : EditorWindow
    {
        private static Texture2D signaliaLogo;
        private VersionInfo versionInfo;
        private Vector2 scrollPosition;
        
        public const string discordUrl = "https://discord.gg/QcFnVfQj5K";
        public const string reviewUrl = "https://assetstore.unity.com/packages/slug/342440";
        public const string support = "https://ahakuo.com/contact-page";
        
        private const string VersionInfoPath = "Assets/AHAKuo Creations/Lingramia/Framework/version.info";

        [MenuItem("Tools/Signalia Localization/About", false, 250)]
        public static void ShowWindow()
        {
            AboutWindow window = GetWindow<AboutWindow>(true, "About Signalia Localization", true);
            window.minSize = new Vector2(400, 450);
            window.maxSize = new Vector2(400, 600);
            window.Show();
        }

        private void OnEnable()
        {
            signaliaLogo = GraphicLoader.LocalizationLingramiaSettings;
            LoadVersionInfo();
        }

        private void LoadVersionInfo()
        {
            try
            {
                if (File.Exists(VersionInfoPath))
                {
                    string jsonContent = File.ReadAllText(VersionInfoPath);
                    versionInfo = JsonUtility.FromJson<VersionInfo>(jsonContent);
                }
                else
                {
                    Debug.LogWarning($"[Signalia Localization] Version info file not found at: {VersionInfoPath}");
                    // Create default version info
                    versionInfo = new VersionInfo
                    {
                        package = "Signalia's Lingramia",
                        version = "Unknown",
                        date = "Unknown",
                        company = "AHAKuo Creations"
                    };
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Signalia Localization] Failed to load version info: {e.Message}");
                versionInfo = new VersionInfo
                {
                    package = "Signalia's Lingramia",
                    version = "Unknown",
                    date = "Unknown",
                    company = "AHAKuo Creations"
                };
            }
        }

        private void DrawHeader()
        {
            if (signaliaLogo != null)
            {
                Rect rect = EditorGUILayout.GetControlRect(false, 256);
                float imageWidth = 512;
                float imageHeight = 256;
                float x = (rect.width - imageWidth) * 0.5f;
                Rect imageRect = new Rect(rect.x + x, rect.y, imageWidth, imageHeight);
                GUI.DrawTexture(imageRect, signaliaLogo, ScaleMode.ScaleToFit);
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            GUILayout.Space(25);
            
            DrawHeader();
            
            GUILayout.Space(20);

            // Package Name
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 18,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label(versionInfo != null ? versionInfo.package : "Signalia's Lingramia", titleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(25);

            // Version Information Section
            if (versionInfo != null)
            {
                DrawInfoSection("Version Information", () =>
                {
                    DrawInfoRow("Package:", versionInfo.package);
                    DrawInfoRow("Version:", versionInfo.version);
                    DrawInfoRow("Release Date:", versionInfo.date);
                    DrawInfoRow("Commit:", versionInfo.commit ?? "—");
                    DrawInfoRow("Company:", versionInfo.company);
                });
            }

            GUILayout.Space(20);

            // Links Section
            DrawInfoSection("Links", () =>
            {
                // First row: Leave a Review and Join Discord
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Leave a Review", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    Application.OpenURL(reviewUrl);
                }
                GUILayout.Space(15);
                if (GUILayout.Button("Join Discord", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    Application.OpenURL(discordUrl);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(12);

                // Second row: Support (centered)
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Support", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    Application.OpenURL(support);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            });

            GUILayout.Space(20);

            // Close Button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", GUILayout.Width(120), GUILayout.Height(30)))
            {
                Close();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            EditorGUILayout.EndScrollView();
        }

        private void DrawInfoSection(string title, System.Action content)
        {
            GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(20, 20, 15, 15),
                margin = new RectOffset(10, 10, 5, 5)
            };
            
            EditorGUILayout.BeginVertical(boxStyle);
            
            GUIStyle sectionTitleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label(title, sectionTitleStyle);
            
            GUILayout.Space(15);
            
            content?.Invoke();
            
            EditorGUILayout.EndVertical();
        }

        private void DrawInfoRow(string label, string value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(120));
            GUILayout.Space(10);
            GUILayout.Label(value, EditorStyles.label);
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
        }
    }
}
#endif
