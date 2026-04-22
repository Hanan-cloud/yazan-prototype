using UnityEngine;

public class ParallaxL : MonoBehaviour
{
    [Header("Chunk Scroller Reference")]
    public ChunkManager chunkManager;

    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    public float parallaxFactor = 0.3f;

    [Header("Tiling")]
    public bool tileHorizontally = true;
    public float textureWidth = 20f;

    private float _startX;
    private float _lastScrollOffset;

    void Start()
    {
        if (chunkManager == null)
        {
            Debug.LogError("[ParallaxLayer] No ChunkManager assigned on " + gameObject.name);
            enabled = false;
            return;
        }

        _startX = transform.position.x;
        _lastScrollOffset = chunkManager.GetScrollOffset();
    }

    void LateUpdate()
    {
        float currentOffset = chunkManager.GetScrollOffset();
        float delta = currentOffset - _lastScrollOffset;
        _lastScrollOffset = currentOffset;

        float moveX = delta * parallaxFactor;

        Vector3 pos = transform.position;
        pos.x += moveX;

        if (tileHorizontally && textureWidth > 0f)
        {
            pos.x = _startX + Mathf.Repeat(pos.x - _startX, textureWidth);
        }

        transform.position = pos;
    }
}