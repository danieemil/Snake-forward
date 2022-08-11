using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private List<Object> objects = new List<Object>();

    public Snake snakeHead = null;

    public int Size
    {
        get
        { 
            return objects.Count;
        }

        private set
        {

        }
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

    public void MoveToHead(Vector3 headPos, Quaternion headDirection)
    {
        for (int i = objects.Count - 1; i > 0; i--)
        {
            objects[i].transform.position = objects[i - 1].transform.position;
            objects[i].transform.rotation = objects[i - 1].transform.rotation;
        }

        if (objects.Count > 0)
        {
            objects[0].transform.position = headPos;
            objects[0].transform.rotation = headDirection;
        }
    }

    public bool ObjectsOfTypeInTail(ObjectType objType, int quantity = 1)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i].objectType == objType)
            {
                quantity--;
                if (quantity < 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void AddObject(Object o)
    {
        int index = objects.Count;
        objects.Add(o);
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
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i].objectType == ot)
            {
                for (int y = objects.Count - 1; y > i; y--)
                {
                    objects[y].transform.position = objects[y - 1].transform.position;
                    objects[y].transform.rotation = objects[y - 1].transform.rotation;
                }
                DestroyObject(objects[i]);
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

    public void GiveObjectsTo(Tail t)
    {
        foreach (Object obj in objects)
        {
            t.AddObject(obj);
        }

        objects.Clear();
    }
}
