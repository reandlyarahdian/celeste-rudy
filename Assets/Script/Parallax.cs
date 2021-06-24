using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject[] levels;
    public Transform main;
    private Vector2 screen;
    public float choke;

    private Vector3 lastScreenPos;

    private void Start()
    {
        screen = main.TransformPoint(new Vector3(Screen.width, Screen.height, main.transform.position.z));
        foreach(GameObject obj in levels)
        {
            loadchild(obj);
        }
        lastScreenPos = transform.position;
    }
    void loadchild (GameObject obj)
    {
        float width = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childNeeded = (int)Mathf.Ceil(screen.x * 2 / width);
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= childNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(width * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }
    void repositionChildObject(GameObject obj)
    {
        Transform[] childern = obj.GetComponentsInChildren<Transform>();
        if (childern.Length > 1)
        {
            GameObject first = childern[1].gameObject;
            GameObject last = childern[childern.Length - 1].gameObject;
            float half = last.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if(transform.position.x + screen.x > last.transform.position.x + half)
            {
                first.transform.SetAsLastSibling();
                first.transform.position = new Vector3(last.transform.position.x + half * 2, last.transform.position.y, last.transform.position.z);
            }else if(transform.position.x - screen.x < first.transform.position.x - half)
            {
                last.transform.SetAsFirstSibling();
                last.transform.position = new Vector3(last.transform.position.x + half * 2, last.transform.position.y, last.transform.position.z);
            }
        }
    }
    private void LateUpdate()
    {
        foreach(GameObject obj in levels)
        {
            repositionChildObject(obj);
            float parallaxSpeed = 1 -  Mathf.Clamp01(Mathf.Abs(transform.position.z / obj.transform.position.z));
            float diff = transform.position.x - lastScreenPos.x;
            obj.transform.Translate(Vector3.right * diff * parallaxSpeed);
        }
        lastScreenPos = transform.position;
    }
}
