using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFol : MonoBehaviour
{
    public GameObject virCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            virCam.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            virCam.SetActive(false);
        }
    }
}
