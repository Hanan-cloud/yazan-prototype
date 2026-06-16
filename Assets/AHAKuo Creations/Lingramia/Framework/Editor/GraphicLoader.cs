using UnityEditor;
using UnityEngine;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    public static class GraphicLoader
    {
        public static Texture2D LoadByName(string n) => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.GRAPHICS_PATH + n, typeof(Texture2D));
        public static Texture2D Signalia => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_SIGNALIA, typeof(Texture2D));
        public static Texture2D SignaliaSettings => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_SIGNALIA_PACKAGES, typeof(Texture2D));
        public static Texture2D SignaliaPackages => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_SIGNALIA_PACKAGES, typeof(Texture2D));
        public static Texture2D SystemVitals => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_SYSTEM_VITALS, typeof(Texture2D));
        public static Texture2D LocalizationHeader => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION, typeof(Texture2D));
        public static Texture2D LocalizationLocbook => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_LOCBOOK, typeof(Texture2D));
        public static Texture2D LocalizationTextStyle => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_TEXTSTYLE, typeof(Texture2D));
        public static Texture2D LocalizationText => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_TEXT, typeof(Texture2D));
        public static Texture2D LocalizationTextSimple => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_TEXTSIMPLE, typeof(Texture2D));
        public static Texture2D LocalizationAudio => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_AUDIO, typeof(Texture2D));
        public static Texture2D LocalizationImage => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_IMAGE, typeof(Texture2D));
        public static Texture2D LocalizationRefresh => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_REFRESH, typeof(Texture2D));
        public static Texture2D LocalizationSwitch => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_SWITCH, typeof(Texture2D));
        public static Texture2D LocalizationLingramiaIcon => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_LINGRAMIA_ICON, typeof(Texture2D));
        public static Texture2D LocalizationLingramiaSettings => (Texture2D)AssetDatabase.LoadAssetAtPath(FrameworkConstants.GraphicPaths.HEADER_LOCALIZATION_LINGRAMIA_SETTINGS, typeof(Texture2D));
    }
}