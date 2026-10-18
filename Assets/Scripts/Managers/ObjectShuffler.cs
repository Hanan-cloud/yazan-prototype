
using System.Collections.Generic;
using UnityEngine;

public class ObjectShuffler : MonoBehaviour
{
    [SerializeField] List<Transform> objects = new List<Transform>();

    [ContextMenu("Shuffle Objects")]
    public void ShuffleObjects()
    {
        int count = objects.Count;

        if (count <= 1)
            return;

        // Save original positions before shuffling
        Vector3[] positions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            positions[i] = objects[i].position;
        }

        // Shuffle the list
        for (int i = count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            (objects[i], objects[randomIndex]) =
                (objects[randomIndex], objects[i]);
        }

        // Assign the saved positions in shuffled order
        for (int i = 0; i < count; i++)
        {
            objects[i].position = positions[i];
        }
    }
}
