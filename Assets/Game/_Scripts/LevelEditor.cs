using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.IO;

public class LevelEditor : MonoBehaviour
{
    [System.Serializable]
    public class TilemapEntry
    {
        public string id;
        public Tilemap tilemap;
    }

    [Header("Tile registry (map TileBase <-> id)")]
    [SerializeField] private TileRegistry tileRegistry;

    [Header("Tilemaps (layers)")]
    [SerializeField] private TilemapEntry[] tilemaps;

    [Header("Scene objects")]
    [SerializeField] private Transform ball;
    [SerializeField] private Transform hole;
    [SerializeField] private Transform obstaclesParent;

    [Header("Save/Load settings")]
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private string saveFolder = "StreamingAssets/Levels";

    [ContextMenu("Save Level To JSON")]
    public void SaveLevel()
    {
        LevelData data = new LevelData();
        data.levelNumber = levelNumber;
        if (ball != null) data.ball = Vector2Data.FromVector2(ball.position);
        if (hole != null) data.hole = Vector2Data.FromVector2(hole.position);

        foreach (var entry in tilemaps)
        {
            if (entry == null || entry.tilemap == null || string.IsNullOrEmpty(entry.id)) continue;

            Tilemap tm = entry.tilemap;
            BoundsInt bounds = tm.cellBounds;

            foreach (var pos in bounds.allPositionsWithin)
            {
                TileBase tile = tm.GetTile(pos);
                if (tile == null) continue;

                string tileId = tileRegistry != null ? tileRegistry.GetId(tile) : "unknown";
                TileData t = new TileData
                {
                    x = pos.x,
                    y = pos.y,
                    tileId = tileId,
                    tilemapId = entry.id
                };
                data.tiles.Add(t);
            }
        }

        if (obstaclesParent != null)
        {
            foreach (Transform obs in obstaclesParent)
            {
                if (obs == null) continue;
                data.obstacles.Add(new ObjectData
                {
                    x = obs.position.x,
                    y = obs.position.y,
                    type = obs.gameObject.name
                });
            }
        }

        string fullFolder = Path.Combine(Application.dataPath, saveFolder.Replace("StreamingAssets", "StreamingAssets"));
        if (saveFolder.StartsWith("StreamingAssets"))
            fullFolder = Path.Combine(Application.streamingAssetsPath, saveFolder.Substring("StreamingAssets".Length).TrimStart('/', '\\'));
        else
            fullFolder = Path.Combine(Application.dataPath, saveFolder);

        if (!Directory.Exists(fullFolder))
            Directory.CreateDirectory(fullFolder);

        string filePath = Path.Combine(fullFolder, $"level_{levelNumber}.json");

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"[LevelEditor] Saved level {levelNumber} -> {filePath}");
    }

    [ContextMenu("Load Level From JSON")]
    public void LoadLevel()
    {
        string fullFolder;
        if (saveFolder.StartsWith("StreamingAssets"))
            fullFolder = Path.Combine(Application.streamingAssetsPath, saveFolder.Substring("StreamingAssets".Length).TrimStart('/', '\\'));
        else
            fullFolder = Path.Combine(Application.dataPath, saveFolder);

        string filePath = Path.Combine(fullFolder, $"level_{levelNumber}.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError($"[LevelEditor] Level JSON not found: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        LevelData data = JsonUtility.FromJson<LevelData>(json);
        if (data == null)
        {
            Debug.LogError("[LevelEditor] Failed to parse JSON.");
            return;
        }

        foreach (var entry in tilemaps)
        {
            if (entry == null || entry.tilemap == null) continue;
            entry.tilemap.ClearAllTiles();
        }
        foreach (var t in data.tiles)
        {
            Tilemap target = null;
            foreach (var entry in tilemaps)
            {
                if (entry != null && entry.id == t.tilemapId)
                {
                    target = entry.tilemap;
                    break;
                }
            }
            if (target == null) continue;

            TileBase tile = tileRegistry != null ? tileRegistry.GetTileById(t.tileId) : null;
            if (tile != null)
            {
                Vector3Int pos = new Vector3Int(t.x, t.y, 0);
                target.SetTile(pos, tile);
            }
        }

        if (obstaclesParent != null)
        {
            var children = new List<Transform>();
            foreach (Transform c in obstaclesParent) children.Add(c);
            foreach (var c in children) DestroyImmediate(c.gameObject);
        }

        foreach (var obj in data.obstacles)
        {
            GameObject prefab = Resources.Load<GameObject>($"Obstacles/{obj.type}");
            if (prefab != null && obstaclesParent != null)
            {
                Instantiate(prefab, new Vector2(obj.x, obj.y), Quaternion.identity, obstaclesParent);
            }
            else
            {
                Debug.LogWarning($"[LevelEditor] Obstacle prefab not found: {obj.type} (expected in Resources/Obstacles)");
            }
        }

        if (ball != null && data.ball != null)
        {
            ball.position = data.ball.ToVector2();
        }
        if (hole != null && data.hole != null)
        {
            hole.position = data.hole.ToVector2();
        }

        Debug.Log($"[LevelEditor] Loaded level {data.levelNumber} from {filePath}");
    }
}
