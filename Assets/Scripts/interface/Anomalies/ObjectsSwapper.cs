using System.Collections.Generic;
using UnityEngine;

public class ObjectsSwapper : AnomalyBase
{
    

    [SerializeField] private List<SwapPair> swaps = new List<SwapPair>();

    [SerializeField] bool isCarved;


    private void Start()
    {
        GetTrees();    
    }

    void GetTrees()
    {
        MonoBehaviour[] allScripts = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (MonoBehaviour script in allScripts)
        {
            if (script is ITree tree)
            {
                swaps.Add(tree.Swapper(isCarved));
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