using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int initialSpawnCount = 1;

    private readonly List<Vector2> _edgeWorldPositions = new();

    void Awake()
    {
        if (groundTilemap == null)
        {
            Debug.LogError("[EnemySpawner] Ground Tilemap reference is missing.");
            return;
        }

        groundTilemap.CompressBounds();
        var b = groundTilemap.cellBounds;

        //Bottom and top
        for (int x = b.xMin; x < b.xMax; x++)
        {
            TryAddEdgeCell(new Vector3Int(x, b.yMin, 0));
            if (b.yMax - 1 != b.yMin)
                TryAddEdgeCell(new Vector3Int(x, b.yMax - 1, 0));
        }

        //Left and right
        for (int y = b.yMin + 1; y < b.yMax - 1; y++)
        {
            TryAddEdgeCell(new Vector3Int(b.xMin, y, 0));
            if (b.xMax - 1 != b.xMin)
                TryAddEdgeCell(new Vector3Int(b.xMax - 1, y, 0));
        }

        if (_edgeWorldPositions.Count == 0)
            Debug.LogWarning("[EnemySpawner] No edge tiles found on the GroundTileMap.");

    }

    void Start()
    {
        for (int i = 0; i < initialSpawnCount; i++)
            SpawnOne();
    }

    public GameObject SpawnOne()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[EnemySpawner] Enemy prefab is not assigned.");
            return null;
        }
        if (_edgeWorldPositions.Count == 0)
        {
            Debug.LogWarning("[EnemySpawner] No valid edge positions to spawn.");
            return null;
        }

        Vector2 pos = _edgeWorldPositions[Random.Range(0, _edgeWorldPositions.Count)];

        // Instantiate and place using pure 2D (Rigidbody2D.position)
        GameObject enemy = Instantiate(enemyPrefab);
        var rb2d = enemy.GetComponent<Rigidbody2D>();
        if (rb2d != null)
            rb2d.position = pos;
        else
            enemy.transform.position = new Vector3(pos.x, pos.y, 0f); // fallback if no Rigidbody2D

        return enemy;
    }

    private void TryAddEdgeCell(Vector3Int cell)
    {
        if (groundTilemap.HasTile(cell))
        {
            Vector3 world3 = groundTilemap.GetCellCenterWorld(cell);
            _edgeWorldPositions.Add((Vector2)world3); // drop Z immediately
        }
    }
}
