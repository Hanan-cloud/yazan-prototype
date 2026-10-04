using UnityEngine;

public class Tree : MonoBehaviour, ITree
{

    SwapPair pair = new();
    SwapPair pairHollowd = new();

    [SerializeField] GameObject Log;
    [SerializeField] GameObject CarvedLog;
    [SerializeField] GameObject HollowedLog;

    public SwapPair Swapper(bool isCarved)
    {


        pair.original = pairHollowd.original= Log;
        pair.replacement = CarvedLog;

        pairHollowd.replacement = HollowedLog;

        if (isCarved)
        {
            return pair;


        }
        else return pairHollowd;



    }





}
