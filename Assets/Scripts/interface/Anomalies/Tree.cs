using UnityEngine;

public class Tree : MonoBehaviour, ITree
{

    SwapPair pair = new();

    [SerializeField] GameObject Log;
    [SerializeField] GameObject CarvedLog;
    [SerializeField] GameObject HollowedLog;

    public SwapPair Swapper(bool isCarved)
    {
        pair.original = Log;

        if (isCarved)
            pair.replacement = CarvedLog;
        else
            pair.replacement = HollowedLog;


        return pair;
    }




 
}
