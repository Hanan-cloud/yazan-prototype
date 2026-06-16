using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.GameSystems
{
    public static class GameSystemsHandler
    {
        public static void InitializeGameSystems()
        {
            // initialize localization system
            SIGS.InitializeLocalization();
        }

        public static void CleanupGameSystems()
        {
            LocalizationStandalone.Internal.Localization.Clear();
        }

        /// <summary>
        /// Method called to shutdown game systems when the application is quitting.
        /// </summary>
        public static void ShutdownProcesses()
        {
            // No shutdown processes needed for localization-only system
        }
    }
}