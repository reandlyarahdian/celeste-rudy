using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D cir;
    private float speed = 10;
    public float wait = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cir = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.tag == "Player")||(collision.gameObject.tag == "Obstacle"))
        {
            StartCoroutine(fall());
        }
    }

    IEnumerator fall ()
    {
        yield return new WaitForSeconds(wait);
        rb.isKinematic = false;
        cir.isTrigger = true;
        rb.velocity = -Vector2.up * speed;
        yield return 0;
    }
}
