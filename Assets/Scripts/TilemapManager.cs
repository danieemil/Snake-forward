using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{


    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileData[] tileDatas;

    private TileBase tileFloor;

    // The tile dimensions are 1x1 (check assets configurations -> 72 px = 1 unit)
    private Vector2Int tileSize = new Vector2Int(1, 1);

    // Allows getting custom data from tiles
    private readonly Dictionary<TileBase, TileData> tilesDictionary = new Dictionary<TileBase, TileData>();

    private void Awake()
    {
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                tilesDictionary.Add(tile, tileData);
            }
        }

        foreach (var tileData in tileDatas)
        {
            if (tileData.tileType == TileType.Void)
            {
                tileFloor = tileData.tiles[0];
            }
        }
    }

    public TileData GetTileData(Vector2Int pos)
    {
        TileBase tb = tilemap.GetTile(new Vector3Int(pos.x * tileSize.x, pos.y * tileSize.y, 0));
        TileData td = tilesDictionary[tb];
        return td;
    }

    public void RemoveDoorTiles(Vector2Int min, Vector2Int max)
    {
        Vector2 size = max - min;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (GetTileData(new Vector2Int(x + min.x, y + min.y)).tileType == TileType.Door)
                {
                    tilemap.SetTile(new Vector3Int(x + min.x, y + min.y, 0), tileFloor);
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gamePaused)
        {

        }
    }
}
