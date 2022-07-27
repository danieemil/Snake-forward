using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void IAInput()
    {
        Vector2Int playerPos = new Vector2Int((int)game.player.gameObject.transform.position.x, (int)game.player.gameObject.transform.position.y);
        Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        Vector2Int dist = playerPos - currentPos;
        Vector2Int distAbs = new Vector2Int(Mathf.Abs(dist.x), Mathf.Abs(dist.y));

        Vector2Int dir = Vector2Int.zero;

        if (dist.x == 0)
        {
            dir.x = RandomSign();
        }
        else
        {
            dir.x = dist.x / distAbs.x;
        }

        if (dist.y == 0)
        {
            dir.y = RandomSign();
        }
        else
        {
            dir.y = dist.y / distAbs.y;
        }

        int distDir = distAbs.x - distAbs.y;

        if (distDir == 0)
        {
            distDir = RandomSign();
        }


        // Let's try to move towards the player first
        if (distDir > 0)
        {

            lookDir = new Vector2Int(dir.x, 0);
            if (checkMove(lookDir))
            {
                return;
            }

            lookDir = new Vector2Int(0, dir.y);
            if (checkMove(lookDir))
            {
                return;
            }

            lookDir = new Vector2Int(0, -dir.y);
            if (checkMove(lookDir))
            {
                return;
            }

            lookDir = new Vector2Int(-dir.x, 0);
            if (checkMove(lookDir))
            {
                return;
            }

        }
        else
        {
            lookDir = new Vector2Int(0, dir.y);
            if (checkMove(lookDir))
            {
                return;
            }

            lookDir = new Vector2Int(dir.x, 0);
            if (checkMove(lookDir))
            {
                return;
            }

            lookDir = new Vector2Int(-dir.x, 0);
            if (checkMove(lookDir))
            {
                return;
            }

            lookDir = new Vector2Int(0, -dir.y);
            if (checkMove(lookDir))
            {
                return;
            }
        }
    }

    int RandomSign()
    {
        if (Random.Range(0, 2) > 0)
        {
            return 1;
        }

        return 0;
    }

    protected bool checkMove(Vector2Int dir)
    {

        if ((moveDir + dir).magnitude == 0)
        {
            return false;
        }

        Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int nextPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);

        TileType nextTileType = game.map.getTileData(nextPos).tileType;

        if (nextTileType == TileType.Wall || tail.checkCollision(nextPos))
        {
            return false;
        }

        return true;
    }



    void FixedUpdate()
    {
        if (velFrames <= 1)
        {
            velFrames = velCounter;

            IAInput();

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

    override public void Die()
    {
        base.Die();
    }
}
