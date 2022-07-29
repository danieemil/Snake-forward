using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{

    [SerializeField] public GameManager game;

    [SerializeField] protected int velCounter = 50;

    [HideInInspector] public int level;
    protected Vector2Int prevDir;
    protected Vector2Int actualDir;
    protected int velFrames;


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
            CheckInput();
        }
    }

    void CheckInput()
    {

    }

    void FixedUpdate()
    {
        if (velFrames <= 1)
        {
            velFrames = velCounter;

            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            Vector2Int nextPos = new Vector2Int(currentPos.x + actualDir.x, currentPos.y + actualDir.y);

            transform.position = new Vector3(nextPos.x, nextPos.y);
            prevDir = actualDir;

        }
        else
        {
            velFrames--;
        }
    }

    virtual protected void Restart()
    {
        prevDir = Vector2Int.zero;
        actualDir = prevDir;

        velFrames = velCounter;

        level = game.world.getLevel(new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Level"))
        {
            level = collision.gameObject.GetComponent<Level>().number;
        }
    }

    virtual public void Pause()
    {

    }
}
