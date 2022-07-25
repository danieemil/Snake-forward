using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{


    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileData[] tileDatas;

    public StartPoint start;

    // The tile dimensions are 1x1 (check assets configurations -> 72 px = 1 unit)
    private Vector2Int tileSize = new Vector2Int(1, 1);

    private Dictionary<TileBase, TileData> tilesDictionary = new Dictionary<TileBase, TileData>();

    private void Awake()
    {
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                tilesDictionary.Add(tile, tileData);
            }
        }
    }

    public TileData getTileData(Vector2Int pos)
    {
        TileBase tb = tilemap.GetTile(new Vector3Int(pos.x * tileSize.x, pos.y * tileSize.y, 0));
        return tilesDictionary[tb];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
