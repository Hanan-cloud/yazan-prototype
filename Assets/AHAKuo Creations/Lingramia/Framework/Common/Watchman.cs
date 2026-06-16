using UnityEngine;
using System;
using AHAKuo.Signalia.LocalizationStandalone.GameSystems;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    /// <summary>
    /// Watches over runtime sticky "static" Signalia values and disposing them.
    /// </summary>
    public class Watchman : MonoBehaviour
    {
        /// <summary>
        /// Only called when the game is closed, as this object is not destroyed.
        /// </summary>
        public static event Action OnTermination;
        private static Watchman instance;
        public static Watchman Instance => instance;

        public static bool IsQuitting { get; private set; } = false;

        private void OnApplicationQuit()
        {
            IsQuitting = true;

            GameSystemsHandler.ShutdownProcesses();
        }

        /// <summary>
        /// No need to call this yourself manually, as it is called automatically when any signalia tool is used.
        /// </summary>
        public static void Watch()
        {
            if (!Application.isPlaying)
                return;

            // Reset IsQuitting if no instance exists (breaks catch-22 where IsQuitting prevents Watchman creation)
            if (instance == null)
                IsQuitting = false;

            if (IsQuitting)
                return;

            // spawn if none exists
            if (instance == null)
            {
                var go = new GameObject("Signalia Watchman");
                instance = go.AddComponent<Watchman>();
            }
        }

        private void Awake()
        {
            IsQuitting = false; // reset quitting state

            Application.quitting += () => IsQuitting = true; // set quitting state

            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            if (ConfigReader.GetConfig().KeepManagerAlive)
                DontDestroyOnLoad(gameObject);

            // subscribe to scene loaded event
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoaded;

            // initialize game systems
            GameSystemsHandler.InitializeGameSystems();
        }

        private void Update()
        {   
            // No update needed for localization-only system
        }

        private void SceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            // No scene loaded handling needed for localization-only system
        }

        /// <summary>
        /// Most likely only called when the game is closed, as this object is not destroyed. **if keep manager alive is set to true, otherwise, this is called when the scene is unloaded.**
        /// </summary>
        private void OnDestroy()
        {
            if (instance != this
                && instance != null) return; // not the instance

            ResetEverything(false); // reset everything
        }

        /// <summary>
        /// Resets all static values and destroys the watchman instance if specified.
        /// </summary>
        /// <param name="destroyWatchman"></param>
        public static void ResetEverything(bool destroyWatchman)
        {
            if (instance == null) return;

            OnTermination?.Invoke(); // invoke termination event

            // reset all static values
            RuntimeValues.ResetRuntimeValues();
            GameSystemsHandler.CleanupGameSystems();

            if (!destroyWatchman) return;

            // destroy me
            Destroy(instance.gameObject);
        }
    }
}