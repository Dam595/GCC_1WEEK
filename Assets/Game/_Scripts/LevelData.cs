using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LevelData
{
    public int levelNumber;
    public Vector2Data ball;
    public Vector2Data hole;
    public List<TileData> tiles = new List<TileData>();
    public List<ObjectData> obstacles = new List<ObjectData>();
}

[Serializable]
public class Vector2Data { public float x; public float y; public Vector2 ToVector2() => new Vector2(x, y); public static Vector2Data FromVector2(Vector2 v) => new Vector2Data { x = v.x, y = v.y }; }

[Serializable]
public class TileData
{
    public int x;
    public int y;
    public string tileId;
    public string tilemapId;
}

[Serializable]
public class ObjectData
{
    public float x;
    public float y;
    public string type;
}
