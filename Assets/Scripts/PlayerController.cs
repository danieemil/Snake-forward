using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Snake
{

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Enemy corpsePrefab;

    private Enemy corpse = null;



    private bool invencible = false;
    private bool changingLevel = false;

    

    // Start is called before the first frame update
    void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Update()
    {

        if (changingLevel)
        {
            return;
        }

        Vector2Int tempDir = lookDir;

        CheckInput();

        if ((lookDir + moveDir).magnitude == 0)
        {
            lookDir = tempDir;
        }
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lookDir = Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            lookDir = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            lookDir = Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            lookDir = Vector2Int.right;
        }
    }

    private void FixedUpdate()
    {
        if (velFrames<=1)
        {
            velFrames = velCounter;

            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int nextPos = new Vector2Int(currentPos.x + lookDir.x, currentPos.y + lookDir.y);

            TileType nextTileType = game.map.getTileData(nextPos).tileType;

            if (nextTileType == TileType.Wall || tail.checkCollision(nextPos))
            {
                if (!invencible)
                {
                    Die();
                }
                return;
            }

            if (changingLevel)
            {
                changingLevel = false;
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

    protected void Restart()
    {
        transform.position = game.map.start.gameObject.transform.position;
        moveDir = game.map.start.startDirection;
        lookDir = moveDir;

        velFrames = velCounter;

        invencible = false;
    }

    void NextWorld()
    {
        Restart();
    }

    override public void Die()
    {
        if (tail.getTailSize() > 0)
        {
            if (corpse == null)
            {
                corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
                corpse.game = game;
                tail.giveObjectsTo(corpse);
            }

            tail.clearObjects();
        }
        
        Restart();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Finish"))
        {
            NextWorld();
            return;
        }

        if (collision.gameObject.tag.Equals("Level"))
        {
            Bounds bounds = collision.bounds;
            mainCamera.gameObject.transform.position = bounds.center + new Vector3(0, 0, mainCamera.gameObject.transform.position.z);

            changingLevel = true;
            return;
        }

        if (collision.gameObject.tag.Equals("Object"))
        {
            Object o = collision.gameObject.GetComponent<Object>();

            if (o.inTail != null)
            {
                if (o.inTail.snakeHead.gameObject.tag.Equals("Corpse"))
                {
                    corpse = null;
                }
                o.inTail.snakeHead.Die();
            }

            tail.addObject(o);
            return;
        }

        if (collision.gameObject.tag.Equals("Corpse") || collision.gameObject.tag.Equals("Enemy"))
        {
            if (invencible)
            {
                collision.gameObject.GetComponent<Enemy>().Die();
            }
            else
            {
                tail.giveObjectsTo(collision.gameObject.GetComponent<Enemy>());
                Die();
            }
        }
    }

}
