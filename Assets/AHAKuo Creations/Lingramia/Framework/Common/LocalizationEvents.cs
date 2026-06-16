using System;
using UnityEngine;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework
{
    /// <summary>
    /// Simplified type-explicit event system for localization language changes.
    /// This replaces the Radio system for localization-specific events.
    /// </summary>
    public static class LocalizationEvents
    {
        /// <summary>
        /// Non-modifiable event string constant for language changes.
        /// </summary>
        public const string LANGUAGE_CHANGED_EVENT = "Signalia_LanguageChanged";

        /// <summary>
        /// Event that fires when the language is changed.
        /// The string parameter is the new language code.
        /// </summary>
        public static event Action<string> OnLanguageChanged;

        /// <summary>
        /// Triggers the language changed event.
        /// This is called internally by the localization system and via SIGS.TriggerLanguageChange().
        /// </summary>
        /// <param name="languageCode">The new language code</param>
        public static void TriggerLanguageChange(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                OnLanguageChanged?.Invoke(languageCode);
            }
        }

        /// <summary>
        /// Subscribes to language change events with automatic cleanup when the GameObject is destroyed.
        /// </summary>
        /// <param name="action">Action to perform when language changes</param>
        /// <param name="context">GameObject context for automatic cleanup</param>
        /// <returns>A disposable subscription that can be manually disposed</returns>
        public static LanguageChangeSubscription Subscribe(Action action, GameObject context = null)
        {
            Action<string> wrappedAction = (code) => action?.Invoke();
            OnLanguageChanged += wrappedAction;

            var subscription = new LanguageChangeSubscription(wrappedAction, context);
            
            // Register for cleanup if context provided
            if (context != null)
            {
                Watchman.OnTermination += subscription.Dispose;
            }

            return subscription;
        }

        /// <summary>
        /// Subscribes to language change events with the new language code parameter.
        /// </summary>
        /// <param name="action">Action to perform when language changes, receives the new language code</param>
        /// <param name="context">GameObject context for automatic cleanup</param>
        /// <returns>A disposable subscription that can be manually disposed</returns>
        public static LanguageChangeSubscription SubscribeWithLanguageCode(Action<string> action, GameObject context = null)
        {
            OnLanguageChanged += action;

            var subscription = new LanguageChangeSubscription(action, context);
            
            // Register for cleanup if context provided
            if (context != null)
            {
                Watchman.OnTermination += subscription.Dispose;
            }

            return subscription;
        }

        /// <summary>
        /// Represents a subscription to language change events that can be disposed.
        /// </summary>
        public class LanguageChangeSubscription
        {
            private Action<string> action;
            private GameObject context;
            private bool isDisposed = false;

            internal LanguageChangeSubscription(Action<string> action, GameObject context)
            {
                this.action = action;
                this.context = context;
            }

            /// <summary>
            /// Unsubscribes from the language change event.
            /// </summary>
            public void Dispose()
            {
                if (isDisposed) return;

                if (action != null)
                {
                    LocalizationEvents.OnLanguageChanged -= action;
                }

                isDisposed = true;
            }

            /// <summary>
            /// Checks if this subscription has been disposed.
            /// </summary>
            public bool IsDisposed => isDisposed;
        }
    }
}
