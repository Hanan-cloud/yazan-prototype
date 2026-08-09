using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using TMPro;


public class InGameStoryPanel : MonoBehaviour
{

     string textKey = "InGameStory_";
    int index;
    [SerializeField] TextMeshProUGUI text;


    private void Start()
    {
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

        text.SetLocalizedText((textKey + index).ToString());
        index++;

        Debug.Log("lower index: " + index);

    }



    void ResetIndex()
    {

        index = 1;
        Debug.Log($"<color=red> index: {index} /<color>");

    }


    private void OnDestroy()
    {
        ResetPoint.OnNailsFalls -= SetInGameStory;
        ResetPoint.OnNailsReset -= ResetIndex;
    }
}
