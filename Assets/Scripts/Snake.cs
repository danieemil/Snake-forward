using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Movable
{

    [SerializeField] public Tail tail;


    private void Awake()
    {
        tail.snakeHead = this;
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

    void FixedUpdate()
    {
        if (velFrames <= 1)
        {
            velFrames = velCounter;

            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int nextPos = new Vector2Int(currentPos.x + actualDir.x, currentPos.y + actualDir.y);

            TileType nextTileType = game.world.tilemap.GetTileData(nextPos).tileType;

            if (nextTileType == TileType.Wall || tail.CheckCollision(nextPos))
            {
                Die();
                return;
            }

            if (!currentPos.Equals(nextPos))
            {
                tail.MoveToHead(transform.position, transform.rotation);
                transform.position = new Vector3(nextPos.x, nextPos.y);
                prevDir = actualDir;
            }
        }
        else
        {
            velFrames--;
        }
    }

    protected override void Restart()
    {
        base.Restart();
    }

    public override void Pause()
    {

    }

    public virtual void Die()
    {
        tail.ClearObjects();
    }
}
