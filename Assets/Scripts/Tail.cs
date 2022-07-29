using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private List<Object> objects = new List<Object>();

    public Snake snakeHead = null;

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

    public bool checkCollision(Vector2Int pos)
    {
        for (int i = 0; i < objects.Count - 1; i++)
        {
            Vector3 v3 = objects[i].gameObject.transform.position;
            Vector2Int vpos = new Vector2Int((int)v3.x, (int)v3.y);
            if (vpos.Equals(pos))
            {
                return true;
            }
        }

        return false;
    }

    public void moveToHead(Vector2 headPos, Quaternion headDirection)
    {
        for (int i = objects.Count - 1; i > 0; i--)
        {
            objects[i].transform.position = objects[i - 1].transform.position;
            objects[i].transform.rotation = objects[i - 1].transform.rotation;
        }

        if (objects.Count > 0)
        {
            objects[0].transform.position = new Vector3(headPos.x, headPos.y, objects[0].transform.position.z);
            objects[0].transform.rotation = headDirection;
        }
    }

    public bool objectInTail(Object obj)
    {
        foreach (Object o in objects)
        {
            if (o == obj)
            {
                return true;
            }
        }
        return false;
    }

    public Object getObjectType(ObjectType objType)
    {
        foreach (Object obj in objects)
        {
            if (obj.objectType == objType)
            {
                return obj;
            }
        }

        return null;
    }

    public void addObject(Object o)
    {
        int index = objects.Count;
        objects.Insert(index, o);
        o.inTail = this;
    }

    public void removeObject(Object o)
    {
        objects.Remove(o);
        o.inTail = null;
    }

    public void destroyObject(Object o)
    {
        objects.Remove(o);
        o.inTail = null;
        Destroy(o);
    }

    public void clearObjects()
    {
        foreach (Object obj in objects)
        {
            obj.inTail = null;
        }
        objects.Clear();
    }

    public int getTailSize()
    {
        return objects.Count;
    }

    public void giveObjectsTo(Snake snake)
    {
        foreach (Object obj in objects)
        {
            snake.tail.addObject(obj);
        }

        objects.Clear();
    }
}
