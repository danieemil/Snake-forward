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

    public bool CheckCollision(Vector2Int pos)
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

    public void MoveToHead(Vector2 headPos, Quaternion headDirection)
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

    public bool ObjectInTail(Object obj)
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

    public bool ObjectTypeInTail(ObjectType objType)
    {
        foreach (Object obj in objects)
        {
            if (obj.objectType == objType)
            {
                return true;
            }
        }

        return false;
    }

    public void AddObject(Object o)
    {
        int index = objects.Count;
        objects.Insert(index, o);
        o.inTail = this;
    }

    public void RemoveObject(Object o)
    {
        objects.Remove(o);
        o.inTail = null;
    }

    public void DestroyObject(Object o)
    {
        objects.Remove(o);
        Destroy(o.gameObject);
    }

    public bool DestroyObjectType(ObjectType ot)
    {
        foreach (Object obj in objects)
        {
            if (obj.objectType == ot)
            {
                DestroyObject(obj);
                return true;
            }
        }
        return false;
    }

    public void DestroyObjects()
    {
        foreach (Object obj in objects)
        {
            Destroy(obj.gameObject);
        }
        objects.Clear();
    }

    public void ClearObjects()
    {
        foreach (Object obj in objects)
        {
            obj.inTail = null;
        }
        objects.Clear();
    }

    public int GetTailSize()
    {
        return objects.Count;
    }

    public void GiveObjectsTo(Snake snake)
    {
        foreach (Object obj in objects)
        {
            snake.tail.AddObject(obj);
        }

        objects.Clear();
    }
}
