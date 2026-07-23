using System.Collections.Generic;
using AHAKuo.Signalia.LocalizationStandalone.Framework;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSettingManager : MonoBehaviour
{


    int indexLang = 0;


    //[SerializeField] TextMeshProUGUI languageTxt;
    [SerializeField] SimpleLocalizedText languageTxt;

    [SerializeField] List<string> languages = new();

    [SerializeField] Button nextLang;
    [SerializeField] Button prevLang;


    private readonly string[] languageList = new string[]
   {
        "Arabic",
        "English"
   };


    private void Awake()
    {
        SIGS.InitializeLocalization();

    }

    private void Start()
    {


        if (nextLang != null)
        {
            nextLang.onClick.AddListener(NextLanguage);
        }

        if (prevLang != null)
        {
            prevLang.onClick.AddListener(PreviouseLanguage);
        }

        for (int i = 0; i < languages.Count; i++)
        {
            Debug.Log("lang: " + languages[i]);

        }

        SetLanguageIndex();
    }



    void SetLanguageIndex()
    {
        string currentL = LocalizationRuntime.CurrentLanguageCode;

        indexLang = languages.FindIndex(l => l == currentL);




    }


    public void PreviouseLanguage()
    {

        indexLang = (indexLang - 1 + languages.Count) % languages.Count;
        SetLanguage();

    }


    public void NextLanguage()
    {

        indexLang = (indexLang + 1) % languages.Count;
        SetLanguage();

    }

    public void SetLanguage()
    {

        LocalizationRuntime.ChangeLanguage(languages[indexLang], save: true);
        languageTxt.SetKey(languageList[indexLang]);

    }
}
