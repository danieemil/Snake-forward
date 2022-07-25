using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{

    [SerializeField] public GameManager game;

    [SerializeField] protected int velCounter = 50;

    protected Vector2Int moveDir;
    protected Vector2Int lookDir;
    protected int velFrames;

    // Start is called before the first frame update
    void Start()
    {
        velFrames = velCounter;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
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
            Vector2Int nextPos = new Vector2Int(currentPos.x + lookDir.x, currentPos.y + lookDir.y);

            moveDir = lookDir;
            transform.position = new Vector3(nextPos.x, nextPos.y);

        }
        else
        {
            velFrames--;
        }
    }
}
