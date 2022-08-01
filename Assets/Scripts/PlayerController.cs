using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputDirBuffer
{
    private const int capacity = 2;
    public int Size { get; private set; } = 0;
    private readonly Vector2Int[] buffer;

    public InputDirBuffer()
    {
        buffer = new Vector2Int[capacity];
        Size = 0;
    }

    public void AddDir(Vector2Int dir)
    {
        if (Size < capacity)
        {
            // If the new direction is the opposite or the same as the previously added one, it won't be added to the buffer
            if (Size > 0)
            {
                if (new Vector2Int(Mathf.Abs(buffer[Size - 1].x) - Mathf.Abs(dir.x), Mathf.Abs(buffer[Size - 1].y) - Mathf.Abs(dir.y)).magnitude == 0)
                {
                    return;
                }
            }

            buffer[Size] = dir;
            Size++;
        }
    }

    public Vector2Int PopDir()
    {
        if (Size > 0)
        {
            Vector2Int dir = buffer[0];

            for (int i = 0; i < Size - 1; i++)
            {
                buffer[i] = buffer[i + 1];
            }

            Size--;

            return dir;
        }
        else
        {
            return Vector2Int.zero;
        }
    }

    public void Clear()
    {
        Size = 0;
    }
}



public class PlayerController : Snake
{

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Enemy corpsePrefab;

    private Enemy corpse = null;


    private bool invencible = false;
    private bool changingLevel = false;

    private readonly InputDirBuffer inputBuffer = new InputDirBuffer();

    private readonly Dictionary<Vector2Int, Func<bool>> dirInputs = new Dictionary<Vector2Int, Func<bool>>()
        {
            {Vector2Int.up, () => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)},
            {Vector2Int.right, () => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)},
            {Vector2Int.down, () => Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)},
            {Vector2Int.left, () => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)}
        };

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
            if (!changingLevel)
            {
                CheckInput();
            }
        }
    }

    void CheckInput()
    {

        Vector2Int tempDir = prevDir;

        if (prevDir.Equals(Vector2Int.zero))
        {
            tempDir = Vector2Int.right;
        }


        List<Vector2Int> dirs = new List<Vector2Int>();
        dirs.Add(new Vector2Int(tempDir.y, -tempDir.x));    // Local Right
        dirs.Add(new Vector2Int(-tempDir.y, tempDir.x));    // Local Left
        dirs.Add(tempDir);                                  // Local Forwards
        dirs.Add(-tempDir);                                 // Local Backwards

        foreach (Vector2Int dir in dirs)
        {
            if(dirInputs[dir]())
            {
                tempDir = dir;
                if ((inputBuffer.Size > 0) || ((tempDir + prevDir).magnitude != 0))
                {
                    inputBuffer.AddDir(tempDir);
                }
            }
        }
    }

    private void FixedUpdate()
    {

        if (velFrames<=1)
        {
            velFrames = velCounter;

            if (inputBuffer.Size > 0)
            {
                actualDir = inputBuffer.PopDir();
            }
            else
            {
                actualDir = prevDir;
            }

            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int nextPos = new Vector2Int(currentPos.x + actualDir.x, currentPos.y + actualDir.y);

            TileType nextTileType = game.world.tilemap.GetTileData(nextPos).tileType;

            if (nextTileType == TileType.Wall || tail.CheckCollision(nextPos))
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
        Vector2Int startPos = game.world.GetStartPosition();
        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);

        base.Restart();

        invencible = false;

        inputBuffer.Clear();
    }

    void NextWorld()
    {
        Restart();
    }

    public override void Die()
    {
        if (tail.GetTailSize() > 0)
        {
            if (corpse == null)
            {
                corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
                corpse.game = game;
                tail.GiveObjectsTo(corpse);
            }

            tail.ClearObjects();
        }
        
        Restart();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            NextWorld();
            return;
        }

        if (collision.gameObject.CompareTag("Level"))
        {
            Bounds bounds = collision.bounds;
            mainCamera.gameObject.transform.position = bounds.center + new Vector3(0, 0, mainCamera.gameObject.transform.position.z);

            changingLevel = true;

            level = collision.gameObject.GetComponent<Level>().number;
            return;
        }

        if (collision.gameObject.CompareTag("Object"))
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

            tail.AddObject(o);
            return;
        }

        if (collision.gameObject.CompareTag("Corpse") || collision.gameObject.CompareTag("Enemy"))
        {
            if (invencible)
            {
                collision.gameObject.GetComponent<Enemy>().Die();
            }
            else
            {
                tail.GiveObjectsTo(collision.gameObject.GetComponent<Enemy>());
                Die();
            }
        }
    }

}
