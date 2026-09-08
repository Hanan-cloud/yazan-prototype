using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using TMPro;


public class InGameStoryPanel : MonoBehaviour
{

     string textKey = "InGameStory_";
    int index;
    [SerializeField] GameObject text;
    SimpleLocalizedText simpleText;

    private void Start()
    {
        simpleText = text.GetComponent<SimpleLocalizedText>();
        index = 1;
        ResetPoint.OnNailsFalls += SetInGameStory;
        ResetPoint.OnNailsReset += ResetIndex;

        SetInGameStory();
        

    }

    void SetInGameStory()
    {

        if (index > 9) return;

    

        simpleText.SetKey(((textKey + index).ToString()));;
        
        index++;


    }



    void ResetIndex()
    {

        index = 1;
        SetInGameStory();

    }


    private void OnDestroy()
    {
        ResetPoint.OnNailsFalls -= SetInGameStory;
        ResetPoint.OnNailsReset -= ResetIndex;
    }
}
