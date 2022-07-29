using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [HideInInspector] public World world;

    static public bool gamePaused = false;
    private float startTimeScale;

    private void Awake()
    {
        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        if (!gamePaused)
        {

        }
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
    }

    void TogglePause()
    {
        if (gamePaused)
        {
            Time.timeScale = startTimeScale;
        }
        else
        {
            Time.timeScale = 0;
        }
        gamePaused = !gamePaused;
    }
}
