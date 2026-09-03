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
        Debug.Log("upper Start index: " + index);

        SetInGameStory();
        Debug.Log("lower Start index: " + index);
        

    }

    void SetInGameStory()
    {

        if (index > 9) return;

        Debug.Log(textKey + index);

        Debug.Log($"<color=grey>current index: {index} /<color>" );

        simpleText.SetKey(((textKey + index).ToString()));;
        
        index++;

        Debug.Log("lower index: " + index);

    }



    void ResetIndex()
    {

        index = 1;
        SetInGameStory();
        Debug.Log($"<color=red> index: {index} /<color>");

    }


    private void OnDestroy()
    {
        ResetPoint.OnNailsFalls -= SetInGameStory;
        ResetPoint.OnNailsReset -= ResetIndex;
    }
}
