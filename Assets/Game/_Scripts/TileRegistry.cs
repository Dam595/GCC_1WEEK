using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "LevelEditor/TileRegistry")]
public class TileRegistry : ScriptableObject
{
    [System.Serializable]
    public class TileEntry
    {
        public string id;
        public TileBase tile;
    }
    public List<TileEntry> tiles = new List<TileEntry>(256);

    private Dictionary<TileBase, string> tileToId;
    private Dictionary<string, TileBase> idToTile;

    private void EnsureLookup()
    {
        if(tileToId != null && idToTile != null) {
            return;
        }
        tileToId = new Dictionary<TileBase, string>();
        idToTile = new Dictionary<string, TileBase>();

        foreach(var e in tiles)
        {
            if(e.tile == null || string.IsNullOrEmpty(e.id)) continue;
            tileToId[e.tile] = e.id;
            idToTile[e.id] = e.tile;
        }
    }

    public string GetId(TileBase tile)
    {
        EnsureLookup();
        return tile != null && tileToId.TryGetValue(tile, out var id) ? id : "unknown";
    }
    public TileBase GetTileById(string id)
    {
        EnsureLookup();
        return idToTile.TryGetValue(id, out var tile) ? tile : null;
    }
}
