using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Snake
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (velFrames <= 1)
        {
            velFrames = velCounter;

            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int nextPos = new Vector2Int(currentPos.x + lookDir.x, currentPos.y + lookDir.y);

            TileType nextTileType = game.map.getTileData(nextPos).tileType;

            if (nextTileType == TileType.Wall || tail.checkCollision(nextPos))
            {
                Die();
                return;
            }

            if (!currentPos.Equals(nextPos))
            {
                tail.moveToHead(transform.position, transform.rotation);
                moveDir = lookDir;
                transform.position = new Vector3(nextPos.x, nextPos.y);
            }
        }
        else
        {
            velFrames--;
        }
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
