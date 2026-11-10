
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPUnderlineFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text targetText;

    private RectTransform line;

    [Header("Settings")]
    [SerializeField] private float verticalGap = 3f;
    [SerializeField] private float extraWidth = 1f;

    private void Awake()
    {
        line = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetText == null || line == null)
            return;

        targetText.ForceMeshUpdate();

        TMP_TextInfo textInfo = targetText.textInfo;

        if (textInfo.characterCount == 0)
        {
            line.gameObject.SetActive(false);
            return;
        }

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;

        bool foundCharacter = false;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            minX = Mathf.Min(minX, charInfo.bottomLeft.x);
            maxX = Mathf.Max(maxX, charInfo.topRight.x);
            minY = Mathf.Min(minY, charInfo.bottomLeft.y);

            foundCharacter = true;
        }

        if (!foundCharacter)
        {
            line.gameObject.SetActive(false);
            return;
        }

        line.gameObject.SetActive(true);

        float textWidth = maxX - minX;

        line.sizeDelta = new Vector2(
            textWidth + extraWidth,
            line.sizeDelta.y
        );

        line.anchoredPosition = new Vector2(
            (minX + maxX) * 0.5f,
            minY - verticalGap
        );
    }
}