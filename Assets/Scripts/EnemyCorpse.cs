using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : Enemy
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

    protected override void IAInput()
    {
        Vector2Int playerPos = new Vector2Int((int)game.world.player.gameObject.transform.position.x, (int)game.world.player.gameObject.transform.position.y);
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

            actualDir = new Vector2Int(dir.x, 0);
            if (checkMove(actualDir))
            {
                return;
            }

            actualDir = new Vector2Int(0, dir.y);
            if (checkMove(actualDir))
            {
                return;
            }

            actualDir = new Vector2Int(0, -dir.y);
            if (checkMove(actualDir))
            {
                return;
            }

            actualDir = new Vector2Int(-dir.x, 0);
            if (checkMove(actualDir))
            {
                return;
            }
            
        }
        else
        {
            actualDir = new Vector2Int(0, dir.y);
            if (checkMove(actualDir))
            {
                return;
            }

            actualDir = new Vector2Int(dir.x, 0);
            if (checkMove(actualDir))
            {
                return;
            }

            actualDir = new Vector2Int(-dir.x, 0);
            if (checkMove(actualDir))
            {
                return;
            }

            actualDir = new Vector2Int(0, -dir.y);
            if (checkMove(actualDir))
            {
                return;
            }
        }
        actualDir = Vector2Int.zero;
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

        if ((prevDir + dir).magnitude == 0)
        {
            return false;
        }

        Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int nextPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);

        TileType nextTileType = game.world.tilemap.getTileData(nextPos).tileType;

        if (nextTileType == TileType.Wall || tail.checkCollision(nextPos))
        {
            return false;
        }

        return true;
    }



    void FixedUpdate()
    {
        if (game.world.player.level != level)
        {
            return;
        }

        if (velFrames <= 1)
        {
            velFrames = velCounter;

            IAInput();

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

    override public void Die()
    {
        base.Die();
    }
}
