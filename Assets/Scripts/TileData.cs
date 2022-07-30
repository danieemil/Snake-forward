using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



// This script is only to add custom data to the tiles on the tilemap.


public enum TileType
{
    Void = 0,
    Wall = 1,

}

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public TileType tileType = TileType.Void; 
}
