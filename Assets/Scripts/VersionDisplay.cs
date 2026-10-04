using UnityEngine;
using TMPro;

public class VersionDisplay : MonoBehaviour
{
     private TextMeshProUGUI versionText;
    [SerializeField] private string prefix = "v";

    void Start()
    {
        versionText = GetComponent<TextMeshProUGUI>();
        versionText.text = prefix + Application.version;
    }
}