<style>
body {
    direction: rtl;
    text-align: right;
}

/* Keep code blocks LTR for proper readability */
code, pre {
    direction: ltr;
    text-align: left;
}

pre code {
    direction: ltr;
    text-align: left;
}
</style>

<div dir="rtl">

# نظام الترجمة Signalia

نظام ترجمة شامل لـ Unity يدعم لغات متعددة وأنماط نصية ومقاطع صوتية وsprites وأصول أخرى. مصمم للعمل بسلاسة مع محرر Lingramia لإدارة محتوى الترجمة.

## جدول المحتويات

- [نظرة عامة](#نظرة-عامة)
- [بدء سريع](#بدء-سريع)
- [الإعداد](#الإعداد)
- [إنشاء LocBooks](#إنشاء-locbooks)
- [تكوين النظام](#تكوين-النظام)
- [استخدام المكونات](#استخدام-المكونات)
- [استخدام الكود](#استخدام-الكود)
- [أنماط النص](#أنماط-النص)
  - [أنماط الفقرة](#أنماط-الفقرة)
- [مصنع الخطوط (إنشاء خطوط TMP)](#مصنع-الخطوط-إنشاء-خطوط-tmp)
- [سير العمل مع Lingramia](#سير-العمل-مع-lingramia)
  - [تثبيت Lingramia](#تثبيت-lingramia)
- [ميزات متقدمة](#ميزات-متقدمة)
- [استكشاف الأخطاء وإصلاحها](#استكشاف-الأخطاء-وإصلاحها)

## نظرة عامة

يوفر نظام ترجمة Signalia:

- **دعم لغات متعددة**: إدارة الترجمات لعدد غير محدود من اللغات
- **أنواع أصول متعددة**: نص ومقاطع صوتية وsprites وكائنات Unity عامة
- **تحديثات تلقائية**: تحديث المكونات تلقائياً عند تغيير اللغة
- **تنسيق النص**: خطوط وتنسيقات خاصة باللغة (بما في ذلك دعم RTL والعربية)
- **وضع المفاتيح المختلط**: البحث عن طريق المفاتيح والسلاسل النصية المصدرية لتسهيل الترحيل
- **تكامل Lingramia**: محرر خارجي لإدارة محتوى الترجمة
- **نظام الأحداث**: الاشتراك في أحداث تغيير اللغة للتطبيقات المخصصة

## بدء سريع

1. **إنشاء LocBook**: انقر بزر الماوس الأيمن في Project > `Create > Signalia > Localization > LocBook`
2. **إضافة ملف .locbook**: قم بتعيين ملف JSON `.locbook` إلى أصل LocBook
3. **التكوين في Signalia**: أضف LocBook إلى نافذة `Signalia > Signalia Config`
4. **التهيئة**: استدعِ `SIGS.InitializeLocalization()` عند بدء اللعبة
5. **استخدام المكونات**: أضف مكونات `Localized Text` أو `Simple Localized Text` إلى واجهة المستخدم

## الإعداد

### 1. تهيئة النظام

يجب تهيئة نظام الترجمة قبل الاستخدام. قم باستدعاء هذا في وقت مبكر من بدء اللعبة (على سبيل المثال، في طريقة `Awake()` أو `Start()` في GameManager):

```csharp
using AHAKuo.Signalia.Framework;

// التهيئة باستخدام LocBooks المكونة في إعدادات Signalia
SIGS.InitializeLocalization();
```

### 2. تكوين إعدادات Signalia

افتح نافذة تكوين Signalia:
- **القائمة**: `Tools > Signalia > Signalia Config`

قم بتكوين هذه الإعدادات:

- **LocBooks**: قم بتعيين جميع أصول LocBook التي تريد تحميلها
- **اللغة الافتراضية للبدء**: رمز اللغة للاستخدام في البداية (على سبيل المثال، `"en"`، `"es"`، `"fr"`)
- **مفتاح حفظ خيار اللغة**: مفتاح PlayerPrefs لحفظ تفضيل لغة المستخدم
- **وضع المفاتيح المختلط**: تفعيل إذا كنت تريد البحث عن طريق المفاتيح والسلاسل النصية المصدرية
- **ذاكرة أنماط النص**: قم بتعيين أصول TextStyle لكل لغة

## إنشاء LocBooks

### الطريقة 1: الإنشاء في Unity

1. انقر بزر الماوس الأيمن في نافذة Project
2. انتقل إلى `Create > Signalia > Localization > LocBook`
3. قم بتسمية أصل LocBook (على سبيل المثال، `MainMenuLocBook`)
4. قم بتعيين ملف JSON `.locbook` إلى حقل **LocBook File**
5. انقر على **🚀 Open in Lingramia** لتحرير المحتوى
6. بعد التحرير، انقر على **🔄 Update Asset from .locbook File** لمزامنة التغييرات

### الطريقة 2: الإنشاء في Lingramia

1. أنشئ ملف `.locbook` باستخدام محرر Lingramia
2. قم باستيراده إلى Unity كـ TextAsset
3. أنشئ أصل LocBook واشِر إلى ملف `.locbook`

## تكوين النظام

### إضافة LocBooks إلى التكوين

1. افتح `Tools > Signalia > Signalia Config`
2. في قسم **LocBook Assets**، انقر على زر **+** أو اسحب أصول LocBook إلى المصفوفة
3. سيقوم النظام بتحميل جميع LocBooks المعينة عند التهيئة

### LocBooks متعددة

يمكنك استخدام LocBooks متعددة لتنظيم المحتوى الخاص بك:
- يتم دمج كل LocBook في قاموس واحد في وقت التشغيل
- المفاتيح المكررة من LocBooks اللاحقة ستعيد كتابة السابقة
- مفيد لفصل المحتوى حسب الميزة أو المشهد أو السياق

## استخدام المكونات

### Localized Text

ترجمة نصية تلقائية مع دعم الأنماط والتجاوزات.

**المكون**: `AHAKuo/Signalia/Game Systems/Localization/Localized Text`

**الميزات**:
- التحديث التلقائي عند تغيير اللغة
- دعم تجاوزات اللغة ونمط النص
- تطبيق TextStyles الخاصة باللغة تلقائياً

**الاستخدام**:
1. أضف إلى GameObject مع مكون `TMP_Text`
2. اضبط حقل **Localization Key**
3. اختيارياً، قم بتجاوز اللغة أو نمط النص

**واجهة برمجة الكود**:
```csharp
LocalizedText localizedText = GetComponent<LocalizedText>();
localizedText.SetKey("welcome_message");
localizedText.SetLanguageOverride("es"); // استخدام الإسبانية
localizedText.SetTextStyleOverride(myTextStyle);
localizedText.SetParagraphStyle("Header"); // استخدام نمط فقرة Header
localizedText.UpdateText(); // تحديث يدوي
```

### Simple Localized Text

إصدار تكوين بسيط للترجمة السريعة.

**المكون**: `AHAKuo/Signalia/Game Systems/Localization/Localized Text (Simple)`

**الميزات**:
- يتطلب مفتاحاً فقط
- التحديث التلقائي عند تغيير اللغة
- يستخدم الإعدادات الافتراضية للنظام للغة والتنسيق

**الاستخدام**:
1. أضف إلى GameObject مع مكون `TMP_Text`
2. اضبط حقل **Key**

**واجهة برمجة الكود**:
```csharp
SimpleLocalizedText simpleText = GetComponent<SimpleLocalizedText>();
simpleText.SetKey("start_game");
simpleText.SetParagraphStyle("Body"); // استخدام نمط فقرة Body
```

### Localized Image

ترجمة sprite تلقائية لصور واجهة المستخدم.

**المكون**: `AHAKuo/Signalia/Game Systems/Localization/Localized Image`

**الميزات**:
- تحديث sprite عند تغيير اللغة
- دعم sprites خاصة باللغة

**الاستخدام**:
1. أضف إلى GameObject مع مكون `Image`
2. اضبط حقل **Sprite Key**
3. أضف إدخالات sprite إلى صفحات الصور في LocBook الخاص بك

**واجهة برمجة الكود**:
```csharp
LocalizedImage localizedImage = GetComponent<LocalizedImage>();
localizedImage.SetKey("flag_icon");
localizedImage.UpdateSprite(); // تحديث يدوي
```

### Localized Audio Source

ترجمة مقطع صوتي تلقائية للتعليق الصوتي والسرد.

**المكون**: `AHAKuo/Signalia/Game Systems/Localization/Localized Audio Source`

**الميزات**:
- تحديث مقطع الصوت عند تغيير اللغة
- خيار للتشغيل التلقائي عند التحديث
- إيقاف التشغيل الحالي قبل التبديل

**الاستخدام**:
1. أضف إلى GameObject مع مكون `AudioSource`
2. اضبط حقل **Audio Key**
3. قم بتكوين إعدادات التحديث والتشغيل
4. أضف إدخالات صوتية إلى صفحات الصوت في LocBook الخاص بك

**واجهة برمجة الكود**:
```csharp
LocalizedAudioSource localizedAudio = GetComponent<LocalizedAudioSource>();
localizedAudio.SetKey("narration_intro");
localizedAudio.UpdateAudioClip();
localizedAudio.UpdateAndPlay(); // تحديث وتشغيل فوري
```

### Language Switcher

مكون مساعد لتبديل اللغات بدون برمجة.

**المكون**: `AHAKuo/Signalia/Game Systems/Localization/Language Switcher`

**الميزات**:
- تبديل اللغات على Awake أو Start أو يدوياً
- يمكن تشغيله من UnityEvents (على سبيل المثال، نقرات الأزرار)
- خيار لحفظ تفضيل اللغة

**الاستخدام**:
1. أضف إلى GameObject
2. اضبط **Language Code** (على سبيل المثال، `"en"`، `"es"`، `"fr"`)
3. اختر **Switch Timing** (None، OnAwake، OnStart)
4. قم بتكوين خيار **Save Preference**

**مثال UnityEvent**:
- اربط بحدث `OnClick()` للزر
- استدعِ طريقة `SwitchLanguage()`

**واجهة برمجة الكود**:
```csharp
LanguageSwitcher switcher = GetComponent<LanguageSwitcher>();
switcher.SwitchToLanguage("fr"); // التبديل إلى الفرنسية
switcher.ResetToDefault(); // إعادة تعيين إلى اللغة الافتراضية
```

### Localization Refresher

تشغيل أحداث تحديث اللغة يدوياً.

**المكون**: `AHAKuo/Signalia/Game Systems/Localization/Localization Refresher`

**الميزات**:
- إجبار جميع المكونات المترجمة على التحديث
- يمكن تشغيله من UnityEvents
- دعم تحديث متأخر

**الاستخدام**:
1. أضف إلى GameObject
2. اختر **Refresh Timing** (None، OnAwake، OnStart)
3. استدعِ `Refresh()` من الكود أو UnityEvents عند الحاجة

**واجهة برمجة الكود**:
```csharp
LocalizationRefresher refresher = GetComponent<LocalizationRefresher>();
refresher.Refresh(); // تحديث فوري
refresher.RefreshDelayed(0.5f); // تحديث بعد 0.5 ثانية
```

## استخدام الكود

### واجهة SIGS API (الواجهة المبسطة)

توفر الفئة الثابتة `SIGS` سهولة الوصول إلى ميزات الترجمة:

```csharp
using AHAKuo.Signalia.Framework;

// الحصول على سلسلة مترجمة
string text = SIGS.GetLocalizedString("welcome_message");

// تغيير اللغة
SIGS.ChangeLanguage("es"); // التبديل إلى الإسبانية

// التحقق من وجود المفتاح
bool exists = SIGS.HasLocalizationKey("my_key");

// الحصول على جميع رموز اللغات المتاحة
List<string> languages = SIGS.GetAvailableLanguageCodes();

// تشغيل حدث تغيير اللغة
SIGS.TriggerLanguageChange();

// تهيئة النظام
SIGS.InitializeLocalization();
```

### واجهة API المباشرة

للمزيد من التحكم، استخدم فئات الترجمة المباشرة:

```csharp
using AHAKuo.Signalia.LocalizationStandalone.Internal;

// الحصول على سلسلة مترجمة للغة الحالية
string text = Localization.ReadKey("welcome_message");

// الحصول على سلسلة مترجمة للغة محددة
string spanishText = Localization.ReadKey("welcome_message", "es");

// الحصول على مقطع صوتي
AudioClip clip = Localization.ReadAudioClip("narration_intro");

// الحصول على sprite
Sprite sprite = Localization.ReadSprite("flag_icon");

// الحصول على أصل عام
UnityEngine.Object asset = Localization.ReadAsset("custom_asset");

// تغيير اللغة
LocalizationRuntime.ChangeLanguage("fr", save: true);

// الحصول على اللغة الحالية
string currentLang = LocalizationRuntime.CurrentLanguageCode;

// الحصول على نمط النص للغة
TextStyle style = LocalizationRuntime.GetTextStyle("ar");

// الحصول على نمط النص مع نمط الفقرة
TextStyle headerStyle = LocalizationRuntime.GetTextStyle("ar", "Header");
TextStyle bodyStyle = LocalizationRuntime.GetTextStyle("ar", "Body");
```

### أحداث تغيير اللغة

الاشتراك في أحداث تغيير اللغة للتطبيقات المخصصة:

```csharp
using AHAKuo.Signalia.Framework;

void Start()
{
    // الاشتراك في أحداث تغيير اللغة
    LocalizationEvents.Subscribe(() =>
    {
        Debug.Log("Language changed!");
        UpdateCustomUI();
    }, gameObject);
    
    // أو استخدم الطريقة المباشرة
    LocalizationEvents.Subscribe(OnLanguageChanged, gameObject);
}

void OnLanguageChanged()
{
    // منطق التحديث المخصص الخاص بك
}
```

## أنماط النص

TextStyles تعرّف التنسيق والخطوط الخاصة باللغة. يمكنك إنشاء أنماط متعددة للغة نفسها باستخدام **أنماط الفقرة** للتمييز بين أنواع النص المختلفة (العناوين، نص الجسم، التسميات التوضيحية، إلخ).

### إنشاء TextStyle

1. انقر بزر الماوس الأيمن في نافذة Project
2. انتقل إلى `Create > Signalia > Localization > Text Style`
3. قم بالتكوين:
   - **Language Code**: اللغة التي ينطبق عليها هذا النمط (على سبيل المثال، `"en"`، `"ar"`)
   - **Paragraph Style**: معرف اختياري لأنواع النص المختلفة (على سبيل المثال، `"Header"`، `"Body"`، `"Description"`، `"Caption"`). اتركه فارغاً للنمط الافتراضي
   - **Font**: أصل TMP Font للاستخدام
     - **موصى به**: استخدم [مصنع الخطوط](#مصنع-الخطوط-إنشاء-خطوط-tmp) لإنشاء خطوط مع تغطية glyphs مناسبة للغات غير الإنجليزية
   - **خيارات التنسيق**: Bold، Italic، Underline، AllCaps، TitleCase، LowerCase
   - **تفعيل RTL**: اتجاه النص من اليمين إلى اليسار (للعربية والعبرية، إلخ)
   - **تفعيل تنسيق العربية**: تشكيل الأحرف العربية

### أنماط الفقرة

أنماط الفقرة تسمح لك بإنشاء أنماط نص متعددة للغة نفسها. هذا مفيد عندما تحتاج إلى تنسيقات مختلفة لأنواع مختلفة من النص.

**أمثلة الاستخدام**:
- **العناوين**: نص كبير وعريض للعناوين
- **الجسم**: نص عادي للفقرات
- **التسميات التوضيحية**: نص أصغر ومائل للوصف
- **الأزرار**: نص بأحرف كبيرة لأزرار واجهة المستخدم

**كيف يعمل**:
1. أنشئ أصول TextStyle متعددة لنفس رمز اللغة
2. اضبط قيم **Paragraph Style** مختلفة على كل منها (على سبيل المثال، `"Header"`، `"Body"`، `"Caption"`)
3. أنشئ TextStyle واحد بنمط فقرة فارغ كاحتياطي افتراضي
4. في مكوناتك، حدد نمط الفقرة الذي تريد استخدامه

**استخدام المكونات**:
- **Localized Text**: اضبط حقل **Paragraph Style** في Inspector
- **Simple Localized Text**: اضبط حقل **Paragraph Style** في Inspector
- **الكود**: استخدم `LocalizationRuntime.GetTextStyle(languageCode, paragraphStyle)` أو `SetLocalizedText(key, paragraphStyle: "Header")`

**سلوك الاحتياطي**:
- إذا تم العثور على TextStyle بنمط الفقرة المطابق تماماً، يتم استخدامه
- إذا لم يتم العثور عليه، يعود النظام إلى النمط الافتراضي (نمط فقرة فارغ)
- إذا لم يكن هناك نمط افتراضي، يتم استخدام أي TextStyle لتلك اللغة

### الإضافة إلى التكوين

أضف أصول TextStyle إلى مصفوفة **Text Style Cache** في Signalia Config. سيقوم النظام بتطبيق النمط الصحيح تلقائياً بناءً على اللغة الحالية ونمط الفقرة.

### خيارات التنسيق

- **Bold**: يجعل النص عريضاً
- **Italic**: يجعل النص مائلاً
- **Underline**: يضع خطاً تحت النص
- **AllCaps**: يحول النص إلى أحرف كبيرة
- **TitleCase**: يحول النص إلى حالة العنوان
- **LowerCase**: يحول النص إلى أحرف صغيرة

**ملاحظة**: تحويلات الحالة (AllCaps، TitleCase، LowerCase) متبادلة الاستبعاد. إذا تم اختيار أكثر من واحد، الأولوية هي: AllCaps > TitleCase > LowerCase.

### دعم RTL والعربية

للغات من اليمين إلى اليسار:

1. أنشئ TextStyle للغة
2. فعّل **تفعيل RTL**
3. بالنسبة للعربية، فعّل أيضاً **تفعيل تنسيق العربية**
4. قم بتعيين خط مناسب يدعم الأحرف العربية
   - **موصى به**: استخدم [مصنع الخطوط](#مصنع-الخطوط-إنشاء-خطوط-tmp) لإنشاء خطوط TMP مع تغطية glyphs عربية مناسبة وأشكال العرض

## مصنع الخطوط (إنشاء خطوط TMP)

**مصنع خطوط TMP في Signalia** هو أداة متخصصة لإنشاء أصول خطوط TextMeshPro من أصول خطوط Unity. إنه قوي بشكل خاص للغات غير الإنجليزية والعربية، لأنه يتضمن بشكل صحيح مجموعات glyphs شاملة وأشكال العرض العربية.

### لماذا استخدام مصنع الخطوط؟

- **مجموعات أحرف شاملة**: يتضمن تلقائياً Basic Latin وExtended Latin والعربية وأشكال العرض العربية
- **أشكال العربية الأساسية**: يتضمن دائماً أشكال العرض العربية الأساسية (U+FE80–U+FEFC) للاتصالات الصحيحة للأحرف
- **تغطية Glyphs أفضل**: يضمن تضمين جميع glyphs اللازمة لعرض النص بشكل صحيح في عدة لغات
- **دعم Multi-Atlas**: يتعامل مع مجموعات الأحرف الكبيرة عن طريق إنشاء نصوص atlas متعددة
- **قابل للتخصيص**: اختر مجموعات الأحرف المراد تضمينها وأضف أحرفاً مخصصة

### فتح مصنع الخطوط

**الطريقة 1: القائمة**
- انتقل إلى `Tools > Signalia > Localization > Font Factory`

**الطريقة 2: قائمة السياق** (إنشاء سريع)
- حدد ملف خط `.ttf` أو `.otf` في نافذة Project
- انقر بزر الماوس الأيمن > `Create > Signalia > Localization > Create TMP Font Asset`
- هذا ينشئ أصل خط بإعدادات افتراضية في نفس المجلد

### استخدام نافذة مصنع الخطوط

1. **إعدادات الخط**:
   - **Source Font**: قم بتعيين أصل خط Unity (`.ttf` أو `.otf`)
   - **Fallback Font**: أصل TMP Font اختياري للاستخدام للـ glyphs المفقودة

2. **إعدادات Atlas**:
   - **Sampling Point Size**: حجم الخط المستخدم للعينة (افتراضي: 90)
   - **Atlas Padding**: المسافة بين glyphs (افتراضي: 9)
   - **Glyph Render Mode**: SDFAA (موصى به) للخطوط القابلة للتوسيع
   - **Atlas Resolution**: دقة قوة اثنين (افتراضي: 1024x1024)
   - **تفعيل Multi Atlas**: السماح بنصوص atlas متعددة لمجموعات الأحرف الكبيرة

3. **مجموعات الأحرف**:
   - **تضمين Basic Latin**: الأحرف القياسية ASCII (U+0020–U+007F)
   - **تضمين Extended Latin**: الأحرف الأوروبية مع علامات التشكيل (U+0080–U+024F)
   - **تضمين العربية**: نطاقات النص العربي (U+0600–U+06FF، U+0750–U+077F، U+08A0–U+08FF)
   - **تضمين أشكال العرض العربية الكاملة**: أشكال العرض الممتدة (U+FB50–U+FDFF، U+FE70–U+FEFF)
   - **أحرف إضافية**: أحرف Unicode مخصصة أو نطاقات (واحد لكل سطر أو مفصولة بفواصل)

**ملاحظة**: أشكال العرض العربية الأساسية (U+FE80–U+FEFC) يتم **تضمينها دائماً** تلقائياً، حتى إذا تم تعطيل "تضمين أشكال العرض العربية الكاملة". هذه مطلوبة لعرض النص العربي بشكل صحيح مع الأحرف المتصلة.

4. **التوليد**:
   - انقر على **Generate TMP Font Asset**
   - اختر موقع الحفظ
   - يتم إنشاء أصل الخط مع جميع مجموعات الأحرف المحددة

### مثال: إنشاء خط عربي

1. قم باستيراد ملف الخط العربي (`.ttf` أو `.otf`) إلى Unity
2. افتح `Tools > Signalia > Localization > Font Factory`
3. قم بتعيين خطك إلى **Source Font**
4. فعّل:
   - ✅ تضمين Basic Latin (للأرقام/الرموز)
   - ✅ تضمين العربية
   - ✅ تضمين أشكال العرض العربية الكاملة (اختياري، للدعم الممتد)
5. اختيارياً، قم بتعيين خط احتياطي للـ glyphs المفقودة
6. انقر على **Generate TMP Font Asset**
7. استخدم الخط المُنشأ في أصل TextStyle

### الاستخدام البرمجي

يمكنك أيضاً توليد الخطوط برمجياً:

```csharp
using AHAKuo.Signalia.LocalizationStandalone.Internal.Editors;
using TMPro;

// التوليد بإعدادات افتراضية (جميع مجموعات الأحرف متضمنة)
Font myFont = // ... خط Unity الخاص بك
TMP_FontAsset fontAsset = SignaliaTMPFontFactory.GenerateTMPFont(myFont, "Assets/Fonts/MyFont_SDF.asset");

// الاستخدام في TextStyle
TextStyle style = ScriptableObject.CreateInstance<TextStyle>();
style.font = fontAsset;
```

### فوائد للغات غير الإنجليزية

- **Extended Latin**: يتضمن بشكل صحيح الأحرف المشكّلة (é، ñ، ü، إلخ) للغات الأوروبية
- **دعم العربية**: نطاقات أحرف عربية شاملة وأشكال العرض
- **كشف Glyphs المفقودة**: يحذر من glyphs المفقودة حتى تتمكن من إضافة خطوط احتياطية
- **التحقق من نطاق الأحرف**: يضمن تضمين جميع نطاقات Unicode اللازمة

### نصائح

- **اختيار الخط**: استخدم الخطوط التي تدعم فعلياً اللغات التي تحتاجها. المصنع يتضمن النطاقات، لكن الخط يجب أن يحتوي على glyphs.
- **الخطوط الاحتياطية**: قم دائماً بتعيين خط احتياطي (مثل Arial Unicode MS) للتعامل مع glyphs المفقودة بشكل سلس
- **حجم Atlas**: إذا حصلت على تحذيرات تجاوز atlas، قم بزيادة دقة atlas أو تفعيل دعم multi-atlas
- **الأداء**: مجموعات الأحرف الأكبر تستغرق وقتاً أطول للتوليد لكنها توفر تغطية أفضل
- **الأشكال الأساسية**: حتى إذا قمت بتعطيل "أشكال العرض العربية الكاملة"، الأشكال الأساسية (U+FE80–U+FEFC) يتم تضمينها دائماً لعرض عربي صحيح

## سير العمل مع Lingramia

### تثبيت Lingramia

يمكن تنزيل وتثبيت Lingramia تلقائياً مباشرة من Unity باستخدام أداة التنزيل المدمجة.

**فتح أداة التنزيل**:
- انتقل إلى `Tools > Signalia Localization > Download Lingramia`

**عملية التنزيل**:
1. انقر على **Download & Install Lingramia** في نافذة التنزيل
2. سيقوم النظام تلقائياً بـ:
   - جلب معلومات أحدث إصدار من GitHub
   - تنزيل الإصدار المناسب لمنصتك (Windows x64/ARM64)
   - استخراج وتثبيت Lingramia في مجلد بيانات التطبيق المحلي
3. بعد التثبيت، يمكنك استخدام Lingramia مباشرة من محرر LocBook

**الميزات**:
- **تحديثات تلقائية**: قم بإعادة التنزيل للحصول على أحدث إصدار
- **كشف المنصة**: تنزيل الإصدار الصحيح لنظامك تلقائياً
- **تتبع التقدم**: عرض تقدم التنزيل ورسائل الحالة
- **موقع التثبيت**: يتم التثبيت في `%LocalAppData%\AHAKuo Creations\Lingramia` على Windows

**التثبيت اليدوي**:
إذا فشل التنزيل التلقائي، يمكنك تنزيل Lingramia يدوياً من مستودع GitHub: `https://github.com/AHAKuo/Lingramia`

### تحرير LocBooks

1. **تثبيت Lingramia**: إذا لم يكن مثبتاً بالفعل، استخدم `Tools > Signalia Localization > Download Lingramia`
2. **فتح في Lingramia**: حدد أصل LocBook الخاص بك وانقر على **🚀 Open in Lingramia**
3. **تحرير المحتوى**: قم بإجراء تغييرات في Lingramia
   - إضافة/تحرير الإدخالات
   - إضافة لغات جديدة
   - استخدام ميزات الترجمة بالذكاء الاصطناعي
   - تنظيم الإدخالات في صفحات
4. **الحفظ**: احفظ في Lingramia (Ctrl+S / Cmd+S)
5. **تحديث Unity**: ارجع إلى Unity وانقر على **🔄 Update Asset from .locbook File**

### بنية LocBook

- **الصفحات**: تنظيم الإدخالات في مجموعات منطقية
- **الإدخالات**: إدخالات ترجمة فردية مع مفتاح وقيمة أصلية ومتغيرات
- **المتغيرات**: ترجمات خاصة باللغة لكل إدخال

### إضافة إدخالات الصوت/الصورة/الأصل

يتم إدارة إدخالات الصوت والصورة والأصل مباشرة في Unity:

1. حدد أصل LocBook الخاص بك
2. في Inspector، قم بتوسيع **Audio Pages** أو **Image Pages** أو **Asset Pages**
3. أضف صفحات وإدخالات جديدة
4. قم بتعيين أصول Unity (AudioClips، Sprites، إلخ) إلى المتغيرات

## ميزات متقدمة

### وضع المفاتيح المختلط

عند التفعيل، يبحث النظام عن طريق المفتاح والقيمة الأصلية. مفيد لـ:

- ترحيل المشاريع مع سلاسل نصية مدمجة
- بحث مرن عن المفاتيح
- الاختبار بدون تعريف المفاتيح أولاً

**التفعيل**: في Signalia Config، حدد **Hybrid Key Mode**

**مثال**:
```csharp
// إذا تم تفعيل Hybrid Key، سيجد هذا الإدخال:
string text = SIGS.GetLocalizedString("Welcome to the game");
// حتى لو كان المفتاح الفعلي هو "welcome_message"
```

### LocBooks متعددة

تنظيم المحتوى عبر LocBooks متعددة:

- **حسب الميزة**: MainMenu، Gameplay، Settings LocBooks
- **حسب المشهد**: Scene1، Scene2 LocBooks
- **حسب نوع المحتوى**: Text، Audio، UI LocBooks

يتم دمج جميع LocBooks عند التهيئة. LocBooks اللاحقة تعيد كتابة السابقة للمفاتيح المكررة.

### ترجمة الأصل المخصص

قم بترجمة أي نوع Unity Object:

1. أضف إدخالات إلى **Asset Pages** في LocBook الخاص بك
2. قم بتعيين كائنات Unity (ScriptableObjects، Prefabs، إلخ) إلى المتغيرات
3. استرجع باستخدام `Localization.ReadAsset(key)`

### تحديث LocBooks تلقائياً

فعّل **Auto Update LocBooks** في Signalia Config لتحديث أصول LocBook تلقائياً عند تعديل ملفات `.locbook` الخاصة بها.

### تحديث ذاكرة التخزين المؤقت في وقت التشغيل

فعّل **Auto Refresh Cache In Runtime** لتحديث ذاكرة التخزين المؤقت للترجمة تلقائياً عند تحديث LocBooks أثناء وقت التشغيل.

## استكشاف الأخطاء وإصلاحها

### النص لا يتحدث

- **تحقق من التهيئة**: تأكد من استدعاء `SIGS.InitializeLocalization()` قبل استخدام الترجمة
- **التحقق من تعيين LocBook**: تأكد من تعيين LocBooks في Signalia Config
- **التحقق من وجود المفتاح**: استخدم `SIGS.HasLocalizationKey(key)` للتحقق من وجود المفتاح
- **تحديث يدوي**: جرّب استدعاء `SIGS.TriggerLanguageChange()` أو أضف مكون `LocalizationRefresher`

### الترجمات المفقودة

- **التحقق من رمز اللغة**: تأكد من تطابق رمز اللغة تماماً (حساس لحالة الأحرف)
- **التحقق من LocBook**: تأكد من تعيين LocBook الذي يحتوي على الإدخال في التكوين
- **التحقق من وجود المتغير**: تأكد من وجود متغير للإدخال للغة الحالية
- **سلوك الاحتياطي**: الترجمات المفقودة ترجع المفتاح أو القيمة الأصلية

### TextStyle لا يُطبق

- **التحقق من تعيين TextStyle**: تأكد من إضافة TextStyle إلى TextStyle Cache في التكوين
- **التحقق من رمز اللغة**: تأكد من تطابق رمز لغة TextStyle مع اللغة الحالية تماماً
- **التحقق من نمط الفقرة**: إذا كنت تستخدم أنماط الفقرة، تأكد من وجود TextStyle بنمط فقرة مطابق، أو أنشئ نمطاً افتراضياً (نمط فقرة فارغ)
- **التحقق من تعيين الخط**: تأكد من تعيين خط إلى TextStyle
- **تطبيق يدوي**: جرّب استدعاء `textStyle.ApplyToText(textComponent)` مباشرة

### Glyphs الخط المفقودة / أحرف مكسورة

- **استخدم مصنع الخطوط**: أنشئ خطوط TMP باستخدام [مصنع الخطوط](#مصنع-الخطوط-إنشاء-خطوط-tmp) لضمان تغطية glyphs مناسبة
- **التحقق من مجموعات الأحرف**: تأكد من تضمين الخط للنطاقات اللازمة (Latin، العربية، إلخ)
- **Glyphs المفقودة**: سيحذر مصنع الخطوط من glyphs المفقودة - قم بتعيين خط احتياطي للتعامل معها
- **الأحرف العربية لا تتصل**: تأكد من تضمين أشكال العرض العربية الأساسية (U+FE80–U+FEFC) - مصنع الخطوط يتضمن هذه تلقائياً
- **Extended Latin مفقود**: فعّل "تضمين Extended Latin" في مصنع الخطوط للأحرف المشكّلة (é، ñ، ü، إلخ)
- **الخط لا يدعم اللغة**: ملف الخط نفسه يجب أن يحتوي على glyphs - مصنع الخطوط يتضمن النطاقات فقط، وليس glyphs نفسها

### مشاكل تكامل Lingramia

- **ارتباط الملف**: تأكد من ربط ملفات `.locbook` بـ Lingramia
- **مرجع الملف**: تأكد من أن أصل LocBook لديه مرجع ملف `.locbook` صالح
- **أذونات الملف**: تأكد من إمكانية كتابة ملفات `.locbook`
- **تحديث يدوي**: استخدم زر "🔄 Update Asset from .locbook File" إذا فشل التحديث التلقائي

### الصوت/الصورة لا يتم تحميلهما

- **التحقق من نوع الصفحة**: تأكد من أن الإدخالات في صفحات الصوت أو صفحات الصور، وليس صفحات النص
- **التحقق من تعيين الأصل**: تأكد من تعيين أصول Unity إلى المتغيرات
- **التحقق من المفتاح**: تأكد من تطابق المفتاح تماماً (حساس لحالة الأحرف)
- **متغير اللغة**: تأكد من وجود متغير للغة الحالية

### مشاكل الأداء

- **التهيئة المبكرة**: قم بتهيئة الترجمة قبل تحميل المشاهد
- **حد LocBooks**: استخدم LocBooks أقل مع إدخالات أكثر بدلاً من LocBooks صغيرة كثيرة
- **تعطيل التحديث التلقائي**: إذا لم يكن مطلوباً، قم بتعطيل ميزات التحديث التلقائي
- **تخزين الأنماط**: عمليات البحث عن TextStyle يتم تخزينها مؤقتاً، لكن العديد من اللغات قد تؤثر على الأداء

## موارد إضافية

- **Signalia Config**: `Tools > Signalia > Signalia Config`
- **ملخص إعادة هيكلة الترجمة**: راجع `LOCALIZATION_REFACTOR_SUMMARY.md` لملاحظات الترحيل
- **الوصول إلى الإطار**: استخدم فئة `SIGS` للوصول إلى API المبسط
- **وثائق الإطار**: راجع وثائق Signalia Framework للاستخدام المتقدم

---

**ملاحظة**: هذا النظام يتكامل مع إطار Signalia. تأكد من تهيئة الإطار بشكل صحيح قبل استخدام ميزات الترجمة.

</div>
