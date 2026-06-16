using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.Internal
{
	/// <summary>
	/// Helper component that automatically sets localized audio clips on an AudioSource component.
	/// Attach this to any GameObject with an AudioSource to enable automatic audio localization.
	/// Useful for localized voice-overs, narration, or sound effects that vary by language.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu("Signalia Localization/Localized Audio Source")]
	[Icon("Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_audio_icon.png")]
	public class LocalizedAudioSource : MonoBehaviour
	{
		//[Header("Localization Settings")]
		[Tooltip("The localization key for the audio clip. Must match an audio entry in your LocBook.")]
		[SerializeField] private string audioKey;
		
		//[Header("Update Settings")]
		[Tooltip("Automatically update the audio clip when the language changes.")]
		[SerializeField] private bool updateOnLanguageChange = true;
		
		[Tooltip("Set the localized audio clip on Start.")]
		[SerializeField] private bool setOnStart = true;
		
		//[Header("Playback Settings")]
		[Tooltip("Automatically play the audio clip after updating.")]
		[SerializeField] private bool playOnUpdate = false;
		
		[Tooltip("Stop the current audio before switching to a new clip.")]
		[SerializeField] private bool stopBeforeUpdate = true;
		
		private AudioSource audioSource;
		private LocalizationEvents.LanguageChangeSubscription languageChangeSubscription;
		
		private void Awake()
		{
			// watchman init
			Watchman.Watch();
			
			audioSource = GetComponent<AudioSource>();
			
			if (audioSource == null)
			{
				Debug.LogError("[Signalia LocalizedAudioSource] No AudioSource component found on this GameObject!", this);
			}
		}
		
		private void Start()
		{
			if (setOnStart)
			{
				UpdateAudioClip();
			}
			
			// Subscribe to language change events if enabled
			if (updateOnLanguageChange)
			{
				SubscribeToLanguageChange();
			}
		}
		
		private void OnDestroy()
		{
			// Clean up subscription
			languageChangeSubscription?.Dispose();
		}
		
		/// <summary>
		/// Updates the audio clip with the current localization settings.
		/// Call this manually if you need to refresh the audio.
		/// </summary>
		public void UpdateAudioClip()
		{
			if (audioSource == null || string.IsNullOrEmpty(audioKey))
			{
				return;
			}
			
			// Stop current audio if configured
			if (stopBeforeUpdate && audioSource.isPlaying)
			{
				audioSource.Stop();
			}
			
			// Get the localized audio clip
			AudioClip localizedClip = Localization.ReadAudioClip(audioKey);
			
			if (localizedClip != null)
			{
				// Apply audio clip to component
				audioSource.clip = localizedClip;
				
				// Play if configured
				if (playOnUpdate)
				{
					audioSource.Play();
				}
			}
			else if (RuntimeValues.Debugging.IsDebugging)
			{
				Debug.LogWarning($"[Signalia LocalizedAudioSource] No audio clip found for key '{audioKey}' in current language.", this);
			}
		}
		
		/// <summary>
		/// Sets a new audio key and updates the audio clip.
		/// </summary>
		/// <param name="key">The new audio key</param>
		public void SetKey(string key)
		{
			audioKey = key;
			UpdateAudioClip();
		}
		
		/// <summary>
		/// Updates the audio clip and immediately plays it.
		/// </summary>
		public void UpdateAndPlay()
		{
			UpdateAudioClip();
			if (audioSource != null && audioSource.clip != null)
			{
				audioSource.Play();
			}
		}
		
		private void SubscribeToLanguageChange()
		{
			languageChangeSubscription = LocalizationEvents.Subscribe(
				() => UpdateAudioClip(),
				gameObject
			);
		}
		
		/// <summary>
		/// Gets the current audio key.
		/// </summary>
		public string AudioKey => audioKey;
	}
}