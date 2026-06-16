using UnityEngine;
using UnityEngine.UI;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

namespace AHAKuo.Signalia.LocalizationStandalone.Internal
{
	/// <summary>
	/// Helper component that automatically sets localized sprites on a UI Image component.
	/// Attach this to any GameObject with an Image component to enable automatic sprite localization.
	/// </summary>
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("Signalia Localization/Localized Image")]
	[Icon("Assets/AHAKuo Creations/Lingramia/Framework/Graphics/localization_image_icon.png")]
	public class LocalizedImage : MonoBehaviour
	{
		//[Header("Localization Settings")]
		[Tooltip("The localization key for the sprite. Must match a sprite entry in your LocBook.")]
		[SerializeField] private string spriteKey;
		
		//[Header("Update Settings")]
		[Tooltip("Automatically update the sprite when the language changes.")]
		[SerializeField] private bool updateOnLanguageChange = true;
		
		[Tooltip("Set the localized sprite on Start.")]
		[SerializeField] private bool setOnStart = true;
		
		private Image imageComponent;
		private LocalizationEvents.LanguageChangeSubscription languageChangeSubscription;
		
		private void Awake()
		{
			// watchman init
			Watchman.Watch();
			
			imageComponent = GetComponent<Image>();
			
			if (imageComponent == null)
			{
				Debug.LogError("[Signalia LocalizedImage] No Image component found on this GameObject!", this);
			}
		}
		
		private void Start()
		{
			if (setOnStart)
			{
				UpdateSprite();
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
		/// Updates the sprite with the current localization settings.
		/// Call this manually if you need to refresh the sprite.
		/// </summary>
		public void UpdateSprite()
		{
			if (imageComponent == null || string.IsNullOrEmpty(spriteKey))
			{
				return;
			}
			
			// Get the localized sprite
			Sprite localizedSprite = Localization.ReadSprite(spriteKey);
			
			if (localizedSprite != null)
			{
				// Apply sprite to component
				imageComponent.sprite = localizedSprite;
			}
			else if (RuntimeValues.Debugging.IsDebugging)
			{
				Debug.LogWarning($"[Signalia LocalizedImage] No sprite found for key '{spriteKey}' in current language.", this);
			}
		}
		
		/// <summary>
		/// Sets a new sprite key and updates the sprite.
		/// </summary>
		/// <param name="key">The new sprite key</param>
		public void SetKey(string key)
		{
			spriteKey = key;
			UpdateSprite();
		}
		
		private void SubscribeToLanguageChange()
		{
			languageChangeSubscription = LocalizationEvents.Subscribe(
				() => UpdateSprite(),
				gameObject
			);
		}
		
		/// <summary>
		/// Gets the current sprite key.
		/// </summary>
		public string SpriteKey => spriteKey;
	}
}