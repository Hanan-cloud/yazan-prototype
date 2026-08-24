using System.Collections.Generic;
using UnityEngine;

public class ObjectsSwapper : AnomalyBase
{
    [System.Serializable]
    public class SwapPair
    {
        public GameObject original;
        public GameObject replacement;
    }

    [SerializeField] private List<SwapPair> swaps = new List<SwapPair>();

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