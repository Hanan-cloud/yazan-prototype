using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    [InitializeOnLoad]
    public class Welcomefare : EditorWindow
    {
        private static Texture2D signaliaLogo;
        public const string discordUrl = "https://discord.gg/QcFnVfQj5K";
        public const string reviewUrl = "https://assetstore.unity.com/packages/slug/342440"; // << signalia localization standalone review url
        public const string support = "https://ahakuo.com/contact-page";

        static Welcomefare()
        {
            EditorApplication.delayCall += OpenWindow;
        }

        private static void OpenWindow()
        {
            if (EditorPrefs.GetBool(FrameworkConstants.EditorPrefsKeys.WELCOME_FARE, false))
            {
                return;
            }

            Welcomefare window = GetWindow<Welcomefare>(true, "Signalia's Lingramia - Localization Standalone: Getting Started", true);
            window.minSize = new Vector2(410, 550);
            window.maxSize = new Vector2(410, 600);
            window.Show();
        }

        [MenuItem("Tools/Signalia Localization/Thank You Page")]
        private static void ShowWindow()
        {
            Welcomefare window = GetWindow<Welcomefare>(true, "Signalia's Lingramia - Localization Standalone: Getting Started", true);
            window.minSize = new Vector2(410, 550);
            window.maxSize = new Vector2(410, 600);
            window.Show();
        }

        [MenuItem("Tools/Signalia Localization/Getting Started/English")]
        private static void OpenEnglishGuide()
        {
            OpenPdfFile(FrameworkConstants.MiscPaths.GETTING_STARTED_EN);
        }

        [MenuItem("Tools/Signalia Localization/Getting Started/Arabic")]
        private static void OpenArabicGuide()
        {
            OpenPdfFile(FrameworkConstants.MiscPaths.GETTING_STARTED_AR);
        }

        private static void OpenPdfFile(string relativePath)
        {
            string fullPath = Path.Combine(Application.dataPath, relativePath.Substring("Assets/".Length));

            try
            {
                Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Failed to open Getting Started file: " + e.Message);
            }
        }

        private void OnEnable()
        {
            signaliaLogo = GraphicLoader.LocalizationLingramiaSettings;
        }

        private void DrawHeader()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 256);
            float imageWidth = 512;
            float imageHeight = 256;
            float x = (rect.width - imageWidth) * 0.5f;
            Rect imageRect = new Rect(rect.x + x, rect.y, imageWidth, imageHeight);
            GUI.DrawTexture(imageRect, signaliaLogo, ScaleMode.ScaleToFit);
        }

        private void OnGUI()
        {
            DrawHeader();

            GUILayout.Space(10);

            // Centered Thank You Text
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Thank you for purchasing Signalia's Lingramia - Localization Standalone!", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("This standalone version contains the complete localization system.", EditorStyles.wordWrappedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Your support keeps development going.", EditorStyles.wordWrappedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Leaving a review helps Signalia's Lingramia grow!", EditorStyles.wordWrappedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            // getting started local doc opener - both options
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Getting Started Guides:", EditorStyles.wordWrappedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("English Guide", GUILayout.Width(120)))
            {
                OpenPdfFile(FrameworkConstants.MiscPaths.GETTING_STARTED_EN);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Arabic Guide", GUILayout.Width(120)))
            {
                OpenPdfFile(FrameworkConstants.MiscPaths.GETTING_STARTED_AR);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            // review button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Leave a Review", GUILayout.Width(120)))
            {
                Application.OpenURL(reviewUrl);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // centered support section
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Need help or have a feature request?", EditorStyles.wordWrappedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Join Discord", GUILayout.Width(120)))
            {
                Application.OpenURL(discordUrl);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Support", GUILayout.Width(120)))
            {
                Application.OpenURL(support);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Centered Close Button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close and Don't Show Again", GUILayout.Width(256)))
            {
                Close();
                EditorPrefs.SetBool(FrameworkConstants.EditorPrefsKeys.WELCOME_FARE, true);
                EditorUtility.DisplayDialog("Thank you!", "Thank you for purchasing Signalia's Lingramia - Localization Standalone! This version contains the complete localization system. You can find the documentation under Tools > Signalia Localization > Documentations. To show this page again, use Signalia Localization > Thank You Page", "OK");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}