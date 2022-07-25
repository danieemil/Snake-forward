using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectType
{
    Generic = 0,
    Key = 1,
}


public class Object : MonoBehaviour
{

    public ObjectType objectType;

    public Tail inTail = null;

    // Start is called before the first frame update
    void Start()
    {
        inTail = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
