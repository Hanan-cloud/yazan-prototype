namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    public static class FrameworkConstants
    {
        /// <summary>
        /// The folder path to the Signalia's Lingramia resources. Used for loading resources.
        /// </summary>
        public const string PATH_RESOURCE = "Signalia/"; // feel free to change this if you have a different folder structure

        public static class EditorPrefsKeys
        {
            // welcome fare
            public const string WELCOME_FARE = "Signalia_WelcomeFare";
        }

        public static class StringConstants
        {
            public const string NOAUDIO = "NOAUDIO";
        }

        public static class GraphicPaths
        {
            public const string HEADER_LOCALIZATION_LINGRAMIA_SETTINGS = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_lingramia_settings.png";
            // START PATH
            public const string GRAPHICS_PATH = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/";

            // FRAMEWORK
            public const string HEADER_SIGNALIA = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/signalia.png";
            public const string HEADER_SIGNALIA_THANKYOU = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/signalia_thankyou.png";
            public const string HEADER_SIGNALIA_PACKAGES = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/signalia_packages.png";
            public const string HEADER_SYSTEM_VITALS = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/sys_vitals.png";

            // LOCALIZATION
            public const string HEADER_LOCALIZATION = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localizationheader.png";
            public const string HEADER_LOCALIZATION_LOCBOOK = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_locbook.png";
            public const string HEADER_LOCALIZATION_TEXT = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_text.png";
            public const string HEADER_LOCALIZATION_TEXTSIMPLE = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_texts.png";
            public const string HEADER_LOCALIZATION_TEXTSTYLE = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_textstyle.png";
            public const string HEADER_LOCALIZATION_IMAGE = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_image.png";
            public const string HEADER_LOCALIZATION_AUDIO = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_audio.png";
            public const string HEADER_LOCALIZATION_REFRESH = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_refresh.png";
            public const string HEADER_LOCALIZATION_SWITCH = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_switch.png";
            public const string HEADER_LOCALIZATION_LINGRAMIA_ICON = "Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_lingramia_icon.png";
        }

        /// <summary>
        /// Leads to the ending file name + package enum with no extension.
        /// </summary>
        public static class PackagePaths
        {
            public const string BlobsPath = "Assets/AHAKuo Creations/Lingramia/Framework/PackageHandlers/packageinfos.info";
            public static string PackagePathResolver(string path, string packageName)
            {
                return $"Assets/AHAKuo Creations/Lingramia/Game Systems/{path}/{packageName}.sigsnature";
            }
        }

        public static class MiscPaths
        {
            /// this branch only has localization standalone guides
            public const string GETTING_STARTED_EN = "Assets/AHAKuo Creations/Lingramia/Offline Documentation/Localization System/guide_en.pdf";
            public const string GETTING_STARTED_AR = "Assets/AHAKuo Creations/Lingramia/Offline Documentation/Localization System/guide_ar.pdf";
        }

        public static class DebugTutorials
        {
            public const string PathToPurchases = "Tools > Signalia > Packages";
            public const string ToolbarPath_Purchases = "Tools/Signalia/Packages";
        }

        public static class InternalListeners
        {
            public const string ANYINPUT_FORCUSTOMINPUT = "sigs_internal_anyinputinvoked";
        }
    }
}