using System.Collections.Generic;
using UnityEngine;

public class PalmSwapper : AnomalyBase
{
    [SerializeField] private List<SwapPair> swaps = new List<SwapPair>();

    private void Awake()
    {
        SetAnomalyName();
    }

    private void Start()
    {
        GetPalms();
    }

    void GetPalms()
    {
        MonoBehaviour[] allScripts = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (MonoBehaviour script in allScripts)
        {
            if (script is IPalm _palm)
            {
                swaps.Add(_palm.Swapper());
            }
        }
    }

    public void Swap()
    {
        foreach (var pair in swaps)
        {
            if (pair.original != null) pair.original.SetActive(false);
            if (pair.replacement != null) pair.replacement.SetActive(true);
        }
    }

    public void Revert()
    {
        foreach (var pair in swaps)
        {
            if (pair.original != null) pair.original.SetActive(true);
            if (pair.replacement != null) pair.replacement.SetActive(false);
        }
    }

    public override void SetAnomaly()
    {
        Swap();
    }

    public override void ResetAnomaly()
    {
        Revert();
    }
}
