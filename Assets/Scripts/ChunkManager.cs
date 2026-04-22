using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk Setup")]
    public List<GameObject> chunkPrefabs = new List<GameObject>();
    public float chunkWidth = 20f;

    [Range(3, 11)]
    public int visibleChunksCount = 5;

    [Header("Player")]
    public Transform player;

    [Range(0f, 1f)]
    public float playerLocalX = 0.15f;

    [Header("Scroll Speed")]
    public float moveSpeed = 5f;

    [Header("Debug")]
    public bool showDebugGizmos = true;

    // --- private ---
    private List<GameObject> _activeChunks = new List<GameObject>();
    private float _scrollOffset = 0f;
    private float _totalWidth;
    private float _halfTotal;
    private int _loopCount = 0;
    private bool _movingRight = true;
    private int _nextPrefabIdx = 0;

    public System.Action<int> OnLoopCompleted;
    public System.Action OnAnomalyZone;

    // -------------------------------------------------------
    void Start()
    {
        ValidateSetup();
        CalculateDimensions();
        SpawnInitialChunks();
        PositionPlayer();
    }

    void Update()
    {
        HandleInput();
        UpdateChunkPositions();
    }

    // -------------------------------------------------------
    void ValidateSetup()
    {
        if (chunkPrefabs.Count == 0) { enabled = false; return; }
        if (player == null) { enabled = false; }
    }

    void CalculateDimensions()
    {
        _totalWidth = chunkWidth * visibleChunksCount;
        _halfTotal = _totalWidth / 2f;
    }

    void SpawnInitialChunks()
    {
        _activeChunks.Clear();
        for (int i = 0; i < visibleChunksCount; i++)
        {
            int prefabIdx = GetNextPrefabIndex();
            GameObject chunk = SpawnChunk(prefabIdx, 0f);
            _activeChunks.Add(chunk);
        }
    }

    void PositionPlayer()
    {
        float leftEdge = -_halfTotal;
        float pX = leftEdge + (chunkWidth * playerLocalX);
        if (player != null)
        {
            Vector3 pos = player.position;
            pos.x = pX;
            player.position = pos;
        }
    }

    // -------------------------------------------------------
    void HandleInput()
    {
        float input = 0f;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            input = 1f;
        else if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            input = -1f;

        if (input > 0f)
        {
            bool wasRight = _movingRight;
            _movingRight = true;
            _scrollOffset -= moveSpeed * Time.deltaTime;
        }
        else if (input < 0f)
        {
            _movingRight = false;
            _scrollOffset += moveSpeed * Time.deltaTime;
        }
    }

    // -------------------------------------------------------
    // Core: each chunk gets a slot index 0..N-1
    // slot world position = (slot * chunkWidth) - halfTotal + halfChunk + scrollOffset
    // then we wrap it so it always stays in the visible ring
    // -------------------------------------------------------
    void UpdateChunkPositions()
    {
        float halfChunk = chunkWidth / 2f;

        for (int i = 0; i < _activeChunks.Count; i++)
        {
            // base position for this slot
            float x = (i * chunkWidth) - _halfTotal + halfChunk + _scrollOffset;

            // wrap into the ring  [-halfTotal - halfChunk , halfTotal + halfChunk]
            float rangeMin = -_halfTotal - halfChunk;
            float rangeMax = _halfTotal + halfChunk;
            float range = _totalWidth;

            // bring x into range using modulo math
            x = x - range * Mathf.Floor((x - rangeMin) / range);

            // clamp floating point drift
            if (x > rangeMax) x -= range;
            if (x < rangeMin) x += range;

            Vector3 pos = _activeChunks[i].transform.position;
            pos.x = x;
            _activeChunks[i].transform.position = pos;
        }

        // detect loop completion: when scrollOffset crosses a full totalWidth
        CheckLoopComplete();
    }

    // -------------------------------------------------------
    float _lastLoopThreshold = 0f;

    void CheckLoopComplete()
    {
        if (_movingRight)
        {
            float crossed = Mathf.Floor(-_scrollOffset / _totalWidth);
            if (crossed > _lastLoopThreshold)
            {
                _lastLoopThreshold = crossed;
                _loopCount++;
                OnLoopCompleted?.Invoke(_loopCount);
            }
        }
        else
        {
            float crossed = Mathf.Floor(_scrollOffset / _totalWidth);
            if (crossed > _lastLoopThreshold)
            {
                _lastLoopThreshold = crossed;
            }
        }
    }

    // -------------------------------------------------------
    int GetNextPrefabIndex()
    {
        int idx = _nextPrefabIdx % chunkPrefabs.Count;
        _nextPrefabIdx++;
        return idx;
    }

    GameObject SpawnChunk(int prefabIndex, float xPos)
    {
        GameObject prefab = chunkPrefabs[prefabIndex];
        Vector3 pos = new Vector3(xPos, 0f, 0f);
        GameObject chunk = Instantiate(prefab, pos, Quaternion.identity, transform);
        chunk.name = $"Chunk_{prefabIndex}_slot{_activeChunks.Count}";
        return chunk;
    }

    // -------------------------------------------------------
    // Public API
    // -------------------------------------------------------
    public float GetScrollOffset() => _scrollOffset;
    public int GetLoopCount() => _loopCount;
    public bool IsMovingRight() => _movingRight;
    public void FreezeScroll() => enabled = false;
    public void ResumeScroll() => enabled = true;

    // -------------------------------------------------------
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;
        CalculateDimensions();

        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_totalWidth, 5f, 0f));

        Gizmos.color = Color.yellow;
        float pX = -_halfTotal + (chunkWidth * playerLocalX);
        Gizmos.DrawLine(new Vector3(pX, -3f, 0f), new Vector3(pX, 3f, 0f));

        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        for (int i = 0; i <= visibleChunksCount; i++)
        {
            float x = -_halfTotal + (i * chunkWidth);
            Gizmos.DrawLine(new Vector3(x, -2.5f, 0f), new Vector3(x, 2.5f, 0f));
        }
    }
#endif
}