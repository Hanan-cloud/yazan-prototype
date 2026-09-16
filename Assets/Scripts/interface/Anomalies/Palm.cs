using UnityEngine;

public class Palm : MonoBehaviour, IPalm
{

    SwapPair pair = new();

    [SerializeField] GameObject OriginalPalm;
    [SerializeField] GameObject AnomalyPalm;

    public SwapPair Swapper()
    {
        pair.original = OriginalPalm;
        pair.replacement = AnomalyPalm;


        return pair;
    }

}