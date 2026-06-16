using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    public static class RuntimeValues
    {
        public static LingramiaConfigAsset Config => ConfigReader.GetConfig();

        /// <summary>
        /// Called by the config booter. Resets all runtime values to default.
        /// Can be called at any time to reset the runtime values, careful as it can break the system if called at the wrong time.
        /// </summary>
        public static void ResetRuntimeValues()
        {
            InputDelegation.ResetInputs();
        }

        public static class Debugging
        {
            public static bool IsDebugging => Config != null && Config.EnableDebugging;
        }

        public static class InputDelegation
        {
            public static Dictionary<string, (bool oneShot, Action action)> AnyInputDownDelegates = new();
            public static bool CustomInputSystemAnyCall = false;

            public static void Initialize()
            {
                // Input initialization if needed
            }

            public static void Update()
            {
                // Check if any input is down and invoke the event if so
                if (AnyInputDown())
                {
                    InvokeAnyInputDown();
                    CustomInputSystemAnyCall = false;
                }
            }

            /// <summary>
            /// Subscribes to any input down event. This will be invoked when any input is detected.
            /// </summary>
            /// <param name="action"></param>
            /// <param name="oneShot"></param>
            public static void SubscribeToAnyInputDown(Action action, bool oneShot = false)
            {
                Watchman.Watch();

                if (action == null) { return; }

                string key = action.Method.Name + (oneShot ? "_OneShot" : "_Persistent");
                if (AnyInputDownDelegates.ContainsKey(key))
                {
                    Debug.LogWarning($"[Signalia's Lingramia] Input delegate with key {key} already exists. Overwriting the existing delegate.");
                }
                AnyInputDownDelegates[key] = (oneShot, action);
            }

            public static void ResetInputs()
            {
                // Reset the input delegation
                AnyInputDownDelegates.Clear();

                // Reset
                CustomInputSystemAnyCall = false;
            }

            public static void InvokeAnyInputDown()
            {
                Watchman.Watch();

                var keysToRemove = new List<string>();
                keysToRemove.AddRange(AnyInputDownDelegates.Keys.Where(k => AnyInputDownDelegates[k].oneShot));

                // Invoke all actions that are subscribed to the any input down event
                foreach (var kvp in AnyInputDownDelegates)
                {
                    AnyInputDownDelegates[kvp.Key].action?.Invoke();
                }

                // remove after loop to avoid modifying the collection while iterating
                foreach (var key in keysToRemove)
                {
                    AnyInputDownDelegates.Remove(key);
                }
            }

            private static bool AnyInputDown()
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                return Input.GetMouseButtonDown(0) ||
                       Input.GetMouseButtonDown(1) ||
                       Input.GetMouseButtonDown(2) ||
                       Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began ||
                       Input.anyKeyDown;
#else
                return CustomInputSystemAnyCall || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
            }
        }
    }
}