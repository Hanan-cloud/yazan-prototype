# Signalia Localization System

A comprehensive localization system for Unity that supports multiple languages, text styles, audio clips, sprites, and other assets. Designed to work seamlessly with the Lingramia editor for managing localization content.

## Table of Contents

- [Overview](#overview)
- [Quick Start](#quick-start)
- [Setup](#setup)
- [Creating LocBooks](#creating-locbooks)
- [Configuring the System](#configuring-the-system)
- [Using Components](#using-components)
- [Code Usage](#code-usage)
- [Text Styles](#text-styles)
  - [Paragraph Styles](#paragraph-styles)
- [Font Factory (TMP Font Creation)](#font-factory-tmp-font-creation)
- [Workflow with Lingramia](#workflow-with-lingramia)
  - [Installing Lingramia](#installing-lingramia)
- [Advanced Features](#advanced-features)
- [Troubleshooting](#troubleshooting)

## Overview

The Signalia Localization System provides:

- **Multi-language Support**: Manage translations for unlimited languages
- **Multiple Asset Types**: Text, audio clips, sprites, and generic Unity objects
- **Automatic Updates**: Components automatically update when language changes
- **Text Styling**: Language-specific fonts and formatting (including RTL and Arabic support)
- **Hybrid Key Mode**: Search by both keys and source strings for easier migration
- **Lingramia Integration**: External editor for managing localization content
- **Event System**: Subscribe to language change events for custom implementations

## Quick Start

1. **Create a LocBook**: Right-click in Project > `Create > Signalia > Localization > LocBook`
2. **Add a .locbook file**: Assign a `.locbook` JSON file to the LocBook asset
3. **Configure in Signalia**: Add the LocBook to `Signalia > Signalia Config` window
4. **Initialize**: Call `SIGS.InitializeLocalization()` at game startup
5. **Use components**: Add `Localized Text` or `Simple Localized Text` components to your UI

## Setup

### 1. Initialize the System

The localization system must be initialized before use. Call this early in your game's startup (e.g., in a GameManager's `Awake()` or `Start()` method):

```csharp
using AHAKuo.Signalia.Framework;

// Initialize using LocBooks configured in Signalia settings
SIGS.InitializeLocalization();
```

### 2. Configure Signalia Settings

Open the Signalia configuration window:
- **Menu**: `Tools > Signalia > Signalia Config`

Configure these settings:

- **LocBooks**: Assign all LocBook assets you want to load
- **Default Starting Language**: Language code to use initially (e.g., `"en"`, `"es"`, `"fr"`)
- **Language Option Save Key**: PlayerPrefs key for saving user's language preference
- **Hybrid Key Mode**: Enable if you want to search by both keys and source strings
- **Text Style Cache**: Assign TextStyle assets for each language

## Creating LocBooks

### Method 1: Create in Unity

1. Right-click in Project window
2. Navigate to `Create > Signalia > Localization > LocBook`
3. Name your LocBook asset (e.g., `MainMenuLocBook`)
4. Assign a `.locbook` JSON file to the **LocBook File** field
5. Click **🚀 Open in Lingramia** to edit content
6. After editing, click **🔄 Update Asset from .locbook File** to sync changes

### Method 2: Create in Lingramia

1. Create a `.locbook` file using the Lingramia editor
2. Import it into Unity as a TextAsset
3. Create a LocBook asset and reference the `.locbook` file

## Configuring the System

### Adding LocBooks to Configuration

1. Open `Tools > Signalia > Signalia Config`
2. In the **LocBook Assets** section, click the **+** button or drag LocBook assets into the array
3. The system will load all assigned LocBooks at initialization

### Multiple LocBooks

You can use multiple LocBooks to organize your content:
- Each LocBook is merged into a single dictionary at runtime
- Duplicate keys from later LocBooks will override earlier ones
- Useful for separating content by feature, scene, or context

## Using Components

### Localized Text

Automatic text localization with style support and overrides.

**Component**: `AHAKuo/Signalia/Game Systems/Localization/Localized Text`

**Features**:
- Automatically updates on language change
- Supports language and text style overrides
- Applies language-specific TextStyles automatically

**Usage**:
1. Add to a GameObject with a `TMP_Text` component
2. Set the **Localization Key** field
3. Optionally override language or text style

**Code API**:
```csharp
LocalizedText localizedText = GetComponent<LocalizedText>();
localizedText.SetKey("welcome_message");
localizedText.SetLanguageOverride("es"); // Use Spanish
localizedText.SetTextStyleOverride(myTextStyle);
localizedText.SetParagraphStyle("Header"); // Use Header paragraph style
localizedText.UpdateText(); // Manual refresh
```

### Simple Localized Text

Minimal configuration version for quick localization.

**Component**: `AHAKuo/Signalia/Game Systems/Localization/Localized Text (Simple)`

**Features**:
- Only requires a key
- Automatically updates on language change
- Uses system defaults for language and styling

**Usage**:
1. Add to a GameObject with a `TMP_Text` component
2. Set the **Key** field

**Code API**:
```csharp
SimpleLocalizedText simpleText = GetComponent<SimpleLocalizedText>();
simpleText.SetKey("start_game");
simpleText.SetParagraphStyle("Body"); // Use Body paragraph style
```

### Localized Image

Automatic sprite localization for UI images.

**Component**: `AHAKuo/Signalia/Game Systems/Localization/Localized Image`

**Features**:
- Updates sprite when language changes
- Supports language-specific sprites

**Usage**:
1. Add to a GameObject with an `Image` component
2. Set the **Sprite Key** field
3. Add sprite entries to your LocBook's image pages

**Code API**:
```csharp
LocalizedImage localizedImage = GetComponent<LocalizedImage>();
localizedImage.SetKey("flag_icon");
localizedImage.UpdateSprite(); // Manual refresh
```

### Localized Audio Source

Automatic audio clip localization for voice-overs and narration.

**Component**: `AHAKuo/Signalia/Game Systems/Localization/Localized Audio Source`

**Features**:
- Updates audio clip when language changes
- Option to play automatically on update
- Stops current playback before switching

**Usage**:
1. Add to a GameObject with an `AudioSource` component
2. Set the **Audio Key** field
3. Configure update and playback settings
4. Add audio entries to your LocBook's audio pages

**Code API**:
```csharp
LocalizedAudioSource localizedAudio = GetComponent<LocalizedAudioSource>();
localizedAudio.SetKey("narration_intro");
localizedAudio.UpdateAudioClip();
localizedAudio.UpdateAndPlay(); // Update and play immediately
```

### Language Switcher

Utility component for switching languages without coding.

**Component**: `AHAKuo/Signalia/Game Systems/Localization/Language Switcher`

**Features**:
- Switch languages on Awake, Start, or manually
- Can be triggered from UnityEvents (e.g., button clicks)
- Option to save language preference

**Usage**:
1. Add to a GameObject
2. Set the **Language Code** (e.g., `"en"`, `"es"`, `"fr"`)
3. Choose **Switch Timing** (None, OnAwake, OnStart)
4. Configure **Save Preference** option

**UnityEvent Example**:
- Attach to a button's `OnClick()` event
- Call `SwitchLanguage()` method

**Code API**:
```csharp
LanguageSwitcher switcher = GetComponent<LanguageSwitcher>();
switcher.SwitchToLanguage("fr"); // Switch to French
switcher.ResetToDefault(); // Reset to default language
```

### Localization Refresher

Manually trigger language refresh events.

**Component**: `AHAKuo/Signalia/Game Systems/Localization/Localization Refresher`

**Features**:
- Force all localized components to update
- Can be triggered from UnityEvents
- Supports delayed refresh

**Usage**:
1. Add to a GameObject
2. Choose **Refresh Timing** (None, OnAwake, OnStart)
3. Call `Refresh()` from code or UnityEvents when needed

**Code API**:
```csharp
LocalizationRefresher refresher = GetComponent<LocalizationRefresher>();
refresher.Refresh(); // Refresh immediately
refresher.RefreshDelayed(0.5f); // Refresh after 0.5 seconds
```

## Code Usage

### SIGS API (Simplified Interface)

The `SIGS` static class provides easy access to localization features:

```csharp
using AHAKuo.Signalia.Framework;

// Get localized string
string text = SIGS.GetLocalizedString("welcome_message");

// Change language
SIGS.ChangeLanguage("es"); // Switch to Spanish

// Check if key exists
bool exists = SIGS.HasLocalizationKey("my_key");

// Get all available language codes
List<string> languages = SIGS.GetAvailableLanguageCodes();

// Trigger language change event
SIGS.TriggerLanguageChange();

// Initialize system
SIGS.InitializeLocalization();
```

### Direct API

For more control, use the direct localization classes:

```csharp
using AHAKuo.Signalia.LocalizationStandalone.Internal;

// Get localized string for current language
string text = Localization.ReadKey("welcome_message");

// Get localized string for specific language
string spanishText = Localization.ReadKey("welcome_message", "es");

// Get audio clip
AudioClip clip = Localization.ReadAudioClip("narration_intro");

// Get sprite
Sprite sprite = Localization.ReadSprite("flag_icon");

// Get generic asset
UnityEngine.Object asset = Localization.ReadAsset("custom_asset");

// Change language
LocalizationRuntime.ChangeLanguage("fr", save: true);

// Get current language
string currentLang = LocalizationRuntime.CurrentLanguageCode;

// Get text style for language
TextStyle style = LocalizationRuntime.GetTextStyle("ar");

// Get text style with paragraph style
TextStyle headerStyle = LocalizationRuntime.GetTextStyle("ar", "Header");
TextStyle bodyStyle = LocalizationRuntime.GetTextStyle("ar", "Body");
```

### Language Change Events

Subscribe to language change events for custom implementations:

```csharp
using AHAKuo.Signalia.Framework;

void Start()
{
    // Subscribe to language change events
    LocalizationEvents.Subscribe(() =>
    {
        Debug.Log("Language changed!");
        UpdateCustomUI();
    }, gameObject);
    
    // Or use the convenience method
    LocalizationEvents.Subscribe(OnLanguageChanged, gameObject);
}

void OnLanguageChanged()
{
    // Your custom update logic
}
```

## Text Styles

TextStyles define language-specific formatting and fonts. You can create multiple styles for the same language using **Paragraph Styles** to differentiate between different text types (headers, body text, captions, etc.).

### Creating a TextStyle

1. Right-click in Project window
2. Navigate to `Create > Signalia > Localization > Text Style`
3. Configure:
   - **Language Code**: The language this style applies to (e.g., `"en"`, `"ar"`)
   - **Paragraph Style**: Optional identifier for different text types (e.g., `"Header"`, `"Body"`, `"Description"`, `"Caption"`). Leave empty for the default style
   - **Font**: TMP Font Asset to use
     - **Recommended**: Use the [Font Factory](#font-factory-tmp-font-creation) to create fonts with proper glyph coverage for non-English languages
   - **Formatting Options**: Bold, Italic, Underline, AllCaps, TitleCase, LowerCase
   - **Enable RTL**: Right-to-left text direction (for Arabic, Hebrew, etc.)
   - **Enable Arabic Formatting**: Arabic character shaping

### Paragraph Styles

Paragraph styles allow you to create multiple text styles for the same language. This is useful when you need different formatting for different types of text.

**Example Use Cases**:
- **Headers**: Large, bold text for titles
- **Body**: Regular text for paragraphs
- **Captions**: Smaller, italic text for descriptions
- **Buttons**: All-caps text for UI buttons

**How It Works**:
1. Create multiple TextStyle assets for the same language code
2. Set different **Paragraph Style** values on each (e.g., `"Header"`, `"Body"`, `"Caption"`)
3. Create one TextStyle with empty paragraph style as the default fallback
4. In your components, specify the paragraph style you want to use

**Component Usage**:
- **Localized Text**: Set the **Paragraph Style** field in the inspector
- **Simple Localized Text**: Set the **Paragraph Style** field in the inspector
- **Code**: Use `LocalizationRuntime.GetTextStyle(languageCode, paragraphStyle)` or `SetLocalizedText(key, paragraphStyle: "Header")`

**Fallback Behavior**:
- If a TextStyle with the exact paragraph style is found, it's used
- If not found, the system falls back to the default style (empty paragraph style)
- If no default style exists, any TextStyle for that language is used

### Adding to Configuration

Add TextStyle assets to the **Text Style Cache** array in Signalia Config. The system will automatically apply the correct style based on the current language and paragraph style.

### Formatting Options

- **Bold**: Makes text bold
- **Italic**: Makes text italic
- **Underline**: Underlines text
- **AllCaps**: Converts text to UPPERCASE
- **TitleCase**: Converts text to Title Case
- **LowerCase**: Converts text to lowercase

**Note**: Case transformations (AllCaps, TitleCase, LowerCase) are mutually exclusive. If multiple are selected, priority is: AllCaps > TitleCase > LowerCase.

### RTL and Arabic Support

For right-to-left languages:

1. Create a TextStyle for the language
2. Enable **Enable RTL**
3. For Arabic, also enable **Enable Arabic Formatting**
4. Assign an appropriate font that supports Arabic characters
   - **Recommended**: Use the [Font Factory](#font-factory-tmp-font-creation) to create TMP fonts with proper Arabic glyph coverage and presentation forms

## Font Factory (TMP Font Creation)

The **Signalia TMP Font Factory** is a specialized tool for creating TextMeshPro font assets from Unity Font assets. It's especially powerful for non-English languages and Arabic, as it properly includes comprehensive glyph sets and Arabic presentation forms.

### Why Use the Font Factory?

- **Comprehensive Character Sets**: Automatically includes Basic Latin, Extended Latin, Arabic, and Arabic Presentation Forms
- **Essential Arabic Forms**: Always includes essential Arabic presentation forms (U+FE80–U+FEFC) for proper character connections
- **Better Glyph Coverage**: Ensures all necessary glyphs are included for proper text rendering in multiple languages
- **Multi-Atlas Support**: Handles large character sets by creating multiple atlas textures
- **Customizable**: Choose which character sets to include and add custom characters

### Opening the Font Factory

**Method 1: Menu**
- Navigate to `Tools > Signalia > Localization > Font Factory`

**Method 2: Context Menu** (Quick Create)
- Select a `.ttf` or `.otf` font file in the Project window
- Right-click > `Create > Signalia > Localization > Create TMP Font Asset`
- This creates a font asset with default settings in the same directory

### Using the Font Factory Window

1. **Font Settings**:
   - **Source Font**: Assign the Unity Font asset (`.ttf` or `.otf`)
   - **Fallback Font**: Optional TMP Font Asset to use for missing glyphs

2. **Atlas Settings**:
   - **Sampling Point Size**: Font size used for sampling (default: 90)
   - **Atlas Padding**: Space between glyphs (default: 9)
   - **Glyph Render Mode**: SDFAA (recommended) for scalable fonts
   - **Atlas Resolution**: Power-of-two resolution (default: 1024x1024)
   - **Enable Multi Atlas**: Allow multiple texture atlases for large character sets

3. **Character Sets**:
   - **Include Basic Latin**: Standard ASCII characters (U+0020–U+007F)
   - **Include Extended Latin**: European characters with diacritics (U+0080–U+024F)
   - **Include Arabic**: Arabic script ranges (U+0600–U+06FF, U+0750–U+077F, U+08A0–U+08FF)
   - **Include Full Arabic Presentation Forms**: Extended presentation forms (U+FB50–U+FDFF, U+FE70–U+FEFF)
   - **Additional Characters**: Custom Unicode characters or ranges (one per line or comma-separated)

**Note**: Essential Arabic presentation forms (U+FE80–U+FEFC) are **always included** automatically, even if "Include Full Arabic Presentation Forms" is disabled. These are required for proper Arabic text rendering with connected characters.

4. **Generate**:
   - Click **Generate TMP Font Asset**
   - Choose save location
   - The font asset is created with all selected character sets

### Example: Creating an Arabic Font

1. Import your Arabic font file (`.ttf` or `.otf`) into Unity
2. Open `Tools > Signalia > Localization > Font Factory`
3. Assign your font to **Source Font**
4. Enable:
   - ✅ Include Basic Latin (for numbers/symbols)
   - ✅ Include Arabic
   - ✅ Include Full Arabic Presentation Forms (optional, for extended support)
5. Optionally assign a fallback font for missing glyphs
6. Click **Generate TMP Font Asset**
7. Use the generated font in a TextStyle asset

### Programmatic Usage

You can also generate fonts programmatically:

```csharp
using AHAKuo.Signalia.LocalizationStandalone.Internal.Editors;
using TMPro;

// Generate with default settings (all character sets included)
Font myFont = // ... your Unity Font
TMP_FontAsset fontAsset = SignaliaTMPFontFactory.GenerateTMPFont(myFont, "Assets/Fonts/MyFont_SDF.asset");

// Use in TextStyle
TextStyle style = ScriptableObject.CreateInstance<TextStyle>();
style.font = fontAsset;
```

### Benefits for Non-English Languages

- **Extended Latin**: Properly includes accented characters (é, ñ, ü, etc.) for European languages
- **Arabic Support**: Comprehensive Arabic character ranges and presentation forms
- **Missing Glyph Detection**: Warns about missing glyphs so you can add fallback fonts
- **Character Range Validation**: Ensures all necessary Unicode ranges are included

### Tips

- **Font Selection**: Use fonts that actually support the languages you need. The factory includes the ranges, but the font must have the glyphs.
- **Fallback Fonts**: Always assign a fallback font (like Arial Unicode MS) to handle missing glyphs gracefully
- **Atlas Size**: If you get atlas overflow warnings, increase the atlas resolution or enable multi-atlas support
- **Performance**: Larger character sets take longer to generate but provide better coverage
- **Essential Forms**: Even if you disable "Full Arabic Presentation Forms", essential forms (U+FE80–U+FEFC) are always included for proper Arabic rendering

## Workflow with Lingramia

### Installing Lingramia

Lingramia can be automatically downloaded and installed directly from Unity using the built-in downloader.

**Opening the Downloader**:
- Navigate to `Tools > Signalia Localization > Download Lingramia`

**Download Process**:
1. Click **Download & Install Lingramia** in the download window
2. The system will automatically:
   - Fetch the latest release information from GitHub
   - Download the appropriate version for your platform (Windows x64/ARM64)
   - Extract and install Lingramia to your local application data folder
3. Once installed, you can use Lingramia directly from the LocBook inspector

**Features**:
- **Automatic Updates**: Re-download to get the latest version
- **Platform Detection**: Automatically downloads the correct version for your system
- **Progress Tracking**: Shows download progress and status messages
- **Installation Location**: Installed to `%LocalAppData%\AHAKuo Creations\Lingramia` on Windows

**Manual Installation**:
If automatic download fails, you can manually download Lingramia from the GitHub repository: `https://github.com/AHAKuo/Lingramia`

### Editing LocBooks

1. **Install Lingramia**: If not already installed, use `Tools > Signalia Localization > Download Lingramia`
2. **Open in Lingramia**: Select your LocBook asset and click **🚀 Open in Lingramia**
3. **Edit Content**: Make changes in Lingramia
   - Add/edit entries
   - Add new languages
   - Use AI translation features
   - Organize entries into pages
4. **Save**: Save in Lingramia (Ctrl+S / Cmd+S)
5. **Update Unity**: Return to Unity and click **🔄 Update Asset from .locbook File**

### LocBook Structure

- **Pages**: Organize entries into logical groups
- **Entries**: Individual localization entries with key, original value, and variants
- **Variants**: Language-specific translations for each entry

### Adding Audio/Image/Asset Entries

Audio, image, and asset entries are managed directly in Unity:

1. Select your LocBook asset
2. In the inspector, expand **Audio Pages**, **Image Pages**, or **Asset Pages**
3. Add new pages and entries
4. Assign Unity assets (AudioClips, Sprites, etc.) to variants

## Advanced Features

### Hybrid Key Mode

When enabled, the system searches by both key and original value. Useful for:

- Migrating projects with hardcoded strings
- Flexible key lookup
- Testing without defining keys first

**Enable**: In Signalia Config, check **Hybrid Key Mode**

**Example**:
```csharp
// If Hybrid Key is enabled, this will find the entry:
string text = SIGS.GetLocalizedString("Welcome to the game");
// Even if the actual key is "welcome_message"
```

### Multiple LocBooks

Organize content across multiple LocBooks:

- **By Feature**: MainMenu, Gameplay, Settings LocBooks
- **By Scene**: Scene1, Scene2 LocBooks
- **By Content Type**: Text, Audio, UI LocBooks

All LocBooks are merged at initialization. Later LocBooks override earlier ones for duplicate keys.

### Custom Asset Localization

Localize any Unity Object type:

1. Add entries to **Asset Pages** in your LocBook
2. Assign Unity objects (ScriptableObjects, Prefabs, etc.) to variants
3. Retrieve using `Localization.ReadAsset(key)`

### Auto-Update LocBooks

Enable **Auto Update LocBooks** in Signalia Config to automatically refresh LocBook assets when their `.locbook` files are modified.

### Runtime Cache Refresh

Enable **Auto Refresh Cache In Runtime** to automatically refresh the localization cache when LocBooks are updated during runtime.

## Troubleshooting

### Text Not Updating

- **Check initialization**: Ensure `SIGS.InitializeLocalization()` is called before using localization
- **Verify LocBook assignment**: Check that LocBooks are assigned in Signalia Config
- **Check key exists**: Use `SIGS.HasLocalizationKey(key)` to verify the key exists
- **Manual refresh**: Try calling `SIGS.TriggerLanguageChange()` or add a `LocalizationRefresher` component

### Missing Translations

- **Check language code**: Ensure the language code matches exactly (case-sensitive)
- **Verify LocBook**: Ensure the LocBook containing the entry is assigned in config
- **Check variant exists**: Verify the entry has a variant for the current language
- **Fallback behavior**: Missing translations return the key or original value

### TextStyle Not Applying

- **Check TextStyle assignment**: Verify TextStyle is added to TextStyle Cache in config
- **Verify language code**: Ensure TextStyle's language code matches the current language exactly
- **Check paragraph style**: If using paragraph styles, ensure a TextStyle with matching paragraph style exists, or create a default style (empty paragraph style)
- **Check font assignment**: Ensure a font is assigned to the TextStyle
- **Manual application**: Try calling `textStyle.ApplyToText(textComponent)` directly

### Missing Font Glyphs / Broken Characters

- **Use Font Factory**: Create TMP fonts using the [Font Factory](#font-factory-tmp-font-creation) to ensure proper glyph coverage
- **Check character sets**: Verify the font includes the necessary character ranges (Latin, Arabic, etc.)
- **Missing glyphs**: The Font Factory will warn about missing glyphs - assign a fallback font to handle them
- **Arabic characters not connecting**: Ensure essential Arabic presentation forms (U+FE80–U+FEFC) are included - Font Factory includes these automatically
- **Extended Latin missing**: Enable "Include Extended Latin" in Font Factory for accented characters (é, ñ, ü, etc.)
- **Font doesn't support language**: The font file itself must contain the glyphs - Font Factory only includes the ranges, not the glyphs themselves

### Lingramia Integration Issues

- **File association**: Ensure `.locbook` files are associated with Lingramia
- **File reference**: Verify the LocBook asset has a valid `.locbook` file reference
- **File permissions**: Ensure `.locbook` files are writable
- **Manual update**: Use "🔄 Update Asset from .locbook File" button if auto-update fails

### Audio/Image Not Loading

- **Check page type**: Ensure entries are in Audio Pages or Image Pages, not text pages
- **Verify asset assignment**: Ensure Unity assets are assigned to variants
- **Check key**: Verify the key matches exactly (case-sensitive)
- **Language variant**: Ensure a variant exists for the current language

### Performance Issues

- **Initialize early**: Initialize localization before loading scenes
- **Limit LocBooks**: Use fewer LocBooks with more entries rather than many small LocBooks
- **Disable auto-refresh**: If not needed, disable auto-refresh features
- **Cache styles**: TextStyle lookups are cached, but many languages may impact performance

## Additional Resources

- **Signalia Config**: `Tools > Signalia > Signalia Config`
- **Localization Refactor Summary**: See `LOCALIZATION_REFACTOR_SUMMARY.md` for migration notes
- **Framework Access**: Use `SIGS` class for simplified API access
- **Framework Documentation**: See Signalia Framework documentation for advanced usage

---

**Note**: This system integrates with the Signalia framework. Ensure the framework is properly initialized before using localization features.

