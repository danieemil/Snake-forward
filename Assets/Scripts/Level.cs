using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    [SerializeField] public int number;
    private BoxCollider2D bCollider;



    private void Awake()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gamePaused)
        {

        }
    }

    public bool IsInside(Vector2Int pos)
    {
        return bCollider.bounds.Contains(new Vector3(pos.x, pos.y, bCollider.bounds.center.z));
    }
}
