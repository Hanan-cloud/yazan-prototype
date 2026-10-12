using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    /// <summary>
    /// Shuffles the elements of a list in-place using the Fisher-Yates algorithm.
    /// </summary>
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            // Unity's Random.Range is max-exclusive for integers, so we use n + 1
            int k = Random.Range(0, n + 1);

            // Swap elements
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}