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

    protected virtual void AIInput()
    {

    }

    public int RandomSign()
    {
        if (Random.Range(0, 2) > 0)
        {
            return 1;
        }

        return 0;
    }

    protected bool CheckMove(Vector2Int dir)
    {

        if ((prevDir + dir).magnitude == 0)
        {
            return false;
        }

        Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int nextPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);

        TileType nextTileType = game.world.tilemap.GetTileData(nextPos).tileType;

        if (nextTileType == TileType.Wall || tail.CheckCollision(nextPos))
        {
            return false;
        }

        return true;
    }

    override public void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
