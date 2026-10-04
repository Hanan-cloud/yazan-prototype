using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using AHAKuo.Signalia.LocalizationStandalone.Framework;
using System.Collections.Generic;
using System;
public class UIEvent : MonoBehaviour
{
    [Serializable]
    public class Logos
    {
        public GameObject logo;
        public string lang;
    }


    [SerializeField] List<Logos> logos;


    Dictionary<string, GameObject> logosDic = new Dictionary<string, GameObject>();



    private void Start()
    {
        setDic();
        LocalizationEvents.Subscribe(OnLanguageChanged, gameObject);
    }


    void setDic()
    {
        logosDic.Clear();

        foreach (Logos item in logos)
        {
            if (item.logo != null && !string.IsNullOrEmpty(item.lang))
            {
                if (!logosDic.ContainsKey(item.lang))
                {
                    logosDic.Add(item.lang, item.logo);
                }
                else
                {
                    Debug.LogWarning($"Duplicate lang key: {item.lang}");
                }
            }
        }
        OnLanguageChanged();

    }
    void OnLanguageChanged()
    {
        for (int i = 0; i < logos.Count; i++) 
        {
            logos[i].logo.SetActive(false);

            logosDic[LocalizationRuntime.CurrentLanguageCode].SetActive(true);


        }

    }
}
