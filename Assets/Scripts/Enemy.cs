using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Snake
{
    // Start is called before the first frame update
    void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gamePaused)
        {

        }
    }

    private void FixedUpdate()
    {
        if (velFrames <= 1)
        {
            velFrames = velCounter;

            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int nextPos = new Vector2Int(currentPos.x + actualDir.x, currentPos.y + actualDir.y);

            TileType nextTileType = game.world.tilemap.getTileData(nextPos).tileType;

            if (nextTileType == TileType.Wall || tail.checkCollision(nextPos))
            {
                Die();
                return;
            }

            if (!currentPos.Equals(nextPos))
            {
                tail.moveToHead(transform.position, transform.rotation);
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

    virtual protected void IAInput()
    {

    }

    override public void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
