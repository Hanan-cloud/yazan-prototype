using UnityEngine;
using System.Collections.Generic;
public class BlockManager : MonoBehaviour
{
    

    [System.Serializable]
    public class Block
    {
        public Transform parent;
        public List<SpriteRenderer> sprites;

        [HideInInspector] public float minX;
        [HideInInspector] public float maxX;
    }

    [SerializeField] private Block blockA;
    [SerializeField] private Block blockB;

    private void Awake()
    {
        CalculateBounds(blockA);
        CalculateBounds(blockB);
    }

    private void CalculateBounds(Block block)
    {
        if (block.sprites == null || block.sprites.Count == 0)
        {
            Debug.LogError($"No sprites assigned to {block.parent.name}");
            return;
        }

        float parentX = block.parent.position.x;

        block.minX = float.MaxValue;
        block.maxX = float.MinValue;

        foreach (SpriteRenderer sprite in block.sprites)
        {
            if (sprite == null)
                continue;

            float min = sprite.bounds.min.x - parentX;
            float max = sprite.bounds.max.x - parentX;

            block.minX = Mathf.Min(block.minX, min);
            block.maxX = Mathf.Max(block.maxX, max);
        }
    }

    public void MoveAToRightOfB()
    {
        float targetX = blockB.parent.position.x + blockB.maxX;
        float currentX = blockA.parent.position.x + blockA.minX;

        blockA.parent.position += Vector3.right * (targetX - currentX);
    }

    public void MoveAToLeftOfB()
    {
        float targetX = blockB.parent.position.x + blockB.minX;
        float currentX = blockA.parent.position.x + blockA.maxX;

        blockA.parent.position += Vector3.right * (targetX - currentX);
    }

    public void MoveBToRightOfA()
    {
        float targetX = blockA.parent.position.x + blockA.maxX;
        float currentX = blockB.parent.position.x + blockB.minX;

        blockB.parent.position += Vector3.right * (targetX - currentX);
    }

    public void MoveBToLeftOfA()
    {
        float targetX = blockA.parent.position.x + blockA.minX;
        float currentX = blockB.parent.position.x + blockB.maxX;

        blockB.parent.position += Vector3.right * (targetX - currentX);
    }


}
