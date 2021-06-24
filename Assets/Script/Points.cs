using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    public Vector3 respawnPosition;

    void OnTriggerEnter2D(Collider2D hit)

    {
        if (hit.tag == "CheckPoint")
        {
            respawnPosition = transform.position;
        }
        if (hit.tag == "Fall")
        {
            transform.position = respawnPosition;
        }
    }
}
