using System.Collections.Generic;
using UnityEngine;

public class ObjectsSwapper : AnomalyBase
{
    

    [SerializeField] private List<SwapPair> swaps = new List<SwapPair>();

    [SerializeField] bool isCarved;
    int swapIndex = 0;
    private void Awake()
    {
        SetAnomalyName();
    }
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
        swapIndex = UnityEngine.Random.Range(0, swaps.Count);
        
        swaps[swapIndex].original.SetActive(false);
        swaps[swapIndex].replacement.SetActive(true);
    }

    public void Revert()
    {
        swaps[swapIndex].original.SetActive(true);
        swaps[swapIndex].replacement.SetActive(false);


        swapIndex = 0;
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