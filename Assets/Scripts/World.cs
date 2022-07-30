using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    [HideInInspector] public PlayerController player;
    [HideInInspector] public TilemapManager tilemap;
    private List<Level> levels;
    private StartPoint start;


    private void Awake()
    {
        levels = new List<Level>();

        player = GetComponentInChildren<PlayerController>();

        tilemap = GetComponentInChildren<TilemapManager>();

        foreach (Level level in GetComponentsInChildren<Level>())
        {
            if (level != null)
            {
                levels.Add(level);
            }
        }

        start = GetComponentInChildren<StartPoint>();
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

    public int GetLevel(Vector2Int pos)
    {
        foreach (Level level in levels)
        {
            if (level.IsInside(pos))
            {
                return level.number;
            }
        }

        return -1;
    }

    public Vector2Int GetStartPosition()
    {
        return new Vector2Int((int)start.transform.position.x, (int)start.transform.position.y);
    }
}
